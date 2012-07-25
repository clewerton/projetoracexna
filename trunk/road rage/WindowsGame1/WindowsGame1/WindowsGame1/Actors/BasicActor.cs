using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Graphics;
using TangoGames.RoadFighter.Scenes;

namespace TangoGames.RoadFighter.Actors
{

    /// <summary>
    /// A basic entity implements simple espatial properties. Movement not taken account.
    /// </summary>
    public abstract class BasicDrawingActor : GameComponent, IDrawableActor
    {

        public BasicDrawingActor(Game game, Rectangle bounds, Texture2D texture)
            : base(game)
        {
            this.bounds = bounds;
            this.texture = texture;
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
            Location += Velocity;

            base.Update(gameTime);
        }

        public abstract void Draw(GameTime gameTime);

        public Boolean collided(BasicDrawingActor AtorColidente)
        {
            return false;
        }

        #region BasicActor properties
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
        public Boolean Scrollable
        {
            get
            {
                return scrollable;
            }
            set
            {
                scrollable = value;
            }

        }
        public SpriteBatch SpriteBatch { get; set; }
        #endregion

        #region BasicActor Fields
        private Rectangle bounds;
        private Vector2 location;
        private Vector2 orientation;
        private Vector2 velocity;
        private Boolean visible;
        private Boolean scrollable;
        private Texture2D texture;
        #endregion
    }
}
