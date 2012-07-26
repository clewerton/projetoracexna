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
        public float gasolina = 100; // quantidade de gasolina do herói
        float angulacaoGasolina = 90;
        Single radianos;
       

        Texture2D indicadorCombustivel;  // imagem do indicador de gasolina
        Texture2D ponteiro;                // ponteiro do indicador de gasolina
        Texture2D marcadorPontos;
        SpriteFont Arial;
        int contadordePontos = 0; // calcula o tempo decorrido para atribuição de pontos
        float tempodecorrido = 0;


        public HUD(ContentManager Content)
        {

            Arial = Content.Load<SpriteFont>("quartz");
            
                        
            marcadorPontos = Content.Load<Texture2D>("HUDElementos/Pontos");
            indicadorCombustivel = Content.Load<Texture2D>("HUDElementos/gasolina");
            ponteiro = Content.Load<Texture2D>("HUDElementos/ponteiro");
        }

        public void Update(GameTime gameTime)
            
        {

            calculaparametros(gameTime);

            ponteirogasolina(gameTime);
        }

                    
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
            
        {
            
            spriteBatch.DrawString(Arial, " " +pontos, new Vector2(10, 40), Color.Red);
            
            spriteBatch.Draw(marcadorPontos, new Rectangle(0, 10, 204, 53),new Rectangle(18,10,204,53) , Color.White);
            spriteBatch.Draw(indicadorCombustivel, new Rectangle(0, 90, 171, 233), new Rectangle(18, 10, 171, 205), Color.White);
            spriteBatch.Draw(ponteiro, new Rectangle(35,205, 78, 6), null, Color.White, radianos, new Vector2(0, ponteiro.Height/2), SpriteEffects.None, 0);

            
        }



        void ponteirogasolina(GameTime gametime) // calcula a angulacao do ponteiro
        {
            

            angulacaoGasolina = (-90 * gasolina) / 100; // calcula o angulo do ponteiro em relacao a quantidade de gasolina

           

            angulacaoGasolina = angulacaoGasolina + 45; // ajusta o valor do angulo em relacao ao desenho do ponteiro

            

            radianos = (((float)Math.PI * angulacaoGasolina) / 180);       // converte de graus para radianos 


        }



        void calculaparametros(GameTime gametime) // calcula a quantidade de pontos ganhos
        {
            
            
            //if( velocidade herói == velmax)   // verifica se a velocidade do heroi está maxima e contabiliza os pontos

            tempodecorrido += (float)gametime.ElapsedGameTime.TotalMilliseconds;




            if (tempodecorrido >= 1000)
            {
                tempodecorrido = 0;
                contadordePontos += 1;

                gasolina --;
            }

            pontos = contadordePontos * 10;

            
            if (gasolina <= 0)
            {

                gasolina = 0;
            }

        }
        



    }
}