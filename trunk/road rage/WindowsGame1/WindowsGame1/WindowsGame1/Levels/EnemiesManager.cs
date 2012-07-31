using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TangoGames.RoadFighter.Scenes;
using TangoGames.RoadFighter.Actors;


namespace TangoGames.RoadFighter.Levels
{

    public interface IEnemiesManager
    {
        void startGeneration(IMap map);
        void stopGeneration();

    }

    public interface IEnemy
    {
        bool Active { get; set; }
    }

    public class EnemiesManager: GameComponent, IEnemiesManager
    {
        /// <summary>
        /// Contrutor do controle de inimigos
        /// </summary>
        /// <param name="game"></param>
        public EnemiesManager(Scene scene)
            : base(scene.Game)
        {
            _currentScene = scene;

            //cria definicão de pistas inicial deve ser atualizado pelo método CurrentRoad
            _lanes = new FourLanes(); 

            //lista de inimigos fora de ação
            _ListofEnemies = new List<IEnemy>();
        }
        
        /// <summary>
        /// Inicia a geração de inimigos e cria instancias dos inimigos
        /// </summary>
        public void startGeneration(IMap map) 
        {
            _currentMap = map;

            _currentMap.OutOfBounds += OnOutOfBound;

            Game.Components.Add(this);

            var actorFactory = (IActorFactory<MainGame.ActorTypes, IDrawableActor>)Game.Services.GetService(typeof(IActorFactory<MainGame.ActorTypes, IDrawableActor>));

            IDrawableActor car = actorFactory[MainGame.ActorTypes.Car];
            car.SpriteBatch = _currentScene.currentSpriteBatch;
            car.Visible = false;
            car.Scrollable = false;
            _ListofEnemies.Add((IEnemy)car);

            IDrawableActor truck = actorFactory[MainGame.ActorTypes.Truck];
            truck.SpriteBatch = _currentScene.currentSpriteBatch;
            truck.Visible = false;
            truck.Scrollable = false;
            _ListofEnemies.Add((IEnemy)truck);

        }

        private void RandomizeEnemy(IDrawableActor enemy)
        {
            int numlane = RandomNumber(_lanes.StartIndex, _lanes.LastIndex);

            enemy.Location = new Vector2(_lanes.LanesList[ numlane ] , -enemy.Bounds.Height);

            enemy.Velocity = new Vector2(0, -RandomNumber(1, 4));

            enemy.Outofscreen = false;

            _currentMap.Add( enemy );
        }


        private int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }

        /// <summary>
        /// Finaliza geração de inimigos e remove instancias 
        /// </summary>
        public void stopGeneration()
        {
            Game.Components.Remove(this);
        }

        public override void Update(GameTime gameTime) 
        {
            if (RandomNumber(0, 100) > 90)
            {
            //    IEnemy ene = EnemiesNotActive.FirstOrDefault();

            //    RandomizeEnemy((IDrawableActor)ene);

             //   ene.Active = true;
            }
        }

        #region Saiu da Tela

        public void OnOutOfBound(Object sender, OutOfBoundsEventArgs args)
        {
            if (args.OutActor is IEnemy) 
            {
                _currentMap.Remove(args.OutActor);
            }
        }


        #endregion


        #region Properties & Fields

        private IMap _currentMap;
        private Scene _currentScene;

        private List<IEnemy> _ListofEnemies;

        /// <summary>
        /// Os atores ativos. Esta propriedade é calculada a partir da 
        /// propriedade <see cref="_ListofEnemies"/>, então é somente para leitura.
        /// </summary>
        protected IEnumerable<IEnemy> EnemiesActive
        {
            get
            {
                return from e in _ListofEnemies 
                       where e.Active 
                       select e as IEnemy;
            }
        }
        /// <summary>
        /// Os atores Nao ativos. Esta propriedade é calculada a partir da 
        /// propriedade <see cref="_ListofEnemies"/>, então é somente para leitura.
        /// </summary>
        protected IEnumerable<IEnemy> EnemiesNotActive
        {
            get
            {
                return from e in _ListofEnemies
                       where !e.Active
                       select e as IEnemy;
            }
        }
        #endregion

        #region Controle de Pistas

        private ILanes _lanes;
        public ILanes CurrentRoad { get { return _lanes; } set { _lanes = value; } }

        #endregion

    }
}
