using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TangoGames.RoadFighter.Scenes;
using TangoGames.RoadFighter.Widgets;

namespace TangoGames.RoadFighter.Levels
{
    public class Menu : Scene
    {
        public Menu(Game game) : base(game) {}

        protected override void LoadContent()
        {
            // configurando os botões da tela
            var clientBounds = Game.Window.ClientBounds;

            ToIntro = new Button(this);
            ToIntro.OnClick += (sender, args) => GetSceneManager<MainGame.Scenes>().GoTo(MainGame.Scenes.Intro);
            ToIntro.Location = new Point((clientBounds.Width - ButtonWidth) / 2, (clientBounds.Height - ButtonHeight) / 2 - ButtonHeight - Padding);
            ToIntro.Size = new Vector2(ButtonWidth, ButtonHeight);
            ToIntro.Text = "To INTRO";

            ToFase = new Button(this);
            ToFase.OnClick += (sender, args) => GetSceneManager<MainGame.Scenes>().GoTo(MainGame.Scenes.Fase);
            ToFase.Location = new Point((clientBounds.Width - ButtonWidth) / 2, (clientBounds.Height - ButtonHeight) / 2);
            ToFase.Size = new Vector2(ButtonWidth, ButtonHeight);
            ToFase.Text = "To FASE";

            ToEnd = new Button(this);
            ToEnd.OnClick += (sender, args) => GetSceneManager<MainGame.Scenes>().GoTo(MainGame.Scenes.End);
            ToEnd.Location = new Point((clientBounds.Width - ButtonWidth) / 2, (clientBounds.Height + ButtonHeight) / 2 + Padding);
            ToEnd.Size = new Vector2(ButtonWidth, ButtonHeight);
            ToEnd.Text = "To END";

            base.LoadContent();
        }

        public override void DrawBefore(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Game.GraphicsDevice.Clear(Color.Brown);
        }

        public Button ToIntro { get; private set; }
        public Button ToFase { get; private set; }
        public Button ToEnd { get; private set; }

        private const int ButtonWidth = 150;
        private const int ButtonHeight = 60;
        private const int Padding = 5;
    }
}
