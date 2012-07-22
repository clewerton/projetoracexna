using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;


namespace TangoGames.RoadFighter.Actors {
    public interface IActorFactory<TId, ActorType>
    {
        ActorType this[TId id] { get; set; }
    }

    /// <summary>
    /// Factory that provides entities. 
    /// </summary>
    public class ActorFactory<TId, ActorType> : Dictionary<TId, ActorType>, IActorFactory<TId, ActorType>
    {
        public new ActorType this[TId id]
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
