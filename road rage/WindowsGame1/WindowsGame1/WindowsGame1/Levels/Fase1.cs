using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using TangoGames.RoadFighter.Scenes;
using TangoGames.RoadFighter.Actors;

namespace TangoGames.RoadFighter.Levels
{
    public class Fase1 : Scene
    {
        public Fase1(Game game) : base(game) { }

        protected override void LoadContent()
        {
            _arial = Game.Content.Load<SpriteFont>("arial");
            gameOverTexture = Game.Content.Load<Texture2D>("Textures/gameover");

            var actorFactory = GetService<IActorFactory<MainGame.ActorTypes, IDrawableActor>>();

            map = new Map(this);
            hud = new HUD(this, map);
            enemies = new EnemiesManager(this);

            //Atribui as pistas da estrada para o gestor de inimigos conhecer
            enemies.CurrentRoad = ((IRoad)map.Road).Lanes;

            hero = new Heroi(Game, map);
            hero.SpriteBatch = this.SpriteBatch;
            hero.Velocity = Vector2.Zero;
            hero.Scrollable = true;
            map.Add(hero);
            map.ChangeLaneRegister( (IChangeLanelistener )hero);
            map.Velocity = Vector2.Zero;
            
            base.LoadContent();

            map.ColisionsOccours += OnColisionsOccours;
            map.ChangeRoadType += OnChangeRoad;

            //inicia a geração de inimigos na estrada
            enemies.startGeneration(map);
        }

        public override void Update(GameTime gameTime)
        {
            var sceneManager = GetService<ISceneManagerService<MainGame.Scenes>>();
            if (!gameIsOver)
            {

                if (Keyboard.GetState().IsKeyDown(Keys.K))
                {
                    sceneManager.GoTo(MainGame.Scenes.Menu);
                }

                map.Update(gameTime);

                //update da HUD deve ser feito depois do calcula da velocidade do mapa
                hud.Update(gameTime);

                if ((map.Velocity.Y <= 0) && map.EndOfGas)
                {
                    gameIsOver = true;
                }

            }
            else
            {
                if (timer > 0)
                {
                    timer -= gameTime.ElapsedGameTime.Milliseconds;
                }
                else 
                {
                    if (highScore)
                    {
                        sceneManager.GoTo(MainGame.Scenes.HighScore);
                    }
                }
            }

        }

        protected override void DrawBefore(GameTime gameTime)
        {
            Game.GraphicsDevice.Clear(Color.Azure);
        }

        protected override void DrawAfter(GameTime gameTime)
        {
            SpriteBatch.Begin();

            map.Draw(gameTime);
            hud.Draw(gameTime, SpriteBatch);

            if (gameIsOver)
            {
                SpriteBatch.Draw(gameOverTexture, new Rectangle((Game.Window.ClientBounds.Width - gameOverTexture.Bounds.Width) / 2, 
                    (Game.Window.ClientBounds.Height - gameOverTexture.Bounds.Height) / 2, 
                    gameOverTexture.Width, gameOverTexture.Height), Color.White);
            }
            SpriteBatch.End();
        }

        public void OnChangeRoad(Object sender, ChangeRoadEventArgs  args)
        {
            enemies.CurrentRoad = args.CurrentLanes;
        }


        #region colision

        public void OnColisionsOccours(Object sender, CollisionEventArgs args)
        {
            #region colisão do heroi com carro inimigo
            if (args.ColliderA is IEnemy && args.ColliderB is Heroi) 
            {
                ((Heroi)args.ColliderB).EnemyCollide( (Enemy)args.ColliderA, map ); 
                return;
            }
            if (args.ColliderB is IEnemy && args.ColliderA is Heroi)
            {
                ((Heroi)args.ColliderA).EnemyCollide( (Enemy)args.ColliderB, map );
                return;
            }
            #endregion

            #region colisão entre os inimigos
            //colisão entre os inimigos
            if (args.ColliderA is Enemy && args.ColliderB is Enemy)
            {
                enemies.EnemyInterCollision((Enemy)args.ColliderA, (Enemy)args.ColliderB);
                return;
            }

            #endregion

        }

        #endregion

        #region Properties & Fields
        private SpriteFont _arial;
        private IMap map;
        private IDrawableActor hero;
        private HUD hud;
        private bool gameIsOver = false;
        private float timer = 1000;
        private Texture2D gameOverTexture;
        private bool highScore = true;


        /// <summary>
        /// Controle de inimigos no map
        /// </summary>
        private EnemiesManager enemies; 

        #endregion
    }
}
