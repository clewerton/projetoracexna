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


namespace TangoGames.RoadFighter.Actors
{
    public interface IMap
    {
        void Add(IDrawableActor actor);
        void Remove(IDrawableActor actor);
        void Update(GameTime gameTime);
        void Draw(GameTime gameTime);
        Vector2 Velocity {get; set; }
        event EventHandler<CollisionEventArgs > ColisionsOccours;
    }

    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Map : DrawableGameComponent, IMap
    {
        public Map(Game game)
            : base(game)
        {
            actors = new DrawAbleActorCollection(game);
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

            foreach (IDrawableActor actor in actors)
            {
                if (actor.Scrollable)
                {
                    adjustPosition(ref screenBounds, ref limits, actor);
                }
                actor.Location += velocity;
                actor.Update(gameTime);
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
                        {
                            ColisionsOccours(this, new CollisionEventArgs(actor1, actor2));
                        }
                    }
                     
                }
                actorsAlreadyTested[actor1]=true;
            }

            base.Update(gameTime);
        }


        public override void Draw(GameTime gameTime)
        {
            actors.Draw(gameTime);
        }

        public void Add(IDrawableActor actor)
        {
            actors.Add(actor);
        }

        public void Remove(IDrawableActor actor)
        {
            actors.Remove(actor);
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
        #endregion

        #region Map Fields
        private DrawAbleActorCollection actors;
        private Vector2 velocity;
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

    }

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
}
