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
        bool ChangeLane(int swicth);
    }

    public class EnemiesManager: GameComponent, IEnemiesManager
    {
        private Random random;

        private int _maxSpeed = 9;

        private int _minSpeed = 1;

        private int maxLanes;

        private int maxEnemies = 3;

        private int interval = 1000;

        private int pixelpass = 0;

        private int lineCount;

        private int frameCount;

        /// <summary>
        /// Contrutor do controle de inimigos
        /// </summary>
        /// <param name="game"></param>
        public EnemiesManager(Scene scene)
            : base(scene.Game)
        {
            _currentScene = scene;

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

            _maxSpeed = (int)(map.MaxSpeed * 0.85);

            _minSpeed = (int)(map.MaxSpeed * 0.15); ;

            _currentMap.OutOfBounds += OnOutOfBound;

            Game.Components.Add(this);

            var actorFactory = (IActorFactory<MainGame.ActorTypes, IDrawableActor>)Game.Services.GetService(typeof(IActorFactory<MainGame.ActorTypes, IDrawableActor>));


            //Carrega a lista de tipos de carro para garantir a presença de todos os tipos
            foreach (Enemy.EnemyTypes typeenemy in Enum.GetValues(typeof(Enemy.EnemyTypes))) { _ListofEnemies.Add(new Enemy(typeenemy, _currentScene, map)); }


            for (int i = 0; i < 7; i++) _ListofEnemies.Add(new Enemy(RandomEnemyType(), _currentScene, map  ));

            lineCount = 0;

            frameCount = 0;

            pixelpass = 2000;

        }

        public Enemy.EnemyTypes RandomEnemyType()
        {
            Array values = Enum.GetValues(typeof(Enemy.EnemyTypes));

            return (Enemy.EnemyTypes)values.GetValue(random.Next(values.Length));
        } 

        private void RandomizeEnemy(IDrawableActor enemy)
        {

            //calcula da velocidade do carro
            float speed = (float) ( random.NextDouble() * _maxSpeed );

            if (speed < _minSpeed) speed = _minSpeed;

            float LocY;

            int numlane;

            enemy.Velocity = new Vector2( 0 , - speed );

            //verifica se o carro inimigo e veloz ou lento em relação a velocidade atual do map;
            if (_currentMap.Velocity.Y < speed ) 
            {
                LocY = _currentScene.Game.Window.ClientBounds.Bottom - 1;
                numlane = random.Next(maxLanes);

            }
            else
            {
                LocY = -enemy.Bounds.Height;
                numlane = random.Next(maxLanes);
            }

            enemy.Location = new Vector2(currLanes.LanesList[numlane], LocY);

        }

        /// <summary>
        /// Finaliza geração de inimigos e remove instancias 
        /// </summary>
        public void stopGeneration()
        {
            Game.Components.Remove(this);
        }

        //float tempodecorrido = 0;


        /// <summary>
        /// ***  UPDATE 
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime) 
        {

            _minSpeed = (int)(_currentMap.Velocity.Y * 0.15);

            pixelpass += (int)_currentMap.Velocity.Y;  

            frameCount++;

            int frameTrigger = 400;
            if (_currentMap.Velocity.Y > 0) frameTrigger = random.Next((int)( 400 / _currentMap.Velocity.Y));  

            if (_currentMap.CheckPointCount < 5)
            {
                if (maxLanes == 2) { maxEnemies = 2; interval = 2500 ; }
                else if (maxLanes == 3) { maxEnemies = 4; interval = 2000; }
                else { maxEnemies = 5; interval = 1500; }
            }
            else 
            {
                if (maxLanes == 2) { maxEnemies = 3; interval = 2000; }
                else if (maxLanes == 3) { maxEnemies = 5; interval = 1500; }
                else { maxEnemies = 6; interval = 1000; }
            }

            if ( pixelpass < interval) { frameCount = 0; }

            //gera inimigos
            if ( frameCount > frameTrigger && !_currentMap.CheckPointReach && EnemiesActive.Count() < maxEnemies && EnemiesNotActive.Count() > 0 ) {

                frameCount = 0;

                lineCount++;

                if (lineCount < maxLanes)
                {
                    CreateEnemy();
                }
                else
                {
                    lineCount = 0;
                    pixelpass = 0;
                }

            }

        }

        private void CreateEnemy()
        {
            IEnemy ene = EnemiesNotActive.ElementAtOrDefault(random.Next(EnemiesNotActive.Count()));
            IDrawableActor enemyDraw = (IDrawableActor)ene;

            int count = 0;
            bool collid = true;
            do
            {
                RandomizeEnemy(enemyDraw);
                collid = CollisionTest((ICollidable)enemyDraw);
                count++;

            } while ((collid) && (count < 10));

            if (!collid)
            {
                enemyDraw.Outofscreen = false;

                _currentMap.Add(enemyDraw);
                _currentMap.ChangeLaneRegister((IChangeLanelistener)ene);

                ene.Active = true;
            }
        }


        public bool CollisionTest(ICollidable enemyBase)
        {
            foreach (IEnemy e in EnemiesActive)
            {
                ICollidable enemyActor = (ICollidable)e;
                if ((enemyActor != enemyBase) && (enemyBase.Collided(enemyActor))) return true;
            }
            return false;

        }

        public void EnemyInterCollision(Enemy enemyA, Enemy enemyB)
        {
            if (enemyA.Bounds.X == enemyB.Bounds.X)
            {
                if (enemyA.Bounds.Y > enemyB.Bounds.Y)
                {
                    EnemyHit(enemyA, enemyB);
                }
                else
                {
                    EnemyHit(enemyB, enemyA);
                }
            }
            else
            {
                if (enemyA.Velocity.Y > enemyB.Velocity.Y)
                {
                    EnemyHit(enemyA, enemyB);
                }
                else
                {
                    EnemyHit(enemyB, enemyA);
                }

                if (enemyA.Bounds.X > enemyB.Bounds.X)
                {
                    if (!enemyB.ChangeLane(-1) && !enemyA.ChangeLane(1))
                    { enemyA.Velocity = Vector2.Zero; }
                }
                else 
                {
                    if (!enemyA.ChangeLane(-1) && !enemyB.ChangeLane(1))
                    { enemyA.Velocity = Vector2.Zero; }
 
                }


            }

        }

        public void EnemyHit(Enemy hit, Enemy reached)
        {
            hit.Velocity = reached.Velocity +new Vector2(0, (float)random.NextDouble());
            reached.Velocity = reached.Velocity - new Vector2(0, (float)random.NextDouble());
            if (hit.Velocity.Y > 0) { hit.Velocity = new Vector2(hit.Velocity.X,0); }
            if (-reached.Velocity.Y > _maxSpeed) { reached.Velocity = new Vector2(0, -(float)_maxSpeed); }

        }

        #region Saiu da Tela

        public void OnOutOfBound(Object sender, OutOfBoundsEventArgs args)
        {
            if (args.OutActor is IEnemy) 
            {
                if ((args.OutActor.Bounds.Bottom < _currentScene.Game.Window.ClientBounds.Top - args.OutActor.Bounds.Height) || (args.OutActor.Bounds.Top > _currentScene.Game.Window.ClientBounds.Bottom + args.OutActor.Bounds.Height))
                {
                    ((IEnemy)args.OutActor).Active = false;
                    _currentMap.Remove(args.OutActor);
                    _currentMap.ChangeLaneUnRegister((IChangeLanelistener)args.OutActor);
                }
                else
                {
                    args.OutActor.Outofscreen = false;
                }
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

        private ILanes prevLanes;
        private ILanes currLanes;
        private ILanes nextLanes;

        public ILanes CurrentRoad 
        { 
            get { return currLanes ; } 
            set 
            { 
                currLanes = value;
                IRoad road = currLanes.Road;
                if (road.nextroad == null) { nextLanes = currLanes; }
                else { nextLanes = road.nextroad.Lanes; }
                prevLanes = road.prevroad.Lanes;
                maxLanes = currLanes.Count;
                maxLanes = Math.Min(maxLanes, nextLanes.Count);
                maxLanes = Math.Min(maxLanes, prevLanes.Count);
            }
        }

        #endregion

    }

}
