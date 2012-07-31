using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using TangoGames.RoadFighter.Levels;

namespace TangoGames.RoadFighter.Actors
{
    class Truck : BasicDrawingActor, ICollidable , IEnemy 
    {
        public Truck(Game game, Vector2 dimensions, SpriteBatch spriteBatch)
            : base(game, dimensions, game.Content.Load<Texture2D>("Textures/truckSprite"))
        {
            Collidable = true;
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
