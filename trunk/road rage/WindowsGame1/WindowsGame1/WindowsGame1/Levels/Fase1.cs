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
            map.Velocity = new Vector2(0, 1);

            hero = new Heroi(Game);
            hud = new HUD(Game.Content);

            Car car = actorFactory[MainGame.ActorTypes.Car] as Car;
            car.SpriteBatch = this.SpriteBatch;
            car.Location = new Vector2(100, 0);
            car.Velocity = new Vector2(0, -2);
            car.Scrollable = true;
            map.Add(car);
            
            Truck truck = actorFactory[MainGame.ActorTypes.Truck] as Truck;
            truck.SpriteBatch = this.SpriteBatch;
            truck.Location = new Vector2(300, 0);
            //truck.Velocity = new Vector2(0, 1);
            truck.Visible = true;
            map.Add(truck);

            base.LoadContent();
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
            hero.Update(gameTime);
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
            hero.Draw(gameTime, SpriteBatch);
            SpriteBatch.End();
        }

        #region Properties & Fields
        private SpriteFont _arial;
        private IMap map;
        private Heroi hero;
        private HUD hud;
        #endregion
    }
}
