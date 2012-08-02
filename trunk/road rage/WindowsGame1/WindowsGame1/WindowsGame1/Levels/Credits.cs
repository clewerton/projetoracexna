using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TangoGames.RoadFighter.Scenes;
using TangoGames.RoadFighter.Utils;
using TangoGames.RoadFighter.Widgets;

namespace TangoGames.RoadFighter.Levels
{
    public class Credits : Scene
    {
        public Credits(Game game) : base(game) 
        {
            TextArea = new TextArea(game);
            TextArea.Text = "Desenvolvedores:\n"
                + "    Arthur Figueiredo\n"
                + "    Clewerton Coelho\n"
                + "    Diogo Honorato\n"
                + "    Humberto Anjos\n"
                + "\n\n"
                + "Professor:\n"
                + "    Cléber Tavares";
            TextArea.Background = Color.Gray;
            TextArea.Alpha = 128;
            Elements.Add(TextArea);
            
            Back = new Button(Game);
            Back.Text = "Back to MENU";
            Back.Size = new Vector2(ButtonWidth, ButtonHeight);
            Back.OnClick += (sender, args) => GetService<ISceneManagerService<MainGame.Scenes>>().GoTo(MainGame.Scenes.Menu);
            Elements.Add(Back);

            BackgroundImage = Game.Content.Load<Texture2D>("Textures/credits-background");
        }

        protected override void DrawBefore(GameTime gameTime)
        {
            Game.GraphicsDevice.Clear(Color.White);

            // ajustando a posição da área de texto (mas não desenhe ainda!)
            var screenWidth = Game.Window.ClientBounds.Width;
            var screenHeight = Game.Window.ClientBounds.Height;
            TextArea.Location = new Point((int) (screenWidth - TextArea.Size.X)/2, (int) (screenHeight - TextArea.Size.Y)/2);

            // ajustando a posição do botão (mas não desenhe ainda!)
            var padding = 15;
            Back.Location = new Point((screenWidth - Back.Bounds.Width) / 2, (int)(TextArea.Location.Y + TextArea.Size.Y + padding));
            Back.Size = new Vector2(TextArea.Size.X, Back.Size.Y);

            // desenhando o fundo
            SpriteBatch.Begin();

            SpriteBatch.Draw(BackgroundImage, Game.Window.ClientBounds, Color.White);

            SpriteBatch.End();
        }

        private TextArea TextArea { get; set; }
        private Button Back { get; set; }
        private Texture2D BackgroundImage { get; set; }

        private const int ButtonWidth = 200;
        private const int ButtonHeight = 60;
    }
}
