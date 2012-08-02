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
            // configurando o fundo
            BackgroundImage = Game.Content.Load<Texture2D>("Textures/menu-background");

            // configurando os botões da tela
            var clientBounds = Game.Window.ClientBounds;

            ToIntro = new Button(Game);
            ToIntro.OnClick += (sender, args) => GetService<ISceneManagerService<MainGame.Scenes>>().GoTo(MainGame.Scenes.Intro);
            ToIntro.Location = new Point((clientBounds.Width - ButtonWidth) / 2, (clientBounds.Height - ButtonHeight) / 2 - 2 * ButtonHeight - 2 * Padding);
            ToIntro.Size = new Vector2(ButtonWidth, ButtonHeight);
            ToIntro.Text = "To INTRO";
            Elements.Add(ToIntro);

            ToFase = new Button(Game);
            ToFase.OnClick += (sender, args) => GetService<ISceneManagerService<MainGame.Scenes>>().GoTo(MainGame.Scenes.Fase);
            ToFase.Location = new Point((clientBounds.Width - ButtonWidth) / 2, (clientBounds.Height - ButtonHeight) / 2 - ButtonHeight - Padding);
            ToFase.Size = new Vector2(ButtonWidth, ButtonHeight);
            ToFase.Text = "To FASE";
            ToFase.Texture = Game.Content.Load<Texture2D>("Widgets/Button");
            ToFase.Background = Color.White;
            Elements.Add(ToFase);

            ToEnd = new Button(Game);
            ToEnd.OnClick += (sender, args) => GetService<ISceneManagerService<MainGame.Scenes>>().GoTo(MainGame.Scenes.End);
            ToEnd.Location = new Point((clientBounds.Width - ButtonWidth) / 2, (clientBounds.Height - ButtonHeight) / 2);
            ToEnd.Size = new Vector2(ButtonWidth, ButtonHeight);
            ToEnd.Text = "To END";
            Elements.Add(ToEnd);

            ToCredits = new Button(Game);
            ToCredits.OnClick += (sender, args) => GetService<ISceneManagerService<MainGame.Scenes>>().GoTo(MainGame.Scenes.Credits);
            ToCredits.Location = new Point((clientBounds.Width - ButtonWidth) / 2, (clientBounds.Height - ButtonHeight) / 2 + ButtonHeight + Padding);
            ToCredits.Size = new Vector2(ButtonWidth, ButtonHeight);
            ToCredits.Text = "To CREDITS";
            Elements.Add(ToCredits);

            ToExit = new Button(Game);
            ToExit.OnClick += (sender, args) => Game.Exit();
            ToExit.Location = new Point((clientBounds.Width - ButtonWidth) / 2, (clientBounds.Height - ButtonHeight) / 2 + 2 * ButtonHeight + 2 * Padding);
            ToExit.Size = new Vector2(ButtonWidth, ButtonHeight);
            ToExit.Text = "EXIT";
            Elements.Add(ToExit);
        }

        protected override void DrawBefore(GameTime gameTime)
        {
            Game.GraphicsDevice.Clear(Color.Brown);

            SpriteBatch.Begin();
            SpriteBatch.Draw(BackgroundImage, Game.Window.ClientBounds, Color.White);
            SpriteBatch.End();
        }

        private Button ToIntro { get; set; }
        private Button ToFase { get; set; }
        private Button ToEnd { get; set; }
        private Button ToCredits { get; set; }
        private Button ToExit { get; set; }

        private Texture2D BackgroundImage { get; set; }

        private const int ButtonWidth = 150;
        private const int ButtonHeight = 60;
        private const int Padding = 5;
    }
}
