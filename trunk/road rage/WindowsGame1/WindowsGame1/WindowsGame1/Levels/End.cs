using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TangoGames.RoadFighter.Scenes;
using System;

namespace TangoGames.RoadFighter.Levels
{
    public class End : Scene
    {
        public End(Game game) : base(game)
        {
            _timeElapsed = TimeSpan.Zero;
        }

        protected override void LoadContent()
        {
            _arial = Game.Content.Load<SpriteFont>("arial");
        }

        public override void Update(GameTime gameTime)
        {
            _timeElapsed += gameTime.ElapsedGameTime;

            var sceneManager = GetSceneManager<MainGame.Scenes>();
        
            if(Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
               sceneManager.GoTo(MainGame.Scenes.Menu); 
            }
        }

        protected override void DrawBefore(GameTime gameTime)
        {
            Game.GraphicsDevice.Clear(Color.DarkGoldenrod);
        }

        protected override void DrawAfter(GameTime gameTime)
        {
            SpriteBatch.Begin();
            SpriteBatch.DrawString(_arial, "In END; click the LEFT BUTTON to go to MENU", new Vector2(100), Color.Bisque);
            SpriteBatch.DrawString(_arial, "In END; press SPACE to " + (Paused ? "resume" : "pause"), new Vector2(100, 130), Color.Bisque);
            SpriteBatch.DrawString(_arial, "Time: " + _timeElapsed, new Vector2(100, 160), Color.Bisque);
            SpriteBatch.End();
        }

        #region Properties & Fields
        private SpriteFont _arial;
        private TimeSpan _timeElapsed;
        #endregion
    }
}
