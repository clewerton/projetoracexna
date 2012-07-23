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
    public interface IMap
    {
        void Add(IDrawableActor actor);
        void Remove(IDrawableActor actor);
        void Update(GameTime gameTime);
        void Draw(GameTime gameTime);
        Vector2 Velocity {get; set; }
    }

    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Map : DrawableGameComponent, IMap
    {
        public Map(Game game)
            : base(game)
        {
            actors = new DrawAbleActorCollection(game);
            visibleActors = new DrawAbleActorCollection(game);

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
            Rectangle limits = new Rectangle(0, 0, screenBounds.Width, screenBounds.Height);

            foreach (IDrawableActor actor in actors)
            {
                Rectangle actorRect = new Rectangle((int)actor.Location.X, (int)actor.Location.Y, actor.Bounds.Width, actor.Bounds.Height);
                if (actorRect.Intersects(limits))
                {
                    actor.Visible = true;
                }
                else if (goingAway(screenBounds, actor))
                {
                    actor.Visible = false;
                    Vector2 oldPosition = actor.Location;
                    actor.Location = new Vector2(oldPosition.X - Math.Sign(actor.Velocity.X) * limits.Width, oldPosition.Y - Math.Sign(actor.Velocity.Y) * limits.Height);
                }
                else
                {
                    actor.Visible = true;
                }
                Configure(actor);
            }
            actors.Move(velocity);
            // update ALL actors in map (no need to call visibleActor.Update)
            actors.Update(gameTime);
            
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            visibleActors.Draw(gameTime);
        }

        public void Add(IDrawableActor actor)
        {
            actors.Add(actor);
            Configure(actor);
        }

        public void Remove(IDrawableActor actor)
        {
            actors.Remove(actor);
            visibleActors.Remove(actor);
        }

        private void Configure(IDrawableActor actor)
        {
            if (actor.Visible && !visibleActors.Contains(actor))
            {
                visibleActors.Add(actor);
            }
        }

        private bool goingAway(Rectangle bounds, IActor actor)
        {
            return
                (actor.Location.X > bounds.Right) && (actor.Velocity.X > velocity.X) ||
                (actor.Location.Y > bounds.Bottom) && (actor.Velocity.Y > velocity.Y) ||
                (actor.Location.X < bounds.Left) && (actor.Velocity.X < velocity.X) ||
                (actor.Location.Y < bounds.Top) && (actor.Velocity.Y < velocity.Y);
        }

        #region Map Properties
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
        #endregion

        #region Map Fields
        private DrawAbleActorCollection actors;
        private DrawAbleActorCollection visibleActors;
        private Vector2 velocity;
        #endregion
    }
}
