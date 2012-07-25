using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using TangoGames.RoadFighter.Scenes;
using Microsoft.Xna.Framework.Content;

namespace TangoGames.RoadFighter.Levels
{
    public class Heroi
    {
        Texture2D textura;

        IList<Vector2> listadepistas;

        Vector2 posicao;
        Vector2 faixa1;
        Vector2 faixa2;
        Vector2 faixa3;
        Vector2 faixa4;
        int faixaanterior = 1;
        int faixaatual = 1;
       

        public Heroi(ContentManager Content)
        {
            textura = Content.Load<Texture2D>("Textures/CarroHeroi");
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

            
        }


        public void Update(GameTime gameTime)
        {
            controleHEroi();
            movimentaHeroi();

        }


        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

            spriteBatch.Draw(textura,new Rectangle((int)posicao.X,(int)posicao.Y, 72, 155), Color.White);

        }

        void controleHEroi()
        {

            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                if (faixaatual > 0)
                {
                    faixaanterior = faixaatual;
                    faixaatual--;
                }
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
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
