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
        private String[] characters;

        public HighScore(Game game) : base(game) 
        {
            characters = new String[4];
            characters[0] = "0123456789";
            characters[1] = "ABCDEFGHIJ";
            characters[2] = "KLMNOPQRST";
            characters[3] = "UVWXYZ";

            startX = game.Window.ClientBounds.Width / 4;
            startY = game.Window.ClientBounds.Height / 2;
        }

        protected override void LoadContent()
        {
            // configurando o fundo
            BackgroundImage = Game.Content.Load<Texture2D>("Textures/menu-background");
            font = Game.Content.Load<SpriteFont>("arial");
            playerName = "";

            // configurando os botões da tela
            var clientBounds = Game.Window.ClientBounds;

            int i, j;
            for (i = 0; i < characters.Length; i++)
            {
                for (j = 0; j < characters[i].Length; j++)
                {
                    Button letterButton = new Button(Game);
                    Char currentLetter = characters[i].ElementAt(j);
                    letterButton.OnClick += (sender, args) => playerName += currentLetter;
                    letterButton.Bounds = new Rectangle(0, 0, ButtonWidth, ButtonWidth);
                    letterButton.Location = new Point(startX + 50 * j + Padding, startY + 50 * i + Padding);
                    letterButton.Size = new Vector2(ButtonWidth, ButtonHeight);
                    letterButton.Text = "[" + currentLetter + "]";
                    Elements.Add(letterButton);
                }
            }

            // Tecla de espaço
            Button spaceButton = new Button(Game);
            Char space = ' ';
            spaceButton.OnClick += (sender, args) => playerName += space;
            spaceButton.Bounds = new Rectangle(0, 0, ButtonWidth, ButtonWidth);
            spaceButton.Location = new Point(startX + 50 * 6 + Padding, startY + 50 * 3 + Padding);
            spaceButton.Size = new Vector2(ButtonWidth, ButtonHeight);
            spaceButton.Text = "[" + space + "]";
            Elements.Add(spaceButton);

            // Tecla de Backspace
            Button bckButton = new Button(Game);
            Char back = '<';
            bckButton.OnClick += (sender, args) =>
            {
                if (playerName.Length > 0)
                {
                    playerName = playerName.Remove(playerName.Length - 1);
                }
            };
            bckButton.Bounds = new Rectangle(0, 0, ButtonWidth, ButtonWidth);
            bckButton.Location = new Point(startX + 50 * 7 + Padding, startY + 50 * 3 + Padding);
            bckButton.Size = new Vector2(ButtonWidth, ButtonHeight);
            bckButton.Text = "[" + back + "]";
            Elements.Add(bckButton);

        }

        public override void Update(GameTime gameTime)
        {
            var sceneManager = GetService<ISceneManagerService<MainGame.Scenes>>();
            var input = GetService<IInputService>();

            if (input.MouseClick(Game.Window.ClientBounds)) // se clicou dentro da tela, vá para o menu
            {
                //sceneManager.GoTo(MainGame.Scenes.Menu);
                //return;
            }

            base.Update(gameTime);
        }

        protected override void DrawBefore(GameTime gameTime)
        {
            Game.GraphicsDevice.Clear(Color.Brown);

            SpriteBatch.Begin();

            Vector2 size = font.MeasureString(playerName);
            var location = new Vector2(
                startX + (ButtonWidth - size.X) / 2,
                startY - 100 + (ButtonHeight - size.Y) / 2
            );

            SpriteBatch.Draw(BackgroundImage, Game.Window.ClientBounds, Color.White);
            SpriteBatch.DrawString(font, playerName, location, Color.White);
            SpriteBatch.End();
        }

        private Texture2D BackgroundImage { get; set; }
        private const int ButtonWidth = 50;
        private const int ButtonHeight = 50;
        private const int Padding = 5;
        private String playerName;
        private int startX;
        private int startY;
        private SpriteFont font;
    }
}
