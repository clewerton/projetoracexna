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

        Texture2D textura;
        Texture2D botaoEsquerda;
        Texture2D botaoDireita;

        Rectangle retEsquerda;
        Rectangle retDireita;

        IList<Vector2> listadepistas;

        Vector2 posicao;
        Vector2 faixa1;
        Vector2 faixa2;
        Vector2 faixa3;
        Vector2 faixa4;
        int faixaanterior = 1;
        int faixaatual = 1;

        IInputService imput;
        
        

        public Heroi(Game game)
        {
            this.game = game;

            textura = game.Content.Load<Texture2D>("Textures/CarroHeroi");
            posicao = new Vector2(300,300);
            faixa1 = new Vector2(200,300);
            faixa2 = new Vector2(300, 300);
            faixa3 = new Vector2(400, 300);
            faixa4 = new Vector2(500, 300);

            listadepistas = new List<Vector2>();
            listadepistas.Add(faixa1);
            listadepistas.Add(faixa2);
            listadepistas.Add(faixa3);
            listadepistas.Add(faixa4);

            botaoEsquerda = game.Content.Load<Texture2D>("Widgets/botaoEsquerda");
            botaoDireita = game.Content.Load<Texture2D>("Widgets/botaoDireita");

            retEsquerda = new Rectangle(20, game.Window.ClientBounds.Height / 5 * 3, 141, 139);
            retDireita = new Rectangle(game.Window.ClientBounds.Width - 150, game.Window.ClientBounds.Height / 5 * 3, 141, 139);

            imput = (IInputService) game.Services.GetService(typeof(IInputService));
        }


        public void Update(GameTime gameTime)
        {
            controleHEroi();
            movimentaHeroi();

        }


        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

            spriteBatch.Draw(textura,new Rectangle((int)posicao.X,(int)posicao.Y, 72, 155), Color.White);

            spriteBatch.Draw(botaoEsquerda, retEsquerda, Color.White);
            spriteBatch.Draw(botaoDireita, retDireita, Color.White);
        }

        void controleHEroi()
        {

            if (imput.KeyPressOnce(Keys.Left) || imput.MouseClick(retEsquerda))
            {
                if (faixaatual > 0)
                {
                    faixaanterior = faixaatual;
                    faixaatual--;
                }
            }
            if (imput.KeyPressOnce(Keys.Right) || imput.MouseClick(retDireita))
            {
                if (faixaatual < 3)
                {
                    faixaanterior = faixaatual;
                    faixaatual++;
                }
            }


        }


        void movimentaHeroi()
        {
            if (faixaanterior <= faixaatual)
            {

                if (posicao.X < listadepistas[faixaatual].X)
                {
                    posicao.X += 3;

                    if(posicao.X + 2 >= listadepistas[faixaatual].X){
                        posicao.X = listadepistas[faixaatual].X;
                    }
                    

                }
                
            }

            if (faixaanterior >= faixaatual)
            {

                if (posicao.X > listadepistas[faixaatual].X)
                {
                    posicao.X -= 3;

                    if (posicao.X + 2 <= listadepistas[faixaatual].X)
                    {
                        posicao.X = listadepistas[faixaatual].X;
                    }


                }
                
            }            
        }







    }
}
