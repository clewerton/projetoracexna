using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TangoGames.RoadFighter.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using TangoGames.RoadFighter.Utils;
using Microsoft.Xna.Framework.Graphics;

namespace TangoGames.RoadFighter.Scenes
{
    public class Intro : IScene, ISimpleUpdateable, ISimpleDrawable
    {
        public Intro(Game game) 
        {
            Game = game;

            _arial = game.Content.Load<SpriteFont>("arial");
        }

        public void Enter() {}

        public void Update(GameTime gameTime)
        {
            var sceneManager = (ISceneManager<MainGame.Scenes>) Game.Services.GetService(typeof(ISceneManager<MainGame.Scenes>));
        
            if(Keyboard.GetState().IsKeyDown(Keys.Space))
            {
               sceneManager.GoTo(MainGame.Scenes.End); 
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Game.GraphicsDevice.Clear(Color.Azure);

            spriteBatch.DrawString(_arial, "In INTRO; press SPACE to go to END", new Vector2(100), Color.BurlyWood);
        }

        public void Leave() {}

        #region Properties & Fields

        public Game Game { get; private set; }

        private SpriteFont _arial;
        #endregion
    }
}
