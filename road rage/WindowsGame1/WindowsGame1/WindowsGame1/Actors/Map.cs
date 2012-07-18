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
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Map : Microsoft.Xna.Framework.GameComponent
    {
        private IActorGroup actors;
        private IActorGroup visibleActors;
        private IActorGroup activeActors;
        private IActor stage;

        public Map(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
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
            foreach (IActor actor in actors)
            {
                if(actor.Bounds.Intersects(screenBounds))
                {

                };
            }

            base.Update(gameTime);
        }

        public void Add(IActor actor)
        {
            actors.Add(actor);
            if(actor.Enable
            visibleActors.Add(actor);
            activeActors.Add(actor);
        }

        public void Remove(IActor actor)
        {
            actors.Add(actor);
            visibleActors.Add(actor);
            activeActors.Remove(actor);
        }

    }
}
