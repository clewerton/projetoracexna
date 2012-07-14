using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;


namespace TangoGames.RoadFighter.Entities
{
    public interface IEntityFactory<TId>
    {
        IEntity this[TId id] { get; set; }
    }

    /// <summary>
    /// Factory that provides entities. 
    /// </summary>
    public class EntityFactory<TId> : Dictionary<TId, IEntity>, IEntityFactory<TId>
    {
        public new IEntity this[TId id]
        {
            get { return base[id]; }
            set
            {
                if (id == null)
                {
                    throw new ArgumentNullException();
                }
                base[id] = value;
            }
        }
    }
}
