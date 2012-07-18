﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using TangoGames.RoadFighter.Scenes;

namespace TangoGames.RoadFighter.Levels
{
    public class Fase1 : Scene
    {
        public Fase1(Game game) : base(game) { }

        HUD hudteste;

        protected override void LoadContent()
        {
            base.LoadContent();

            _arial = Game.Content.Load<SpriteFont>("arial");
            SpriteBatch = new SpriteBatch(Game.GraphicsDevice);

            hudteste = new HUD(Game.Content);

        }

        public override void Update(GameTime gameTime)
        {
            var sceneManager = (ISceneManagerService<MainGame.Scenes>)Game.Services.GetService(typeof(ISceneManagerService<MainGame.Scenes>));

            if (Keyboard.GetState().IsKeyDown(Keys.K))
            {
                sceneManager.GoTo(MainGame.Scenes.End);
            }

            hudteste.Update(gameTime);

        }

        public override void Draw(GameTime gameTime)
        {
            Game.GraphicsDevice.Clear(Color.Azure);

            SpriteBatch.Begin();

            SpriteBatch.DrawString(_arial, "In INTRO; press K to go to END", new Vector2(100), Color.BurlyWood);

            hudteste.Draw(gameTime, SpriteBatch);

            SpriteBatch.End();
        }

        #region Properties & Fields
        private SpriteFont _arial;
        private SpriteBatch SpriteBatch { get; set; }
        #endregion
    }
}
