using System;
using System.Collections;
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
        Vector2 Velocity { get; set; }
        void Update(GameTime gameTime);
        bool Enabled { get; set; }
        void Move(Vector2 delta);
    }

    // Encapsulates entity group functionality.
    public interface IActorGroup<ActorType> : IEnumerable<ActorType> where ActorType : IActor
    {
        void Add(ActorType entity);
        void Remove(ActorType entity);
        void Update(GameTime gameTime);
        bool Enabled { set; }
    }

    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class ActorCollection<ActorType> : GameComponent, IActorGroup<ActorType> where ActorType : IActor
    {
        protected IList<ActorType> actors = new List<ActorType>();

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
            foreach (IDrawableActor actor in actors)
            {
                actor.Update(gameTime);
            }

            base.Update(gameTime);
        }

        public IEnumerator<ActorType> GetEnumerator()
        {
            return actors.GetEnumerator();
        }

        public void Add(ActorType entity)
        {
            if (entity == null) // null is not a valid argument!
            {
                throw new ArgumentNullException();
            }
            actors.Add(entity);
        }

        public void Remove(ActorType entity)
        {
            actors.Remove(entity);
        }

        public new bool Enabled
        {
            set
            {
                foreach (ActorType item in actors)
                {
                    item.Enabled = value;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

}
