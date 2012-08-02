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

            var actorFactory = GetService<IActorFactory<MainGame.ActorTypes, IDrawableActor>>();

            map = new Map(Game);
            hud = new HUD(Game.Content, map);
            enemies = new EnemiesManager(this);

            //Atribui as pistas da estrada para o gestor de inimigos conhecer
            enemies.CurrentRoad = ((IRoad)map.Road).Lanes;

            hero = actorFactory[MainGame.ActorTypes.Hero];
            hero.SpriteBatch = this.SpriteBatch;
            hero.Location = new Vector2(300, 500);
            hero.Velocity = Vector2.Zero;
            hero.Scrollable = true;
            map.Add(hero);

            map.Velocity = Vector2.Zero;
            
            base.LoadContent();

            map.ColisionsOccours += OnColisionsOccours;

            //inicia a geração de inimigos na estrada
            enemies.startGeneration(map);
        }

        public override void Update(GameTime gameTime)
        {
            var sceneManager = GetService<ISceneManagerService<MainGame.Scenes>>();

            if (Keyboard.GetState().IsKeyDown(Keys.K))
            {
                sceneManager.GoTo(MainGame.Scenes.Menu);
            }

            map.Update(gameTime);
            hud.Update(gameTime, map.Velocity);

        }

        protected override void DrawBefore(GameTime gameTime)
        {
            Game.GraphicsDevice.Clear(Color.Azure);
        }

        protected override void DrawAfter(GameTime gameTime)
        {
            SpriteBatch.Begin();

            SpriteBatch.DrawString(_arial, "In FASE; press K to go to MENU", new Vector2(300), Color.BurlyWood);

            map.Draw(gameTime);
            hud.Draw(gameTime, SpriteBatch);
            SpriteBatch.End();
        }

        #region colision

        public void OnColisionsOccours(Object sender, CollisionEventArgs args)
        {
            //Console.WriteLine(args.ColliderA + " bateu no ator " + args.ColliderB);
   
            #region colisão do heroi com a estrada
            //colisão com a estrada joga a configuração da nova estrada para 
            //o carro do heroi
            if (args.ColliderA is IRoad && args.ColliderB is Heroi) 
            {
                args.ColliderA.Collidable = false;
                ((Heroi)args.ColliderB).CurrentRoad = ((IRoad)args.ColliderA).Lanes;
                return;
            }
            if (args.ColliderB is IRoad && args.ColliderA is Heroi)
            {
                args.ColliderB.Collidable = false;
                ((Heroi)args.ColliderB).CurrentRoad = ((IRoad)args.ColliderB).Lanes;
                return;
            }
            #endregion

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


        /// <summary>
        /// Controle de inimigos no map
        /// </summary>
        private EnemiesManager enemies; 

        #endregion
    }
}
