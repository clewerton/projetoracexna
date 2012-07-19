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
            base.LoadContent();

            Game.IsMouseVisible = true;

            _arial = Game.Content.Load<SpriteFont>("arial");
            _timeElapsed = TimeSpan.Zero;
            SpriteBatch = new SpriteBatch(Game.GraphicsDevice);

            // XXX textura vazia para preencher o botão
            var dummyTexture = new Texture2D(GraphicsDevice, 1, 1);
            dummyTexture.SetData(new Color[] { Color.White });

            _button = new Button(dummyTexture, _arial);
            _button.Location = new Point(200, 300);
            _button.Size = new Vector2(120, 60);
            _button.Text = "To END";

            _button.OnClick += 
                (sender, args) =>
                {
                    var sceneManager = GetSceneManager<MainGame.Scenes>();

                    sceneManager.GoTo(MainGame.Scenes.End);
                };
        }

        public override void Update(GameTime gameTime)
        {
            _button.Update(gameTime);
            
            _timeElapsed += gameTime.ElapsedGameTime;

            var sceneManager = GetSceneManager<MainGame.Scenes>();
        
            if(Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
               sceneManager.GoTo(MainGame.Scenes.Menu);
               return;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            Game.GraphicsDevice.Clear(Color.Azure);

            SpriteBatch.Begin();
            SpriteBatch.DrawString(_arial, "In INTRO; press ENTER to go to MENU", new Vector2(100), Color.BurlyWood);
            SpriteBatch.DrawString(_arial, "In INTRO; press SPACE to " + (Paused ? "resume" : "pause"), new Vector2(100, 130), Color.BurlyWood);
            SpriteBatch.DrawString(_arial, "Time: " + _timeElapsed, new Vector2(100, 160), Color.BurlyWood);

            _button.Draw(gameTime, SpriteBatch);

            SpriteBatch.End();
        }

        #region Properties & Fields
        private SpriteBatch SpriteBatch { get; set; }
        private SpriteFont _arial;
        private TimeSpan _timeElapsed;
        private Button _button;
        #endregion
    }
}
