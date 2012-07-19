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
    public class Map : Microsoft.Xna.Framework.DrawableGameComponent
    {
        private IDrawableActorGroup actors;
        private IDrawableActorGroup visibleActors;
        private IDrawableActorGroup activeActors;

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
            foreach (IDrawableActor actor in actors)
            {
                if (actor.Bounds.Intersects(screenBounds))
                {
                    actor.Visible = true;
                }
                else if (goingAway(screenBounds, actor))
                {
                    actor.Visible = false;
                }
                else
                {
                    actor.Visible = true;
                }
                Configure(actor);
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (IDrawableActor actor in visibleActors)
            {
                actor.Draw(gameTime);
            }

        }

        public void Add(IDrawableActor actor)
        {
            actors.Add(actor);
            Configure(actor);
        }

        public void Remove(IDrawableActor actor)
        {
            actors.Add(actor);
            visibleActors.Add(actor);
            activeActors.Remove(actor);
        }

        private void Configure(IDrawableActor actor)
        {
            if (actor.Enabled && !visibleActors.Contains(actor))
            {
                activeActors.Add(actor);
            }
            if (actor.Visible && !activeActors.Contains(actor))
            {
                visibleActors.Add(actor);
            }
        }

        private bool goingAway(Rectangle bounds, IActor actor)
        {
            return
                (actor.Location.X > bounds.Right) && (actor.Orientation.X > 0) ||
                (actor.Location.Y > bounds.Bottom) && (actor.Orientation.Y > 0) ||
                (actor.Location.X < bounds.Left) && (actor.Orientation.X < 0) ||
                (actor.Location.Y < bounds.Top) && (actor.Orientation.Y < 0);
        }

    }
}
