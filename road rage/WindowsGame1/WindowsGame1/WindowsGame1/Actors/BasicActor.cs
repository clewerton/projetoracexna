using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Graphics;


namespace TangoGames.RoadFighter.Actors
{

    /// <summary>
    /// A basic entity implements simple espatial properties. Movement not taken account.
    /// </summary>
    public abstract class BasicDrawingActor : GameComponent, IDrawableActor, IActorsCollision
    {
        private Rectangle bounds;
        private Vector2 location;
        private Vector2 orientation;
        private Vector2 velocity;
        private Boolean visible;
        private Texture2D texture;
        protected SpriteBatch spriteBatch;

        public BasicDrawingActor(Game game, Rectangle bounds, Texture2D texture, SpriteBatch spriteBatch)
            : base(game)
        {
            this.bounds = bounds;
            this.texture = texture;
            this.spriteBatch = spriteBatch;
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
            // TODO: Add your update code here

            base.Update(gameTime);
        }

        public abstract void Draw(GameTime gameTime);

        public Rectangle Bounds 
        {
            get {
                return bounds;
            }
            set {
                bounds = value;
            }
        }

        public Vector2 Location
        {
            get {
                return location;
            }
            set {
                location = value;
            }
        }

        public Vector2 Orientation
        {
            get {
                return orientation;
            }
            set {
                orientation = value;
            }
        }

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

        public Boolean colidiu(BasicDrawingActor AtorColidente)
        {
            return false;
        }

        public Texture2D Texture
        {
            get
            {
                return texture;
            }
            set
            {
                texture = value;
            }
        }

        public Boolean Visible
        {
            get
            {
                return visible;
            }
            set
            {
                visible = value;
            }
        }

    }
}
