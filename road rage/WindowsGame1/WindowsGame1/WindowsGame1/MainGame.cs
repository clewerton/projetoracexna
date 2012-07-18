using Microsoft.Xna.Framework;
using TangoGames.RoadFighter.Levels;
using TangoGames.RoadFighter.Scenes;
using TangoGames.RoadFighter.Actors;
using Microsoft.Xna.Framework.Input;

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
        public enum Scenes { Intro, End, Fase }
        public enum EntityTypes { Basic }

        public MainGame()
        {
            // XXX precisa ficar no construtor, pois base.Initialize precisa de um serviço para 
            // XXX iniciar o GraphicDevice.
            new GraphicsDeviceManager(this); 

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

            // a Intro começa como a cena atual
            sceneManager.GoTo(Scenes.Intro);
        }

        private void StartEntityFactory()
        {
	        EntityFactory<EntityTypes> _entityFactory = new EntityFactory<EntityTypes>(); 
            _entityFactory[EntityTypes.Basic] = new BasicActor(this, new Rectangle(0, 0, 100, 100));
            Services.AddService(typeof(IEntityFactory<EntityTypes>), _entityFactory);

        }


    }
}
