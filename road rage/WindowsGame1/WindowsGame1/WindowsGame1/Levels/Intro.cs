using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using TangoGames.RoadFighter.Scenes;

namespace TangoGames.RoadFighter.Levels
{
    public class Intro : DrawableScene
    {
        public Intro(Game game) : base(game) {}

        protected override void LoadContent()
        {
            base.LoadContent();

            _arial = Game.Content.Load<SpriteFont>("arial");
            SpriteBatch = new SpriteBatch(Game.GraphicsDevice);
        }

        public override void Update(GameTime gameTime)
        {
            var sceneManager = (ISceneManagerService<MainGame.Scenes>) Game.Services.GetService(typeof(ISceneManagerService<MainGame.Scenes>));
        
            if(Keyboard.GetState().IsKeyDown(Keys.Space))
            {
               sceneManager.GoTo(MainGame.Scenes.End); 
            }
        }

        public override void Draw(GameTime gameTime)
        {
            Game.GraphicsDevice.Clear(Color.Azure);

            SpriteBatch.Begin();
            SpriteBatch.DrawString(_arial, "In INTRO; press SPACE to go to END", new Vector2(100), Color.BurlyWood);
            SpriteBatch.End();
        }

        #region Properties & Fields
        private SpriteFont _arial;
        private SpriteBatch SpriteBatch { get; set; }
        #endregion
    }
}
