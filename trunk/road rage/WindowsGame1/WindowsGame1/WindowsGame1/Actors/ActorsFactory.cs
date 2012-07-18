using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;


namespace TangoGames.RoadFighter.Actors {
    public interface IEntityFactory<TId>
    {
        IActor this[TId id] { get; set; }
    }

    /// <summary>
    /// Factory that provides entities. 
    /// </summary>
    public class EntityFactory<TId> : Dictionary<TId, IActor>, IEntityFactory<TId>
    {
        public new IActor this[TId id]
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
