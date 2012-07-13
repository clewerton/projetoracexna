using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TangoGames.RoadFighter.Components;
using TangoGames.RoadFighter.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TangoGames.RoadFighter.Scenes
{
    public class End : IScene, ISimpleUpdateable, ISimpleDrawable
    {
        public End(Game game) 
        { 
            Game = game;

            _arial = game.Content.Load<SpriteFont>("arial");
        }

        public void Enter()
        { }

        public void Update(GameTime gameTime)
        {
            ISceneManager<MainGame.Scenes> sceneManager = (ISceneManager<MainGame.Scenes>) Game.Services.GetService(typeof(ISceneManager<MainGame.Scenes>));
        
            if(Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
               sceneManager.GoTo(MainGame.Scenes.Intro); 
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Game.GraphicsDevice.Clear(Color.DarkGoldenrod);

            spriteBatch.DrawString(_arial, "In END; click the LEFT BUTTON to go to INTRO", new Vector2(100), Color.Bisque);
        }

        public void Leave()
        { }

        #region Properties & Fields

        public Game Game { get; private set; }

        private SpriteFont _arial;
        #endregion
    }
}
