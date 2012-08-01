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
        private float _acceleration = 0.05F;
        private int _maxSpeed = 15;

        public Fase1(Game game) : base(game) { }

        protected override void LoadContent()
        {
            _arial = Game.Content.Load<SpriteFont>("arial");

            var actorFactory = GetService<IActorFactory<MainGame.ActorTypes, IDrawableActor>>();

            hud = new HUD(Game.Content);
            map = new Map(Game);
            enemies = new EnemiesManager(this);

            road1 = actorFactory[MainGame.ActorTypes.StraightRoad1];
            road1.SpriteBatch = this.SpriteBatch;
            road1.Location = new Vector2(0.0F, 0.0F);
            road1.Velocity  = Vector2.Zero;
            road1.Scrollable = true;
            map.Add(road1);

            //Atribui as pistas da estrada para o gestor de inimigos conhecer
            enemies.CurrentRoad = ((IRoad)road1).Lanes;

            road2 = actorFactory[MainGame.ActorTypes.StraightRoad2];
            road2.SpriteBatch = this.SpriteBatch;
            road2.Location = new Vector2((float)road1.Bounds.Left, (float)road1.Location.Y - road2.Bounds.Height);
            road2.Velocity = Vector2.Zero;
            road2.Scrollable = true;
            map.Add(road2);

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

            //aceleração do heroi
            map.Velocity = new Vector2(map.Velocity.X, map.Velocity.Y + _acceleration);
            if (map.Velocity.Y > _maxSpeed) map.Velocity = new Vector2(map.Velocity.X, _maxSpeed);

            map.Update(gameTime);
            hud.Update(gameTime);

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
        private IDrawableActor road1;
        private IDrawableActor road2;
        private HUD hud;


        /// <summary>
        /// Controle de inimigos no map
        /// </summary>
        private EnemiesManager enemies; 

        #endregion
    }
}
