using System;
using System.Collections.Generic;

namespace TangoGames.RoadFighter.Scenes
{
    public interface IScene
    {
        void Enter();
        void Leave();
    }

    public interface ISceneManagerService<TId>
    {
        IScene this[TId id] { get; set; }
        IScene Current { get; }

        bool Contains(TId id);
        void GoTo(TId id);
        void Stop();
    }

    /// <summary>
    /// O gerenciador de cenas.
    /// </summary>
    public class SceneManager<TId> : Dictionary<TId, IScene>, ISceneManagerService<TId>
    {
        public SceneManager() {}

        #region ISceneManagerService Operations
        public new IScene this[TId id] 
        { 
            get { return base[id]; }
            set
            {
                if (id == null) // null is not a valid argument!
                {
                    throw new ArgumentNullException();
                }

                if (! Contains(id)) // new scene, just add it
                {
                    base[id] = value;
                    return;
                }

                // finalize the old one before swappage
                var old = base[id];

                if (old.Equals(Current)) // if old is running, leave it before swapping
                {
                    old.Leave();
                    Current = null;
                }

                // the changing of the guard
                base[id] = value;
            }
        }

        public bool Contains(TId id)
        {
            return this.ContainsKey(id);
        }

        public void GoTo(TId id)
        {
            // throw up on null ids, since some types are not nullable
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            // throw up on unknown ids
            if (! Contains(id))
            {
                throw new ArgumentException("Scene not found: " + id, "id");
            }

            // set the new current
            Current = this[id];
        }

        public void Stop()
        {
            Current = null;
        }

        public IScene Current
        {
            get { return _current; }
            private set
            {
                if(_current != null) { _current.Leave(); }

                _current = value;

                if (_current != null) { _current.Enter(); }
            }
        }
        #endregion
        
        #region Properties & Fields
        private IScene _current;
        #endregion   
    }
}
