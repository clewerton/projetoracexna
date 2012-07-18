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
    public class HUD
    {
        public int pontos = 0;     // total de pomntos do player
        public int gasolina = 100; // quantidade de gasolina do herói
        Texture2D indicadorCombustivel;  // imagem do indicador de gasolina
        Texture2D ponteiro;                // ponteiro do indicador de gasolina
        SpriteFont Arial;
        int contadordePontos = 0; // calcula o tempo decorrido para atribuição de pontos
        float tempodecorrido = 0;


        public HUD(ContentManager Content)
        {

            Arial = Content.Load<SpriteFont>("arial");


        }

        public void Update(GameTime gameTime)
            
        {

            calculagasolina(gameTime);
            calculapontos(gameTime);

            
        }

                    
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
            
        {
            
            //SpriteBatch.Begin();

            spriteBatch.DrawString(Arial, "Pontuação: " +pontos, new Vector2(10, 10), Color.Red);

            //SpriteBatch.End();
            
        }


        void calculagasolina(GameTime gametime) // calcula a quantidade de gasolina gasta
        {


        }

        void calculapontos(GameTime gametime) // calcula a quantidade de pontos ganhos
        {
            
            
            //if( velocidade herói == velmax)   // verifica se a velocidade do heroi está maxima e contabiliza os pontos

            tempodecorrido += (float)gametime.ElapsedGameTime.TotalMilliseconds;




            if (tempodecorrido >= 1000)
            {
                tempodecorrido = 0;
                contadordePontos += 1;
            }

            pontos = contadordePontos * 10;

            //if ((tempodecorrido % 1) == 0)
            //{
                //pontos += 10;
            //}
            //}

        }
        



    }
}