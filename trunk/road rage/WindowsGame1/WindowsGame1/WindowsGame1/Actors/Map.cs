using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using TangoGames.RoadFighter.Levels;
using TangoGames.RoadFighter.Actors ;


namespace TangoGames.RoadFighter.Actors
{
    public interface IMap
    {
        void Add(IDrawableActor actor);
        void Remove(IDrawableActor actor);
        void Update(GameTime gameTime);
        void Draw(GameTime gameTime);
        Vector2 Velocity {get; set; }
        int MaxSpeed { get; set; }
        event EventHandler<CollisionEventArgs > ColisionsOccours;
        event EventHandler<OutOfBoundsEventArgs> OutOfBounds;
    }

    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Map : DrawableGameComponent, IMap
    {
        private float _acceleration = 0.05F;
        private int _maxSpeed = 15;

        private List<IDrawableActor> _safeRemoveList;

        public Map(Game game)
            : base(game)
        {
            spritebatch = new SpriteBatch(game.GraphicsDevice);
            current = new StraightRoad(game, new Vector2(1024, 1024));
            current.SpriteBatch = spritebatch;
            current.Location = Vector2.Zero;

            next = new StraightRoad(game, new Vector2(1024, 1024));
            next.SpriteBatch = spritebatch;
            next.Location = new Vector2(current.Bounds.Left, current.Location.Y - next.Bounds.Height + 5);

            actors = new DrawAbleActorCollection(game);
            Add(current);
            Add(next);

            _safeRemoveList = new List<IDrawableActor>();
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            Rectangle screenBounds = Game.Window.ClientBounds;
            Rectangle limits = new Rectangle(0, 0, screenBounds.Width, screenBounds.Height);

            // Aceleracao
            velocity = new Vector2(velocity.X, velocity.Y + _acceleration);
            if (velocity.Y > _maxSpeed) velocity = new Vector2(velocity.X, _maxSpeed);

            foreach (IDrawableActor actor in actors)
            {
                if (actor.Scrollable)
                {
                    adjustPosition(ref screenBounds, ref limits, actor);
                }

                actor.Location += velocity;
                actor.Update(gameTime);


                //Testa se ator saiu da tela e dispara evento OutOfBounds
                if (!actor.Outofscreen && !screenBounds.Intersects (actor.Bounds))
                {
                    actor.Outofscreen = true;
                    if (OutOfBounds != null)
                        OutOfBounds(this, new OutOfBoundsEventArgs(actor));
                }

            }

            // Teste de colisão entre os objetos colidiveis
            Dictionary<ICollidable,Boolean > actorsAlreadyTested = new Dictionary<ICollidable,Boolean >();

            foreach (ICollidable actor1 in ElementsToCollide )
            {
                foreach (ICollidable actor2 in ElementsToCollide) 
                {
                    if ((!actorsAlreadyTested.ContainsKey(actor2)) && (actor1 != actor2) && (actor1.Collided(actor2))) 
                    {
                        if (ColisionsOccours != null)
                            ColisionsOccours(this, new CollisionEventArgs(actor1, actor2));
                    }
                     
                }
                actorsAlreadyTested[actor1]=true;
            }

            base.Update(gameTime);

            //remover com segurança os atores 
            SafeRemove();
        }

        public override void Draw(GameTime gameTime)
        {
            spritebatch.Begin();
            actors.Draw(gameTime);
            spritebatch.End();
        }

        public void Add(IDrawableActor actor)
        {
            actors.Add(actor);
        }

        public void Remove(IDrawableActor actor)
        {
            _safeRemoveList.Add(actor);
            //actors.Remove(actor);
        }

        private void SafeRemove()
        {
            foreach (IDrawableActor actor in _safeRemoveList) actors.Remove(actor);
            _safeRemoveList.Clear();
        }

        private bool goingAway(Rectangle bounds, IActor actor, Vector2 delta)
        {
            return
                (actor.Location.X >= bounds.Width) && (delta.X > 0) ||
                (actor.Location.Y >= bounds.Height) && (delta.Y > 0) ||
                (actor.Location.X <= 0) && (delta.X < 0) ||
                (actor.Location.Y <= 0) && (delta.Y < 0);
        }

        private void adjustPosition(ref Rectangle screenBounds, ref Rectangle limits, IDrawableActor actor)
        {
            Rectangle actorRect = new Rectangle((int)actor.Location.X, (int)actor.Location.Y, actor.Bounds.Width, actor.Bounds.Height);
            Vector2 delta = actor.Velocity + velocity;

            if (actorRect.Intersects(limits))
            {
                actor.Visible = true;
            }
            else if (goingAway(screenBounds, actor, delta))
            {
                Vector2 newPosition = actor.Location;
                actor.Visible = false;

                if (delta.X > 0)
                {
                    newPosition.X = -actor.Bounds.Width;
                }
                else if (delta.X < 0)
                {
                    newPosition.X = limits.Width;
                }
                if (delta.Y > 0)
                {
                    newPosition.Y = -actor.Bounds.Height;
                }
                else if (delta.Y < 0)
                {
                    newPosition.Y = limits.Height;
                }
                actor.Location = newPosition;
                if (actor is ICollidable) ((ICollidable)actor).Collidable = true;
            }
            else
            {
                actor.Visible = true;
            }
        }

        #region Map Properties
        public Vector2 Velocity
        {
            get
            {
                return velocity;
            }
            set
            {
                velocity = value;
            }
        }
        public int MaxSpeed
        {
            get
            {
                return _maxSpeed;
            }
            set
            {
                _maxSpeed = value;
            }
        }

        #endregion

        #region Map Fields
        private DrawAbleActorCollection actors;
        private Vector2 velocity;
        private IDrawableActor current;
        private IDrawableActor next;
        private SpriteBatch spritebatch;
        #endregion

        #region Collision
        public event EventHandler<CollisionEventArgs> ColisionsOccours;


        /// <summary>
        /// Os atores colidíveis. Esta propriedade é calculada a partir da 
        /// propriedade <see cref="actors"/>, então é somente para leitura.
        /// </summary>
        protected IEnumerable<ICollidable> ElementsToCollide
        {
            get
            {
                return from e in actors
                       where e is ICollidable
                       select e as ICollidable;
            }
        }
        #endregion

        #region Out of Bounds Actor

        public event EventHandler<OutOfBoundsEventArgs> OutOfBounds;

        #endregion

    }
    


    /// <summary>
    /// Classe para eventos colisão entre atores
    /// </summary>
    public class CollisionEventArgs : EventArgs
    {
        ICollidable colliderA;
        public ICollidable ColliderA
        {
            get { return colliderA; }
            private set { colliderA = value; }
        }

        ICollidable colliderB;
        public ICollidable ColliderB
        {
            get { return colliderB; }
            private set { colliderB = value; }
        }

        public CollisionEventArgs(ICollidable a, ICollidable b)
        {
            ColliderA = a;
            ColliderB = b;
        }
    }

    /// <summary>
    /// Class para eventos de saida da tela
    /// </summary>
    public class OutOfBoundsEventArgs : EventArgs
    {
        IDrawableActor outactor;

        public IDrawableActor OutActor {  get { return outactor; } private set { outactor = value; }  }

        public OutOfBoundsEventArgs(IDrawableActor outator) { this.outactor = outator; }

    }


}
