using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TangoGames.RoadFighter.Utils;

namespace TangoGames.RoadFighter.Components
{
    public interface IScene
    {
        void Enter();
        void Leave();
    }
    
    public interface ISceneManager<TId>
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
    public class SceneManager<TId> : DrawableGameComponent, ISceneManager<TId>
    {
        public SceneManager(Game game) : base(game)
        {
            Scenes = new Dictionary<TId, IScene>();
            
            // se registra como provedor do serviço ISceneManager<TId>
            game.Services.AddService(typeof(ISceneManager<TId>), this);

            // se registra como componente do game, fazendo parte do ciclo de vida
            game.Components.Add(this);
        }

        #region ISceneManager
        public IScene this[TId id] 
        { 
            get { return Scenes[id]; }
            set
            {
                if (id == null) // null is not a valid argument!
                {
                    throw new ArgumentNullException();
                }

                if (! Contains(id)) // new scene, just add it
                {
                    Scenes[id] = value;
                    return;
                }

                // finalize the old one before swappage
                var old = Scenes[id];

                if (old.Equals(Current)) // if old is running, leave it before swapping
                {
                    old.Leave();
                    Current = null;
                }

                // the changing of the guard
                Scenes[id] = value;
            }
        }

        public bool Contains(TId id)
        {
            return Scenes.ContainsKey(id);
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

        #region Game Component Operations
        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // cria o sprite batch a ser usado pelas cenas
            SpriteBatch = new SpriteBatch(Game.GraphicsDevice);
        }

        /// <summary>
        /// Atualiza a cena ativa, caso ela seja atualizável. Uma cena é atualizável 
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            if(Current is ISimpleUpdateable)
            {
                (Current as ISimpleUpdateable).Update(gameTime);
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// Allows the game component to draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Draw(GameTime gameTime)
        {
            if (Current is ISimpleDrawable)
            {
                SpriteBatch.Begin();

                (Current as ISimpleDrawable).Draw(gameTime, SpriteBatch);
                
                SpriteBatch.End();
            }

            base.Draw(gameTime);
        }
        #endregion
        
        #region Properties & Fields
        private IDictionary<TId, IScene> _scenes;
        private IScene _current;
        private SpriteBatch _spriteBatch;

        public IDictionary<TId, IScene> Scenes
        {
            get { return _scenes; }
            private set { _scenes = value; }
        }

        public SpriteBatch SpriteBatch
        {
            get { return _spriteBatch; }
            private set { _spriteBatch = value; }
        }
        #endregion   
    }
}
