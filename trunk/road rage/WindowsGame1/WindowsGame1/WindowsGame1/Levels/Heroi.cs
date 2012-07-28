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

namespace TangoGames.RoadFighter.Levels
{
    public class Heroi
    {
        private Game game;

        private Texture2D textura;
        private Texture2D botaoEsquerda;
        private Texture2D botaoDireita;

        private Rectangle retEsquerda;
        private Rectangle retDireita;

        private IList<Vector2> listadepistas;
        private int numeroPistas = 4;

        private Vector2 posicao;
        private int faixaAnterior = 1;
        private int faixaAtual = 1;

        private IInputService imput;

        public Heroi(Game game)
        {
            this.game = game;

            textura = game.Content.Load<Texture2D>("Textures/CarroHeroi");
            listadepistas = new List<Vector2>();
            for (int i = 0; i < numeroPistas; i++)
            {
                listadepistas.Add(new Vector2(200 + 100 * i, 300));
            }
            posicao = listadepistas.ElementAt(1);

            botaoEsquerda = game.Content.Load<Texture2D>("Widgets/botaoEsquerda");
            botaoDireita = game.Content.Load<Texture2D>("Widgets/botaoDireita");

            retEsquerda = new Rectangle(20, game.Window.ClientBounds.Height / 5 * 3, 120, 120);
            retDireita = new Rectangle(game.Window.ClientBounds.Width - 150, game.Window.ClientBounds.Height / 5 * 3, 120, 120);

            imput = (IInputService)game.Services.GetService(typeof(IInputService));
        }


        public void Update(GameTime gameTime)
        {
            controleHeroi();
            movimentaHeroi();
        }


        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(textura, new Rectangle((int)posicao.X, (int)posicao.Y, 72, 155), Color.White);

            spriteBatch.Draw(botaoEsquerda, retEsquerda, Color.White);
            spriteBatch.Draw(botaoDireita, retDireita, Color.White);
        }

        void controleHeroi()
        {
            if ((imput.KeyPressOnce(Keys.Left) || imput.MouseClick(retEsquerda)) && (faixaAtual > 0))
            {
                faixaAnterior = faixaAtual;
                faixaAtual--;
            }
            if ((imput.KeyPressOnce(Keys.Right) || imput.MouseClick(retDireita)) && (faixaAtual < numeroPistas - 1))
            {
                faixaAnterior = faixaAtual;
                faixaAtual++;
            }
        }


        void movimentaHeroi()
        {
            if ((faixaAnterior <= faixaAtual) && (posicao.X < listadepistas[faixaAtual].X))
            {
                posicao.X += 3;

                if (posicao.X + 3 >= listadepistas[faixaAtual].X)
                {
                    posicao.X = listadepistas[faixaAtual].X;
                }
            }
            if ((faixaAnterior >= faixaAtual) && (posicao.X > listadepistas[faixaAtual].X))
            {
                posicao.X -= 3;

                if (posicao.X - 3 <= listadepistas[faixaAtual].X)
                {
                    posicao.X = listadepistas[faixaAtual].X;
                }
            }
        }

    }
}
