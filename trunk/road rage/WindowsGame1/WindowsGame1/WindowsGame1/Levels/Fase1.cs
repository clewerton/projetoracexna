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

            hud = new HUD(Game.Content);
            map = new Map(Game);
            enemies = new EnemiesManager(this);

            road1 = actorFactory[MainGame.ActorTypes.StraightRoad1];
            road1.SpriteBatch = this.SpriteBatch;
            road1.Location = Vector2.Zero;
            road1.Scrollable = true;
            map.Add(road1);

            //Atribui as pistas da estrada para o gestor de inimigos conhecer
            enemies.CurrentRoad = ((IRoad)road1).Lanes;

            road2 = actorFactory[MainGame.ActorTypes.StraightRoad2];
            road2.SpriteBatch = this.SpriteBatch;
            road2.Location = new Vector2(road1.Bounds.Left, road1.Location.Y - road2.Bounds.Height + 30);
            road2.Scrollable = true;
            map.Add(road2);

            hero = actorFactory[MainGame.ActorTypes.Hero];
            hero.SpriteBatch = this.SpriteBatch;
            hero.Location = new Vector2(300, 500);
            hero.Velocity = new Vector2(0, -5);
            hero.Scrollable = true;
            map.Add(hero);

            map.Velocity = -hero.Velocity;
            
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
   
            #region colisão com a estrada
            //colisão com a estrada joga a configuração da nova estrada para 
            //o carro do heroi
            if (args.ColliderA is IRoad && args.ColliderB is Heroi) 
            {
               args.ColliderA.Collidable = false;
               ((Heroi)args.ColliderB).CurrentRoad = ((IRoad)args.ColliderA).Lanes;
            }
            if (args.ColliderB is IRoad && args.ColliderA is Heroi)
            {
                args.ColliderB.Collidable = false;
                ((Heroi)args.ColliderB).CurrentRoad = ((IRoad)args.ColliderB).Lanes;
            }
            #endregion

            //if (!(args.ColliderA is Heroi)) args.ColliderA.Collidable = false;
            //if (!(args.ColliderB is Heroi)) args.ColliderB.Collidable = false;
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
