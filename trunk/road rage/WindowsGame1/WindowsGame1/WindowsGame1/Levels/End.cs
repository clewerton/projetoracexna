using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TangoGames.RoadFighter.Scenes;

namespace TangoGames.RoadFighter.Levels
{
    public class End : DrawableScene
    {
        public End(Game game) : base(game) {}

        protected override void LoadContent()
        {
            base.LoadContent();

            _arial = Game.Content.Load<SpriteFont>("arial");
            SpriteBatch = new SpriteBatch(Game.GraphicsDevice);
        }

        public override void Update(GameTime gameTime)
        {
            var sceneManagerService = (ISceneManagerService<MainGame.Scenes>) Game.Services.GetService(typeof(ISceneManagerService<MainGame.Scenes>));
        
            if(Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
               sceneManagerService.GoTo(MainGame.Scenes.Intro); 
            }
        }

        public override void Draw(GameTime gameTime)
        {
            Game.GraphicsDevice.Clear(Color.DarkGoldenrod);

            SpriteBatch.Begin();
            SpriteBatch.DrawString(_arial, "In END; click the LEFT BUTTON to go to INTRO", new Vector2(100), Color.Bisque);
            SpriteBatch.End();
        }

        #region Properties & Fields
        private SpriteFont _arial;
        private SpriteBatch SpriteBatch { get; set; }
        #endregion
    }
}
