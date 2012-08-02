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
using TangoGames.RoadFighter.Scenes;
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
        event EventHandler<ChangeRoadEventArgs> ChangeRoadType;
        IDrawableActor Road { get; }
        void ChangeLaneRegister(IChangeLanelistener listener);
        void ChangeLaneUnRegister(IChangeLanelistener listener);

    }

    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Map : DrawableGameComponent, IMap
    {

        public Map(Scene scene)
            : base(scene.Game)
        {
            this.scene=scene;

            //gera background
            FifoBackground = new MyFifo();
            //gerar tres backgounds que ficam rolando na fila
            FifoBackground.Enqueue(new BackGround(scene.Game, scene.currentSpriteBatch));
            FifoBackground.Enqueue(new BackGround(scene.Game, scene.currentSpriteBatch));
            FifoBackground.Enqueue(new BackGround(scene.Game, scene.currentSpriteBatch));

            //gera instancia de gestor de estradas
            roads = new RoadManager(scene);
            FifoRoad = new MyFifo();
            //gera a fila de estradas pelo gerenciado de extradas 
            FifoRoad.Enqueue(roads.CurrentRoad);
            FifoRoad.Enqueue(roads.NextRoad());
            FifoRoad.Enqueue(roads.NextRoad());

            actors = new DrawAbleActorCollection(scene.Game);

            _safeRemoveList = new List<IDrawableActor>();

            listenersChangeLane = new List<IChangeLanelistener>() ;

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
            // Aceleracao
            velocity = new Vector2(velocity.X, velocity.Y + _acceleration);
            if (velocity.Y > _maxSpeed) velocity = new Vector2(velocity.X, _maxSpeed);

            //Atualiza rolagem das estradas e background
            foreach (IDrawableActor bkg in FifoBackground)
            {
                bkg.Location += velocity;
                bkg.Update(gameTime);
            }
            foreach (IDrawableActor road in FifoRoad)
            {
                road.Location += velocity;
                road.Update(gameTime);
                foreach (IChangeLanelistener cll in listenersChangeLane)
                {
                    if (road.Bounds.Bottom >= cll.Bounds.Y && road.Bounds.Top <= cll.Bounds.Y && cll.CurrentLanes.Count  != ((IRoad)road).Lanes.Count )
                            cll.NewLanes = ((IRoad)road).Lanes;
                }
            }

            adjustPosition(FifoBackground, true );

            if (adjustPosition(FifoRoad , false))
            {
                //raise event roadchange
                if (ChangeRoadType != null)
                    ChangeRoadType(this, new ChangeRoadEventArgs(((IRoad)FifoRoad.First()).Lanes, ((StraightRoad)FifoRoad.First()).RoadType));
            }

            foreach (IDrawableActor actor in actors)
            {

                actor.Location += velocity;
                actor.Update(gameTime);


                //Testa se ator saiu da tela e dispara evento OutOfBounds
                if (!actor.Outofscreen && ! scene.Game.Window.ClientBounds.Intersects (actor.Bounds))
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
            foreach (IDrawableActor bkg in FifoBackground) bkg.Draw(gameTime);
            foreach (IDrawableActor road in FifoRoad) road.Draw(gameTime);
            actors.Draw(gameTime);
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

        private bool adjustPosition(MyFifo fifo, bool enqueue)
        {
            if ( fifo.Peek().Bounds.Top > scene.Game.Window.ClientBounds.Bottom)
            {
                if (enqueue) 
                {
                    fifo.Enqueue(fifo.Dequeue());
                }
                else
                { 
                    fifo.Dequeue();
                    fifo.Enqueue(roads.NextRoad());
                }
                return true;
            }
            return false;
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
        public IDrawableActor Road
        {
            get
            {
                return FifoRoad.First();
            }
        }

        #endregion

        #region Map Fields
        private DrawAbleActorCollection actors;
        private Vector2 velocity;

        //filas de fundo e estrada
        private MyFifo FifoBackground;
        private MyFifo FifoRoad;

        //private SpriteBatch spritebatch;
        private float _acceleration = 0.05F;
        private int _maxSpeed = 15;
        private List<IDrawableActor> _safeRemoveList;

        //roads manager
        private IRoadManager roads;

        //Current Scene 
        private Scene scene;

        //
        private List<IChangeLanelistener> listenersChangeLane;

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

        public void ChangeLaneRegister(IChangeLanelistener listener)
        {
            listenersChangeLane.Add(listener);
        }

        public void ChangeLaneUnRegister(IChangeLanelistener listener)
        {
            listenersChangeLane.Remove(listener);
        }

        public event EventHandler<ChangeRoadEventArgs> ChangeRoadType;

        private class BackGround : BasicDrawingActor
        {
            public BackGround(Game game, SpriteBatch spriteBatch)
                : base(game, game.Content.Load<Texture2D>("Textures/grass1024"))
            {
                this.SpriteBatch = spriteBatch;
            }
            public override void Draw(GameTime gameTime)
            {
                SpriteBatch.Draw(Texture, new Rectangle((int)Location.X, (int)Location.Y, Bounds.Width, Bounds.Height), Color.White);
            }
        }

        private class MyFifo : Queue<IDrawableActor> 
        {
            public IDrawableActor Last { get; private set; }
            public IDrawableActor SecondLast { get; private set; }
            public IDrawableActor Fist { get { return base.Peek(); } }

            public new void Enqueue(IDrawableActor item) 
            {
                SecondLast = Last;
                Last = item;
                base.Enqueue(item);
                if (SecondLast != null) adjustNext();
            }
            private void adjustNext()
            {
                Last.Location = new Vector2(Last.Bounds.X, SecondLast.Location.Y - Last.Bounds.Height);
            }
        } 
 

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


    /// <summary>
    /// Class para eventos de troca de tipo da estrada
    /// </summary>
    public class ChangeRoadEventArgs : EventArgs
    {
        ILanes lanes;
        RoadTypes roadtype;

        public ILanes CurrentLanes { get { return lanes; } private set { lanes = value; } }
        public RoadTypes Roadtype { get { return roadtype; } private set { roadtype = value; } }

        public ChangeRoadEventArgs(ILanes lanes, RoadTypes roadtype ) 
        { 
            this.lanes = lanes;
            this.roadtype = roadtype;
        }

    }

}
