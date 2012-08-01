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
        private Random random;

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

            int TheSeed = (int)DateTime.Now.Ticks;
            random = new Random(TheSeed);

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

            for (int i = 0; i < 10; i++) _ListofEnemies.Add(new Enemy(RandomEnemyType(), _currentScene ));

            //_ListofEnemies.Add(new Enemy(Enemy.EnemyTypes.Inimigo1, _currentScene));
            //_ListofEnemies.Add(new Enemy(Enemy.EnemyTypes.Inimigo2, _currentScene));
            //_ListofEnemies.Add(new Enemy(Enemy.EnemyTypes.Inimigo3, _currentScene));
            //_ListofEnemies.Add(new Enemy(Enemy.EnemyTypes.Inimigo4, _currentScene));
            //_ListofEnemies.Add(new Enemy(Enemy.EnemyTypes.Inimigo5, _currentScene));
            //_ListofEnemies.Add(new Enemy(Enemy.EnemyTypes.Inimigo6, _currentScene));
            //_ListofEnemies.Add(new Enemy(Enemy.EnemyTypes.Inimigo7, _currentScene));
            //_ListofEnemies.Add(new Enemy(Enemy.EnemyTypes.Inimigo8, _currentScene));

        }

        public Enemy.EnemyTypes RandomEnemyType()
        {
            Array values = Enum.GetValues(typeof(Enemy.EnemyTypes));

            return (Enemy.EnemyTypes)values.GetValue(random.Next(values.Length));
        } 

        private void RandomizeEnemy(IDrawableActor enemy)
        {
            int numlane = random.Next (_lanes.StartIndex, _lanes.LastIndex);

            enemy.Location = new Vector2(_lanes.LanesList[ numlane ] , -enemy.Bounds.Height);

            enemy.Velocity = new Vector2(0, -random.Next (1, 5));

            enemy.Outofscreen = false;

            _currentMap.Add( enemy );
        }

        /// <summary>
        /// Finaliza geração de inimigos e remove instancias 
        /// </summary>
        public void stopGeneration()
        {
            Game.Components.Remove(this);
        }

        float tempodecorrido = 0;

        public override void Update(GameTime gameTime) 
        {
            tempodecorrido += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (tempodecorrido >= 3000 && EnemiesNotActive.Count()>0)
            {
                tempodecorrido = 0;

                IEnemy ene = EnemiesNotActive.ElementAtOrDefault( random.Next (EnemiesNotActive.Count()));

                RandomizeEnemy((IDrawableActor)ene);

                ene.Active = true;

            }

        }

        #region Saiu da Tela

        public void OnOutOfBound(Object sender, OutOfBoundsEventArgs args)
        {
            if (args.OutActor is IEnemy) 
            {
                ((IEnemy)args.OutActor).Active = false;
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
