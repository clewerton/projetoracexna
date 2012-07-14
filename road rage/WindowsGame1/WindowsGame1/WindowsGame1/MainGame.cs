using Microsoft.Xna.Framework;
using TangoGames.RoadFighter.Levels;
using TangoGames.RoadFighter.Scenes;

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
        public enum Scenes { Intro, End }

        public MainGame()
        {
            _graphics = new GraphicsDeviceManager(this); // XXX precisa ficar no construtor
            Content.RootDirectory = "Content";

            // crie o gerenciador de telas
            _sceneManager = new SceneManager<Scenes>();
            
            // registre-o como provedor do serviço ISceneManagerService<Scenes>
            Services.AddService(typeof(ISceneManagerService<Scenes>), _sceneManager);

        }

        /// <summary>
        /// Registra todos os GameComponents do jogo e os inicializa junto com o gerenciador de cenas. 
        /// </summary>
        protected override void Initialize()
        {
            // registra as cenas conhecidas
            _sceneManager[Scenes.Intro] = new Intro(this);
            _sceneManager[Scenes.End] = new End(this);

            // a Intro começa como a cena atual
            _sceneManager.GoTo(Scenes.Intro);

            // inicializa todos os componentes registrados no jogo
            base.Initialize();
        }

        #region Properties & Fields
        readonly SceneManager<Scenes> _sceneManager;
        GraphicsDeviceManager _graphics; // XXX base.Initialize precisa de um serviço para iniciar o GraphicDevice
        #endregion
    }
}
