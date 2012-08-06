﻿using System;
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
    struct HighscoreLine
    {
        public String name;
        public int value;

        public HighscoreLine(String name, int value)
        {
            this.name = name;
            this.value = value;
        }

    }

    class HighScore : Scene
    {
        public const int HIGHSCORE_NUMBER = 10;
        public HighScore(Game game) : base(game) 
        {
            characters = new String[5];
            characters[0] = "0123456789";
            characters[1] = "ABCDEFGHIJ";
            characters[2] = "KLMNOPQRST";
            characters[3] = "UVWXYZ";
            characters[4] = "!@#$%&*?";

            startX = game.Window.ClientBounds.Width / 4;
            startY = game.Window.ClientBounds.Height / 2;

            highScores = new List<HighscoreLine>(HIGHSCORE_NUMBER);
            // Setando os valores dos highscores
            for(int i = 0; i < HIGHSCORE_NUMBER; i++)
            {
                highScores.Add(new HighscoreLine("", 0));
            }
            // TODO: ler os highcores do arquivo (se houver)

        }

        protected override void LoadContent()
        {
            // configurando o fundo
            BackgroundImage = Game.Content.Load<Texture2D>("Textures/grass1920");
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
            spaceButton.Location = new Point(startX + 50 * 8 + Padding, startY + 50 * 4 + Padding);
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
            bckButton.Location = new Point(startX + 50 * 9 + Padding, startY + 50 * 4 + Padding);
            bckButton.Size = new Vector2(ButtonWidth, ButtonHeight);
            bckButton.Text = "[" + back + "]";
            Elements.Add(bckButton);

            Button ToFase = new Button(Game);
            ToFase.OnClick += (sender, args) =>
            {
                int index = HighScoreIndex(((MainGame)Game).HighScore);
                highScores[index] = new HighscoreLine(playerName, ((MainGame)Game).HighScore); ;

                GetService<ISceneManagerService<MainGame.Scenes>>().GoTo(MainGame.Scenes.Fase);
            };
            ToFase.Location = new Point((clientBounds.Width - ButtonWidth) / 2, 500 + (clientBounds.Height - ButtonHeight) / 2 - ButtonHeight - Padding);
            ToFase.Size = new Vector2(ButtonWidth, ButtonHeight);
            ToFase.Text = "Salvar recorde";
            ToFase.Texture = Game.Content.Load<Texture2D>("Widgets/Button");
            ToFase.Background = Color.White;
            Elements.Add(ToFase);

        }

        public override void Enter()
        {
            base.Enter();
            playerName = "";
        }

        public static bool insideHighscores(int score)
        {
            foreach (HighscoreLine item in highScores)
            {
                if (score > item.value)
                {
                    return true;
                }
            }
            return false;
        }

        private int HighScoreIndex(int score)
        {
            for(int i = 0; i < highScores.Count; i++)
            {
                if (score > highScores.ElementAt(i).value)
                {
                    return i;
                }
            }
            return highScores.Count + 1;
        }

        //public override void Enter()
        //{
        //}

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
            var location = new Vector2(Game.Window.ClientBounds.Width / 2 - ButtonWidth - 200, 100);

            SpriteBatch.Draw(BackgroundImage, Game.Window.ClientBounds, Color.White);
            SpriteBatch.DrawString(font, "[" + playerName + "]", location, Color.White);
            int i = 1;
            foreach (HighscoreLine item in highScores)
            {
                if (item.value > 0)
                {
                    SpriteBatch.DrawString(font, "Posicao " + i + ": " + item.name + " -> " + item.value, new Vector2(300 + Game.Window.ClientBounds.Width / 2 - ButtonWidth, i * 50), Color.White);
                }
                i++; ;
            }
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
        private static IList<HighscoreLine> highScores;
        private String[] characters;

    }
}
