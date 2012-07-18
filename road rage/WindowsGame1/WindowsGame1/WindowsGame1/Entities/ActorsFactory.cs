using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;


namespace TangoGames.RoadFighter.Actors {
    public interface IEntityFactory<TId>
    {
        IActors this[TId id] { get; set; }
    }

    /// <summary>
    /// Factory that provides entities. 
    /// </summary>
    public class EntityFactory<TId> : Dictionary<TId, IActors>, IEntityFactory<TId>
    {
        public new IActors this[TId id]
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
