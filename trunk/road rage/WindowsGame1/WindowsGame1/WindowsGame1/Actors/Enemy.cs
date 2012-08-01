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
    public class Enemy : BasicDrawingActor, ICollidable, IEnemy 
    {
        public enum EnemyTypes { Inimigo1, Inimigo2, Inimigo3, Inimigo4, Inimigo5, Inimigo6, Inimigo7, Inimigo8 }

        private Scene scene;
        private EnemyTypes etype;



        public Enemy(EnemyTypes etype, Scene scene)
            : base(scene.Game, TextureEnemy(scene.Game, etype))
        {
            this.scene = scene;
            this.etype = etype;
            this.SpriteBatch = scene.currentSpriteBatch;
            Collidable = true;

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
            SpriteBatch.Draw(Texture, new Rectangle((int)Location.X, (int)Location.Y, Bounds.Width, Bounds.Height), Color.White);
        }

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
