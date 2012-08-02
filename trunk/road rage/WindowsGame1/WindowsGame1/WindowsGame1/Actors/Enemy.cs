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
        private EnemyTypes etype;

        private float angulo = 0;
        private int faixaAnterior = -1;
        private int faixaAtual = -1;


        public Enemy(EnemyTypes etype, Scene scene)
            : base(scene.Game, TextureEnemy(scene.Game, etype))
        {
            this.scene = scene;
            this.etype = etype;
            this.SpriteBatch = scene.currentSpriteBatch;
            Collidable = true;
            _lanes = new FourLanes();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (faixaAtual == -1)
            {
                faixaAtual = XtoLane(Location.X);
                faixaAnterior = faixaAtual;
            }

            movimenta();

        }


        private void movimenta()
        {
            if ((faixaAnterior <= faixaAtual) && (Location.X < _lanes.LanesList[faixaAtual]))
            {
                Move(new Vector2(3, 0));
                angulo = (float)0.1;

                if (Location.X >= _lanes.LanesList[faixaAtual])
                {
                    angulo = 0;
                    Location = new Vector2(_lanes.LanesList[faixaAtual], Location.Y);
                }
            }
            if ((faixaAnterior >= faixaAtual) && (Location.X > _lanes.LanesList[faixaAtual]))
            {
                Move(new Vector2(-3, 0));
                angulo = (float)-0.1;

                if (Location.X <= _lanes.LanesList[faixaAtual])
                {
                    angulo = 0;
                    Location = new Vector2(_lanes.LanesList[faixaAtual], Location.Y);
                }
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

        private ILanes _lanes;
        public ILanes CurrentLanes { get { return _lanes; } }
        public ILanes NewLanes
        {
            set
            {
                _lanes = value;
                faixaAtual = -1;
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
