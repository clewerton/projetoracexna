using System;
using Microsoft.Xna.Framework;
using TangoGames.RoadFighter.Levels;
using TangoGames.RoadFighter.Scenes;
using TangoGames.RoadFighter.Actors;
using TangoGames.RoadFighter.Input;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace TangoGames.RoadFighter
{
    /// <summary>
    /// A classe que representa o jogo.
    /// </summary>
    public class MainGame : Game
    {
        /// <summary>
        /// Ser�o usados pelo gerenciador de cenas para identificar as cenas.
        /// </summary>
        public enum Scenes { Intro, End, Fase, Menu }
        public enum ActorTypes { Car, Truck }

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        public MainGame()
        {
            // XXX precisa ficar no construtor, pois base.Initialize precisa de um servi�o para 
            // XXX iniciar o GraphicDevice.
            graphics = new GraphicsDeviceManager(this); 

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        /// <summary>
        /// Registra todos os GameComponents os inicializa junto com o gerenciador de cenas. 
        /// </summary>
        protected override void Initialize()
        {
            // configura o gerenciamento de cenas
            StartSceneManager();
            StartEntityFactory();

            //Cria Servi�o de input
            StartInputService();

            new ProbeComponent(this);

            // inicializa todos os componentes registrados no jogo
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
            var sceneManager = (ISceneManagerService<MainGame.Scenes>) Services.GetService(typeof(ISceneManagerService<MainGame.Scenes>));

            if (Keyboard.GetState().IsKeyDown(Keys.Space))
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

            // registra-o como provedor do servi�o ISceneManagerService<Scenes>
            Services.AddService(typeof (ISceneManagerService<Scenes>), sceneManager);

            // registra as cenas conhecidas
            sceneManager[Scenes.Intro] = new Intro(this);
            sceneManager[Scenes.End] = new End(this);
            sceneManager[Scenes.Fase] = new Fase1(this);
            sceneManager[Scenes.Menu] = new Menu(this);

            // a Intro come�a como a cena atual
            sceneManager.GoTo(Scenes.Intro);
        }

        private void StartEntityFactory()
        {
            ActorFactory<ActorTypes, IDrawableActor> _entityFactory = new ActorFactory<ActorTypes, IDrawableActor>(); 
            _entityFactory[ActorTypes.Car] = new Car(this, new Rectangle(0, 0, 100, 100), spriteBatch);
            _entityFactory[ActorTypes.Truck] = new Truck(this, new Rectangle(0, 0, 85, 135), spriteBatch);
            Services.AddService(typeof(IActorFactory<ActorTypes, IDrawableActor>), _entityFactory);

        }


        private void StartInputService()
        {
            var inputservice = new InputService(this);
        }
    }

    public class ProbeComponent : DrawableGameComponent
    {
        public ProbeComponent(Game game) : base(game)
        {
            game.Components.Add(this);
        }

        public override void Initialize()
        {
            Console.WriteLine("before base.Initialize()");

            base.Initialize();

            Console.WriteLine("after base.Initialize()");
        }

        protected override void LoadContent()
        {
            Console.WriteLine("before base.LoadContent()");

            base.LoadContent();

            Console.WriteLine("after base.LoadContent()");
        }

        public override void Update(GameTime gameTime)
        {
            Console.WriteLine("before base.Update()");

            base.Update(gameTime);

            Console.WriteLine("after base.Update()");
        }

        public override void Draw(GameTime gameTime)
        {
            Console.WriteLine("before base.Draw()");

            base.Draw(gameTime);

            Console.WriteLine("after base.Draw()");
        }
    }
}
