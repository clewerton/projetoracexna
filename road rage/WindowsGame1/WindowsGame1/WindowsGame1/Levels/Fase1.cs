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

            hero = actorFactory[MainGame.ActorTypes.Hero] as Heroi;
            hero.SpriteBatch = this.SpriteBatch;
            hero.Location = new Vector2(300, 500);
            hero.Velocity = new Vector2(0, -5);
            hero.Scrollable = true;
            map.Add(hero);
            
            map.Velocity = -hero.Velocity;

            Car car = actorFactory[MainGame.ActorTypes.Car] as Car;
            car.SpriteBatch = this.SpriteBatch;
            car.Location = new Vector2(500, 600);
            car.Velocity = new Vector2(0, -3);
            car.Scrollable = true;
            map.Add(car);
            
            Truck truck = actorFactory[MainGame.ActorTypes.Truck] as Truck;
            truck.SpriteBatch = this.SpriteBatch;
            truck.Location = new Vector2(400, 0);
            //truck.Velocity = new Vector2(0, -2);
            truck.Visible = true;
            truck.Scrollable = true;
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

        #region Properties & Fields
        private SpriteFont _arial;
        private IMap map;
        private Heroi hero;
        private HUD hud;
        #endregion
    }
}
