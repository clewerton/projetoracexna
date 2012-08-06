using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using TangoGames.RoadFighter.Levels;
using TangoGames.RoadFighter.Scenes;

namespace TangoGames.RoadFighter.Actors
{
    public class Enemy : BasicDrawingActor, ICollidable, IEnemy, IChangeLanelistener  
    {
        public enum EnemyTypes { Inimigo1, Inimigo2, Inimigo3, Inimigo4, Inimigo5, Inimigo6, Inimigo7, Inimigo8 }

        private Scene scene;
        private IMap map;
        private EnemyTypes etype;

        private float angulo = 0;

        private int targetLane = -1;

        private float _acceleration = 0;

        private float maxSpeed;

        private float targetSpeed=0;

        private bool LaneChanger;

        private int sortChange;

        private int countChange;

        private float minSpeed;

        private Random random;

        public Enemy(EnemyTypes etype, Scene scene, IMap map)
            : base(scene.Game, TextureEnemy(scene.Game, etype))
        {
            this.scene = scene;
            this.map = map;
            this.etype = etype;
            this.SpriteBatch = scene.currentSpriteBatch;
            Collidable = true;

            maxSpeed = (float)(map.MaxSpeed * 0.85);

            minSpeed = (float)(map.MaxSpeed * 0.15);

            int TheSeed = (int)DateTime.Now.Ticks;
            random = new Random(TheSeed);

            LaneChanger = false;

            InitBehavior();

            sortChange = random.Next(100, 300);

        }

        public override void Update(GameTime gameTime)
        {
            Velocity = UpdateSpeed();

            base.Update(gameTime);

            if (map.CheckPointReach) { targetSpeed = -minSpeed; }

            if (targetSpeed != 0)
            {
                if (Velocity.Y > targetSpeed) { _acceleration = 0.01F; }
                else { _acceleration = -0.01F; }
            }
            else { targetSpeed = Velocity.Y; }

            if (targetLane == -1) { targetLane = XtoLane(Location.X); }

            countChange++;

            if (LaneChanger)
            {
                if ( sortChange < countChange  )
                {
                    if (ChangeLane(random.Next(-1,2))) 
                    {
                        countChange = 0;

                        sortChange = random.Next(100, 300);

                    }
                }
            }

            movimenta();

        }

        private void InitBehavior()
        {
            switch (etype)
            {
                case EnemyTypes.Inimigo1:
                    LaneChanger = true;
                    break;
                case EnemyTypes.Inimigo2:
                    break;
                case EnemyTypes.Inimigo3:
                    break;
                case EnemyTypes.Inimigo4:
                    break;
                case EnemyTypes.Inimigo5:
                    LaneChanger = true;
                    break;
                case EnemyTypes.Inimigo6:
                    break;
                case EnemyTypes.Inimigo7:
                    targetSpeed = -(float)(map.MaxSpeed * 0.9);
                    break;              
                case EnemyTypes.Inimigo8:
                    targetSpeed = -(float)(map.MaxSpeed * 0.10); 
                    break;
                default:
                    break;
            }

        }

        /// <summary>
        /// Calcula a velocidade do inimigo
        /// </summary>
        /// <returns>Vetor de velocidade</returns>
        private Vector2 UpdateSpeed()
        {
            float speed = Velocity.Y;
            float speedinc = speed * _acceleration;
            if (-speedinc < _acceleration) { speedinc = -_acceleration; }

            speed += speedinc;

            if (speed < -maxSpeed )  { speed = - maxSpeed;}
            if (speed >= 0 ) 
            { 
                speed = 0.0F;
                targetSpeed = -minSpeed;
            }

            return new Vector2(Velocity.X, speed);
        }

        private float ratioSpeed()
        {
            return ( -Velocity.Y / map.MaxSpeedGlobal);
        }

        private void movimenta()
        {
            if (targetLane > _lanes.LastIndex) { targetLane = _lanes.LastIndex; }
            if (targetLane < _lanes.StartIndex) { targetLane = _lanes.StartIndex; }


            int targetX = _lanes.LanesList[targetLane];

            int dif = Math.Abs(targetX - (int)Location.X);

            if (dif < (7 * ratioSpeed()))
            {
                Location = new Vector2((float)targetX, Location.Y);
                angulo = 0;
                return;
            }


            if (Location.X < targetX)
            {
                Move(new Vector2(7 * ratioSpeed(), 0));
                angulo = (float)((0.1F * ratioSpeed()) + 0.05F);
            }
            else if (Location.X > targetX)
            {
                Move(new Vector2(-7 * ratioSpeed(), 0));
                angulo = -(float)((0.1F * ratioSpeed()) + 0.05F);
            }

        }

        public static Texture2D TextureEnemy(Game game, EnemyTypes etype)
        {
            switch (etype)
            {
                case EnemyTypes.Inimigo1:
                    return game.Content.Load<Texture2D>("Textures/carroInimigo1");
                case EnemyTypes.Inimigo2:
                    return game.Content.Load<Texture2D>("Textures/carroInimigo2");
                case EnemyTypes.Inimigo3:
                    return game.Content.Load<Texture2D>("Textures/carroInimigo3");
                case EnemyTypes.Inimigo4:
                    return game.Content.Load<Texture2D>("Textures/carroInimigo4");
                case EnemyTypes.Inimigo5:
                    return game.Content.Load<Texture2D>("Textures/carroInimigo5");
                case EnemyTypes.Inimigo6:
                    return game.Content.Load<Texture2D>("Textures/carroInimigo6");
                case EnemyTypes.Inimigo7:
                    return game.Content.Load<Texture2D>("Textures/carroInimigo7");
                case EnemyTypes.Inimigo8:
                    return game.Content.Load<Texture2D>("Textures/carroInimigo8");
                default:
                    return game.Content.Load<Texture2D>("Textures/carSprite");
            }

        }

        public override void Draw(GameTime gameTime)
        {
            //SpriteBatch.Draw(Texture, new Rectangle((int)Location.X, (int)Location.Y, Bounds.Width, Bounds.Height), Color.White);
            SpriteBatch.Draw(Texture, new Rectangle((int)Location.X + Texture.Width / 2, (int)Location.Y + Texture.Height, Bounds.Width, Bounds.Height), null, Color.White, (float)angulo, new Vector2(Texture.Width / 2, Texture.Height), SpriteEffects.None, 0);

        }

        #region Controle de Pistas


        public bool ChangeLane(int swicth) 
        {
            int go = targetLane + swicth;
            if (go > _lanes.LastIndex || go < _lanes.StartIndex)
            { 
                return false;
            }
            targetLane = go;
            return true;
        }


        private ILanes _lanes;
        public ILanes CurrentLanes { get { return _lanes; } }
        public ILanes NewLanes
        {
            set
            {
                _lanes = value;
                targetLane = -1;
            }
        }

        private int XtoLane(float x)
        {
            int lane = 0;

            while ((x > _lanes.LanesList[lane]) && (lane < _lanes.LastIndex))
            {
                lane++;
            }

            return lane;
        }

        #endregion

        #region Collision implementation
        /// <summary>
        /// Teste de colisão por retangulo
        /// </summary>
        ICollider collider = new BoundingBox();

        public bool Collided(ICollidable that)
        {
            return collider.TestCollision(this, that);
        }

        public ICollider Collider { get { return this.collider; } set { this.collider = value; } }

        public bool Collidable { get; set; }

        #endregion

        #region Enemy implementation

        private bool _active = false;
        public Boolean Active { get { return _active; } set { _active = value; } }

        #endregion

    }

}
