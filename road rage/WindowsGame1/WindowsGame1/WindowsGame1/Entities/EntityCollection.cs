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


namespace TangoGames.RoadFighter.Entities
{
    public interface IEntity
    {
        Vector2 GetBounds { get; set; }
        Rectangle Location { get; set; }
        Vector2 Orientation { get; set; }
        void Update(GameTime gameTime);
        void Enable();
        void Disable();
    }

    // Encapsulates entity group functionality.
    public interface IEntityGroup
    {
        void Add(IEntity entity);
        void Remove(IEntity entity);
        void Enable();
        void Disable();
    }

    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class EntityCollection : GameComponent
    {
        private IList<IEntity> entities = new List<IEntity>();

        public EntityCollection(Game game)
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

        public void Add(IEntity entity)
        {
            if (entity == null) // null is not a valid argument!
            {
                throw new ArgumentNullException();
            }
            entities.Add(entity);
        }

        public void Remove(IEntity entity)
        {
            entities.Remove(entity);
        }

        public void Enable()
        {
            foreach (IEntity item in entities)
            {
                item.Enable();
            }
        }

        public void Disable()
        {
            foreach (IEntity item in entities)
            {
                item.Disable();
            }
        }

    }
}
