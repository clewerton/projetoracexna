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
public interface IActor
    {
        Rectangle Bounds { get; set; }
        Vector2 Location { get; set; }
        Vector2 Orientation { get; set; }
        void Update(GameTime gameTime);
        bool Enabled{get; set;}
        bool Visible{get; set;}
    }

    // Encapsulates entity group functionality.
    public interface IActorGroup: IEnumerable<IActor>
    {
        void Add(IActor entity);
        void Remove(IActor entity);
        void Enable();
        void Disable();
    }

    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class ActorCollection : GameComponent, IActorGroup
    {
        private IList<IActor> actors = new List<IActor>();

        public ActorCollection(Game game)
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
            // TODO: Add your update code here

            base.Update(gameTime);
        }

        public IEnumerator<IActor> GetEnumerator()
        {
            return actors.GetEnumerator();
        }

        public void Add(IActor entity)
        {
            if (entity == null) // null is not a valid argument!
            {
                throw new ArgumentNullException();
            }
            actors.Add(entity);
        }

        public void Remove(IActor entity)
        {
            actors.Remove(entity);
        }

        public void Enable()
        {
            foreach (IActor item in actors)
            {
                item.Enable();
            }
        }

        public void Disable()
        {
            foreach (IActor item in actors)
            {
                item.Disable();
            }
        }

    }
}
