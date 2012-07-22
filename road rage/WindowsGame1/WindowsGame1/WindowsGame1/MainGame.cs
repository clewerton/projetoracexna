using Microsoft.Xna.Framework;
using TangoGames.RoadFighter.Levels;
using TangoGames.RoadFighter.Scenes;
using TangoGames.RoadFighter.Actors;
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
        /// Serão usados pelo gerenciador de cenas para identificar as cenas.
        /// </summary>
        public enum Scenes { Intro, End, Fase, Menu }
        public enum ActorTypes { Car, Truck }

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        public MainGame()
        {
            // XXX precisa ficar no construtor, pois base.Initialize precisa de um serviço para 
            // XXX iniciar o GraphicDevice.
            graphics = new GraphicsDeviceManager(this); 

            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Registra todos os GameComponents os inicializa junto com o gerenciador de cenas. 
        /// </summary>
        protected override void Initialize()
        {
            // configura o gerenciamento de cenas
            StartSceneManager();
            StartEntityFactory();

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

            // registra-o como provedor do serviço ISceneManagerService<Scenes>
            Services.AddService(typeof (ISceneManagerService<Scenes>), sceneManager);

            // registra as cenas conhecidas
            sceneManager[Scenes.Intro] = new Intro(this);
            sceneManager[Scenes.End] = new End(this);
            sceneManager[Scenes.Fase] = new Fase1(this);
            sceneManager[Scenes.Menu] = new Menu(this);

            // a Intro começa como a cena atual
            sceneManager.GoTo(Scenes.Intro);
        }

        private void StartEntityFactory()
        {
            ActorFactory<ActorTypes, IDrawableActor> _entityFactory = new ActorFactory<ActorTypes, IDrawableActor>(); 
            _entityFactory[ActorTypes.Car] = new Car(this, new Rectangle(0, 0, 100, 100), spriteBatch);
            _entityFactory[ActorTypes.Truck] = new Truck(this, new Rectangle(0, 0, 50, 50), spriteBatch);
            Services.AddService(typeof(IActorFactory<ActorTypes, IDrawableActor>), _entityFactory);

        }

    }
}
