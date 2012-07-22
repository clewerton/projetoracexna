using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using TangoGames.RoadFighter.Scenes;
using System;
using TangoGames.RoadFighter.Widgets;

namespace TangoGames.RoadFighter.Levels
{
    public class Intro : Scene
    {
        public Intro(Game game) : base(game) {}

        protected override void LoadContent()
        {
            Game.IsMouseVisible = true;

            _arial = Game.Content.Load<SpriteFont>("arial");
            _timeElapsed = TimeSpan.Zero;

            _button = new Button(this);
            _button.Location = new Point(200, 300);
            _button.Size = new Vector2(120, 60);
            _button.Text = "To END";

            _button.OnClick += 
                (sender, args) =>
                {
                    var sceneManager = GetSceneManager<MainGame.Scenes>();

                    sceneManager.GoTo(MainGame.Scenes.End);
                };

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            _timeElapsed += gameTime.ElapsedGameTime;

            var sceneManager = GetSceneManager<MainGame.Scenes>();
        
            if(Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
               sceneManager.GoTo(MainGame.Scenes.Menu);
               return;
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            Game.GraphicsDevice.Clear(Color.Azure);

            base.Draw(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            SpriteBatch.DrawString(_arial, "In INTRO; press ENTER to go to MENU", new Vector2(100), Color.BurlyWood);
            SpriteBatch.DrawString(_arial, "In INTRO; press SPACE to " + (Paused ? "resume" : "pause"), new Vector2(100, 130), Color.BurlyWood);
            SpriteBatch.DrawString(_arial, "Time: " + _timeElapsed, new Vector2(100, 160), Color.BurlyWood);
        }

        #region Properties & Fields
        private SpriteFont _arial;
        private TimeSpan _timeElapsed;
        private Button _button;
        #endregion
    }
}
