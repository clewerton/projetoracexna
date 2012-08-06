using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TangoGames.RoadFighter.Scenes;
using Microsoft.Xna.Framework.Graphics;
using TangoGames.RoadFighter.Widgets;
using Microsoft.Xna.Framework;
using TangoGames.RoadFighter.Input;

namespace TangoGames.RoadFighter.Levels
{
    class HighScore : Scene
    {

        public HighScore(Game game) : base(game) 
        {
            startX = game.Window.ClientBounds.Width / 4;
            startY = game.Window.ClientBounds.Height / 2;
        }

        protected override void LoadContent()
        {
            // configurando o fundo
            BackgroundImage = Game.Content.Load<Texture2D>("Textures/menu-background");

            // configurando os botões da tela
            var clientBounds = Game.Window.ClientBounds;

            Button letterA = new Button(Game);
            letterA.OnClick += (sender, args) => playerName += 'A';
            letterA.Location = new Point((clientBounds.Width - ButtonWidth) / 2, (clientBounds.Height - ButtonHeight) / 2 - 2 * ButtonHeight - 2 * Padding);
            letterA.Size = new Vector2(ButtonWidth, ButtonHeight);
            letterA.Text = "[A]";
            Elements.Add(letterA);

        }

        public override void Update(GameTime gameTime)
        {
            var sceneManager = GetService<ISceneManagerService<MainGame.Scenes>>();
            var input = GetService<IInputService>();

            if (input.MouseClick(Game.Window.ClientBounds)) // se clicou dentro da tela, vá para o menu
            {
                sceneManager.GoTo(MainGame.Scenes.Menu);
                return;
            }

            base.Update(gameTime);
        }

        protected override void DrawBefore(GameTime gameTime)
        {
            Game.GraphicsDevice.Clear(Color.Brown);

            SpriteBatch.Begin();
            SpriteBatch.Draw(BackgroundImage, Game.Window.ClientBounds, Color.White);
            SpriteBatch.End();
        }

        private Texture2D BackgroundImage { get; set; }
        private const int ButtonWidth = 150;
        private const int ButtonHeight = 60;
        private const int Padding = 5;
        private String playerName;
        private int startX;
        private int startY;

    }
}
