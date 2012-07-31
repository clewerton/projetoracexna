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
            car.Location = new Vector2(500, 600);
            car.Velocity = new Vector2(0, -3);
            car.Scrollable = false;
            _currentMap.Add(car);

            IDrawableActor truck = actorFactory[MainGame.ActorTypes.Truck];
            truck.SpriteBatch = _currentScene.currentSpriteBatch; 
            truck.Location = new Vector2(400, 0);
            //truck.Velocity = new Vector2(0, -2);
            truck.Visible = true;
            truck.Scrollable = false;
            _currentMap.Add(truck);

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

        }

        #region Saiu da Tela

        public void OnOutOfBound(Object sender, OutOfBoundsEventArgs args)
        {
            if (args.OutActor is IEnemy) 
            {
                Console.WriteLine(args.OutActor + " saiu da tela ");
            }


        }
        #endregion


        #region Properties & Fields
        private IMap _currentMap;
        private Scene _currentScene;
        #endregion
    }
}
