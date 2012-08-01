using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TangoGames.RoadFighter.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TangoGames.RoadFighter.Widgets;

namespace TangoGames.RoadFighter.Levels
{
    public class Credits : Scene
    {
        public Credits(Game game) : base(game) 
        {
            Text = "Desenvolvedores:\n"
                + "    Arthur Figueiredo\n"
                + "    Clewerton Coelho\n"
                + "    Diogo Honorato\n"
                + "    Humberto Anjos\n"
                + "Professor:\n"
                + "    Cléber Tavares";
            
            Font = Game.Content.Load<SpriteFont>("arial");
            
            Back = new Button(Game);
            Back.Text = "Back to MENU";
            Back.Size = new Vector2(ButtonWidth, ButtonHeight);
            Back.OnClick += (sender, args) => GetService<ISceneManagerService<MainGame.Scenes>>().GoTo(MainGame.Scenes.Menu);
            Elements.Add(Back);
        }

        protected override void DrawBefore(GameTime gameTime)
        {
            Game.GraphicsDevice.Clear(Color.Black);

            // ajustar a posição do botão
            var padding = 15;
            var size = Font.MeasureString(Text);
            var screenWidth = Game.Window.ClientBounds.Width;
            Back.Location = new Point((screenWidth - Back.Bounds.Width)/2, (int) (TextPosition.Y + size.Y + padding));
        }

        protected override void DrawAfter(GameTime gameTime)
        {
            SpriteBatch.Begin();

            SpriteBatch.DrawString(Font, Text, TextPosition, Color.White);

            SpriteBatch.End();
        }

        private Vector2 TextPosition 
        {
            get
            {
                var size = Font.MeasureString(Text);
                var screenWidth = Game.Window.ClientBounds.Width;
                var screenHeight = Game.Window.ClientBounds.Height;

                return new Vector2((screenWidth - size.X) / 2, (screenHeight - size.Y) / 2);
            }
        }

        private string Text { get; set; }
        private SpriteFont Font { get; set; }
        private Button Back { get; set; }

        private const int ButtonWidth = 200;
        private const int ButtonHeight = 60;
    }
}
