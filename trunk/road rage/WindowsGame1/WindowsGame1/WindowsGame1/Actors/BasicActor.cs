using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Graphics;


namespace TangoGames.RoadFighter.Actors
{

    /// <summary>
    /// A basic entity implements simple espatial properties. Movement not taken account.
    /// </summary>
    public class BasicDrawingActor : GameComponent, IActor, IActorsCollision
    {
        private Rectangle bounds;
        private Vector2 location;
        private Vector2 orientation;
        private Vector2 velocity;
        private Texture2D texture;

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
            // TODO: Add your update code here

            base.Update(gameTime);
        }

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
        
        public Texture2D GetTextura()
        {
            return texture;
        }

    }
}