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
        private int _maxSpeed = 9;

        private int maxEnemies = 3;

        private int interval = 1000;

        private int timepass = 0;

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

            _currentMap.OutOfBounds += OnOutOfBound;

            Game.Components.Add(this);

            var actorFactory = (IActorFactory<MainGame.ActorTypes, IDrawableActor>)Game.Services.GetService(typeof(IActorFactory<MainGame.ActorTypes, IDrawableActor>));

            for (int i = 0; i < 16; i++) _ListofEnemies.Add(new Enemy(RandomEnemyType(), _currentScene, map  ));

        }

        public Enemy.EnemyTypes RandomEnemyType()
        {
            Array values = Enum.GetValues(typeof(Enemy.EnemyTypes));

            return (Enemy.EnemyTypes)values.GetValue(random.Next(values.Length));
        } 

        private void RandomizeEnemy(IDrawableActor enemy)
        {

            //calcula da velocidade do carro
            float speed = ( 1 + (float)( random.NextDouble() * (_maxSpeed-1) ) ) ;

            float LocY;

            int numlane;

            enemy.Velocity = new Vector2( 0 , - speed );

            //verifica se o carro inimigo e veloz ou lento em relação a velocidade atual do map;
            if (_currentMap.Velocity.Y < speed ) 
            {
                LocY = _currentScene.Game.Window.ClientBounds.Bottom - 1;
                numlane = random.Next(_lanes.StartIndex, _lanes.LastIndex + 1);

            }
            else
            {
                LocY = -enemy.Bounds.Height;
                numlane = random.Next(_lanes.StartIndex, _lanes.LastIndex + 1);

            }

            enemy.Location = new Vector2(_lanes.LanesList[numlane], LocY);

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
            timepass += (int) gameTime.ElapsedGameTime.TotalMilliseconds;

            if (_lanes.Count == 2) { maxEnemies = 3; interval = (EnemiesActive.Count() * 2000) ; }
            else if (_lanes.Count == 3) { maxEnemies = 6; interval = (EnemiesActive.Count() * 1000); }
            else { maxEnemies = 9; interval = (EnemiesActive.Count() * 500); }

            if ( EnemiesActive.Count() <  maxEnemies &&  timepass > interval && EnemiesNotActive.Count() >  0 )
            {
                timepass = 0;

                IEnemy ene = EnemiesNotActive.ElementAtOrDefault( random.Next (EnemiesNotActive.Count()));
                IDrawableActor enemyDraw = (IDrawableActor)ene;

                int count = 0;
                bool collid = true;
                do
                {
                  RandomizeEnemy( enemyDraw );
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
                    Console.WriteLine("Tirou" + args.OutActor);
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

        private ILanes _lanes;
        public ILanes CurrentRoad { get { return _lanes; } set { _lanes = value; } }

        #endregion

    }
}
