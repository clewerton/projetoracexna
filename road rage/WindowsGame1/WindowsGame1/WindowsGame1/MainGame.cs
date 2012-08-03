using System;
using Microsoft.Xna.Framework;
using TangoGames.RoadFighter.Levels;
using TangoGames.RoadFighter.Scenes;
using TangoGames.RoadFighter.Actors;
using TangoGames.RoadFighter.Input;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using TangoGames.RoadFighter.Shared;

namespace TangoGames.RoadFighter
{
    /// <summary>
    /// A classe que representa o jogo.
    /// </summary>
    public class MainGame : Game
    {
        /// <summary>
        /// Serão usados pelo gerenciador de cenas para identificar as cenas.
        /// </summary>
        public enum Scenes { Intro, End, Fase, Menu, Credits }
        public enum ActorTypes { Car, Truck, Hero }

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public MainGame()
        {
            // XXX precisa ficar no construtor, pois base.Initialize precisa de um serviço para 
            // XXX iniciar o GraphicDevice.
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            if (!_graphics.IsFullScreen)
            {
                //_graphics.ToggleFullScreen();
            }
            _graphics.ApplyChanges();
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        /// <summary>
        /// Registra todos os GameComponents os inicializa junto com o gerenciador de cenas. 
        /// </summary>
        protected override void Initialize()
        {
            StartInputService(); // detecção de input
            StartSceneManager(); // o gerenciamento de cenas
            StartActorFactory(); // fábrica de atores
            StartSharedState();  // o estado global das cenas

            // inicializa todos os componentes registrados no jogo
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
            Window.Title = _graphics.PreferredBackBufferWidth.ToString();

            var sceneManager = (ISceneManagerService<MainGame.Scenes>) Services.GetService(typeof(ISceneManagerService<MainGame.Scenes>));
            var input = (IInputService) Services.GetService(typeof (IInputService));

            if (input.KeyPressOnce(Keys.Space)) // espaço pausa a cena corrente
            {
                var current = sceneManager.Current;
                if (current.Paused)
                {
                    current.Resume();
                }
                else
                {
                    current.Pause();
                }
            }

            base.Update(gameTime);
        }

        // prepara o gerenciamento de cenas do jogo
        private void StartSceneManager()
        {
            // cria o gerenciador de telas
            var sceneManager = new SceneManager<Scenes>();

            // registra-o como provedor do serviço ISceneManagerService<Scenes>
            Services.AddService(typeof (ISceneManagerService<Scenes>), sceneManager);

            // registra as cenas conhecidas
            sceneManager[Scenes.Intro] = new Intro(this);
            sceneManager[Scenes.End] = new End(this);
            sceneManager[Scenes.Fase] = new Fase1(this);
            sceneManager[Scenes.Menu] = new Menu(this);
            sceneManager[Scenes.Credits] = new Credits(this);

            // a Intro começa como a cena atual
            sceneManager.GoTo(Scenes.Intro);
        }

        private void StartActorFactory()
        {
            //var actorFactory = new ActorFactory<ActorTypes, IDrawableActor>(); 
            //actorFactory[ActorTypes.Car] = new Car(this, new Vector2(100, 100), _spriteBatch);
            //actorFactory[ActorTypes.Truck] = new Truck(this, new Vector2(85, 135), _spriteBatch);
            //actorFactory[ActorTypes.Hero] = new Heroi(this, new Vector2(72, 155), _spriteBatch);

            //// se registra como serviço
            //Services.AddService(typeof(IActorFactory<ActorTypes, IDrawableActor>), actorFactory);
        }

        private void StartInputService()
        {
            var inputservice = new InputService(this);

            // se registra como serviço
            Services.AddService(typeof(IInputService), inputservice);

            // se registra como GameComponent
            Components.Add(inputservice);
        }

        private void StartSharedState()
        {
            var sharedState = new SharedState();

            // se registra como serviço
            Services.AddService(typeof(ISharedState), sharedState);
        }
    }
}
