using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using TangoGames.RoadFighter.Scenes;
using Microsoft.Xna.Framework.Content;
using TangoGames.RoadFighter.Input;
using TangoGames.RoadFighter.Actors;

namespace TangoGames.RoadFighter.Levels
{
    public class Heroi : BasicDrawingActor
    {
        private Game game;

        private Texture2D botaoEsquerda;
        private Texture2D botaoDireita;

        private Rectangle retEsquerda;
        private Rectangle retDireita;

        private IList<int> listadepistas;
        private int numeroPistas = 4;

        private int faixaAnterior = 1;
        private int faixaAtual = 1;

        private IInputService input;

        public Heroi(Game game, Vector2 dimensions, SpriteBatch spriteBatch)
            : base(game, dimensions, game.Content.Load<Texture2D>("Textures/CarroHeroi"))
        {
            this.game = game;

            listadepistas = new List<int>();
            for (int i = 0; i < numeroPistas; i++)
            {
                listadepistas.Add(200 + 100 * i);
            }
            Move(new Vector2(listadepistas.ElementAt(1), 0));

            botaoEsquerda = game.Content.Load<Texture2D>("Widgets/botaoEsquerda");
            botaoDireita = game.Content.Load<Texture2D>("Widgets/botaoDireita");

            retEsquerda = new Rectangle(20, game.Window.ClientBounds.Height / 5 * 3, 120, 120);
            retDireita = new Rectangle(game.Window.ClientBounds.Width - 150, game.Window.ClientBounds.Height / 5 * 3, 120, 120);

            input = (IInputService)game.Services.GetService(typeof(IInputService));
        }


        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            controleHeroi();
            movimentaHeroi();
        }


        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Draw(Texture, new Rectangle((int)Location.X, (int)Location.Y, Bounds.Width, Bounds.Height), Color.White);

            SpriteBatch.Draw(botaoEsquerda, retEsquerda, Color.White);
            SpriteBatch.Draw(botaoDireita, retDireita, Color.White);
        }

        private void controleHeroi()
        {
            if ((input.KeyPressOnce(Keys.Left) || input.MouseClick(retEsquerda)) && (faixaAtual > 0))
            {
                faixaAnterior = faixaAtual;
                faixaAtual--;
            }
            if ((input.KeyPressOnce(Keys.Right) || input.MouseClick(retDireita)) && (faixaAtual < numeroPistas - 1))
            {
                faixaAnterior = faixaAtual;
                faixaAtual++;
            }
        }


        private void movimentaHeroi()
        {
            if ((faixaAnterior <= faixaAtual) && (Location.X < listadepistas[faixaAtual]))
            {
                Move(new Vector2(3, 0));

                if (Location.X > listadepistas[faixaAtual])
                {
                    Location = new Vector2(listadepistas[faixaAtual], Location.Y);
                }
            }
            if ((faixaAnterior >= faixaAtual) && (Location.X > listadepistas[faixaAtual]))
            {
                Move(new Vector2(-3, 0));

                if (Location.X < listadepistas[faixaAtual])
                {
                    Location = new Vector2(listadepistas[faixaAtual], Location.Y);
                }
            }
        }

    }
}
