using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using TangoGames.RoadFighter.Input;
using TangoGames.RoadFighter.Scenes;
using System;
using TangoGames.RoadFighter.Widgets;

namespace TangoGames.RoadFighter.Levels
{
    public class Intro : Scene
    {
        public Intro(Game game) : base(game)
        {
            _timeElapsed = TimeSpan.Zero;   
        }

        protected override void LoadContent()
        {
            _arial = Game.Content.Load<SpriteFont>("arial");

            BackgroundImage = Game.Content.Load<Texture2D>("Textures/intro-background");
            
            Text = new TextArea(Game);
            Text.Text = "Clique em qualquer lugar para começar!";
            Text.Font = _arial;
            Elements.Add(Text);
        }

        public override void Update(GameTime gameTime)
        {
            _timeElapsed += gameTime.ElapsedGameTime;

            var sceneManager = GetService<ISceneManagerService<MainGame.Scenes>>();
            var input = GetService<IInputService>();
        
            if(input.MouseClick(Game.Window.ClientBounds)) // se clicou dentro da tela, vá para o menu
            {
               sceneManager.GoTo(MainGame.Scenes.Menu);
               return;
            }

            base.Update(gameTime);
        }

        protected override void DrawBefore(GameTime gameTime)
        {
            Game.GraphicsDevice.Clear(Color.Azure);

            SpriteBatch.Begin();
            SpriteBatch.Draw(BackgroundImage, Game.Window.ClientBounds, Color.White);
            SpriteBatch.End();

            // posicionando o texto
            var screenWidth = Game.Window.ClientBounds.Width;
            var screenHeight = Game.Window.ClientBounds.Height;

            Text.Location = new Point((int) (screenWidth - Text.Size.X)/2, (int) (screenHeight - Text.Size.Y)/2);
        }

        #region Properties & Fields

        private Texture2D BackgroundImage { get; set; }
        private TextArea Text { get; set; }

        private SpriteFont _arial;
        private TimeSpan _timeElapsed;
        #endregion
    }
}
