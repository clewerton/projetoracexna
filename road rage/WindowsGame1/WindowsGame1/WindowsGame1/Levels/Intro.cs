using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using TangoGames.RoadFighter.Scenes;
using System;

namespace TangoGames.RoadFighter.Levels
{
    public class Intro : Scene
    {
        public Intro(Game game) : base(game) {}

        protected override void LoadContent()
        {
            base.LoadContent();

            _arial = Game.Content.Load<SpriteFont>("arial");
            _timeElapsed = TimeSpan.Zero;
            SpriteBatch = new SpriteBatch(Game.GraphicsDevice);
        }

        public override void Update(GameTime gameTime)
        {
            _timeElapsed += gameTime.ElapsedGameTime;
 
            var sceneManager = (ISceneManagerService<MainGame.Scenes>) Game.Services.GetService(typeof(ISceneManagerService<MainGame.Scenes>));
        
            if(Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
               sceneManager.GoTo(MainGame.Scenes.Fase);
               return;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            var sceneManager = (ISceneManagerService<MainGame.Scenes>) Game.Services.GetService(typeof(ISceneManagerService<MainGame.Scenes>));

            Game.GraphicsDevice.Clear(Color.Azure);

            SpriteBatch.Begin();
            SpriteBatch.DrawString(_arial, "In INTRO; press ENTER to go to FASE", new Vector2(100), Color.BurlyWood);
            SpriteBatch.DrawString(_arial, "In INTRO; press SPACE to " + (Paused ? "resume" : "pause"), new Vector2(100, 130), Color.BurlyWood);
            SpriteBatch.DrawString(_arial, "Time: " + _timeElapsed, new Vector2(100, 160), Color.BurlyWood);
            SpriteBatch.End();
        }

        #region Properties & Fields
        private SpriteFont _arial;
        private TimeSpan _timeElapsed;
        private SpriteBatch SpriteBatch { get; set; }
        #endregion
    }
}
