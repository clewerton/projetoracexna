using Microsoft.Xna.Framework;


namespace TangoGames.RoadFighter.Actors
{

    /// <summary>
    /// A basic entity implements simple espatial properties. Movement not taken account.
    /// </summary>
    public class BasicActor : GameComponent, IActors
    {
        private Rectangle bounds;
        private Vector2 location;
        private Vector2 orientation;

        public BasicActor(Game game, Rectangle bounds)
            : base(game)
        {
            game.Components.Add(this);
            this.bounds = bounds;
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

        public Rectangle GetBounds 
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

        public void Enable()
        {
            Enabled = true;
        }

        public void Disable()
        {
            Enabled = false;
        }


    }
}
