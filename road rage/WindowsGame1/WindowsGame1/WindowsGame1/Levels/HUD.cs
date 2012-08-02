using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using TangoGames.RoadFighter.Scenes;
using Microsoft.Xna.Framework.Content;
using TangoGames.RoadFighter.Actors;


namespace TangoGames.RoadFighter.Levels
{
    public class HUD
    {
        public int pontos = 0;     // total de pomntos do player
        public float gasolina = 100; // quantidade de gasolina do herói
        float angulacaoGasolina = 82;
        float angulacaoVelocidade = -115;

        int pixel;
       
        Single angponteiro1;
        Single angponteiro2;

        float velocidadeatual;
        int velocidademaxima;

        Texture2D indicadorCombustivel;  // imagem do indicador de gasolina
        Texture2D marcadorPontos;
        Texture2D velocimetro;
        Texture2D ponteiro1;                // ponteiro do indicador de gasolina
        Texture2D ponteiro2; // ponteiro para o velocimetro
        SpriteFont Arial;
        int contadordePontos = 0; // calcula o tempo decorrido para atribuição de pontos
        float tempodecorrido = 0;


        public HUD(ContentManager Content, IMap map)
        {

            Arial = Content.Load<SpriteFont>("arial");

            velocidademaxima = map.MaxSpeed;
                        
            marcadorPontos = Content.Load<Texture2D>("HUDElementos/Pontos");
            indicadorCombustivel = Content.Load<Texture2D>("HUDElementos/gasolina2");
            ponteiro1 = Content.Load<Texture2D>("HUDElementos/ponteiro");
            velocimetro = Content.Load<Texture2D>("HUDElementos/velocimetro");
            ponteiro2 = Content.Load<Texture2D>("HUDElementos/ponteirovelocimetro");
        }

        public void Update(GameTime gameTime, Vector2 velocidade)
            
        {
            velocidadeatual = velocidade.Y;

            calculaparametros(gameTime);

            ponteirogasolina(gameTime);
            ponteirovelocidade(gameTime);
        }

                    
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
            
        {
            
            spriteBatch.DrawString(Arial, " " +pontos, new Vector2(10, 140), Color.Red);

            spriteBatch.Draw(marcadorPontos, new Rectangle(0, 100, 204, 53), new Rectangle(18, 10, 204, 53), Color.White);

            spriteBatch.Draw(indicadorCombustivel, new Rectangle(30, 195, indicadorCombustivel.Width, indicadorCombustivel.Height), new Rectangle(0, 0, indicadorCombustivel.Width, indicadorCombustivel.Height), Color.White);
            //spriteBatch.Draw(indicadorCombustivel, new Rectangle(30, 190, 171, 233), new Rectangle(18, 10, 171, 205), Color.White);

            spriteBatch.Draw(ponteiro1, new Rectangle(65,305, 78, 6), null, Color.White, angponteiro1, new Vector2(0, ponteiro1.Height/2), SpriteEffects.None, 0);

            spriteBatch.Draw(velocimetro, new Rectangle(0, 200 + indicadorCombustivel.Height, velocimetro.Width, velocimetro.Height), Color.White);
            spriteBatch.Draw(ponteiro2, new Rectangle(velocimetro.Width / 2 - ponteiro2.Width/2, 200 + indicadorCombustivel.Height + ponteiro2.Height + 45, ponteiro2.Width, ponteiro2.Height),null, Color.White,angponteiro2,new Vector2(ponteiro2.Width/2,ponteiro2.Height), SpriteEffects.None, 0);
            
        }



        void ponteirogasolina(GameTime gametime) // calcula a angulacao do ponteiro
        {
            

            angulacaoGasolina = (-82 * gasolina) / 100; // calcula o angulo do ponteiro em relacao a quantidade de gasolina

           

            angulacaoGasolina = angulacaoGasolina + 41; // ajusta o valor do angulo em relacao ao desenho do ponteiro

            angponteiro1 = calcularadianos(angulacaoGasolina);

            //radianos = (((float)Math.PI * angulacaoGasolina) / 180);       // converte de graus para radianos 


        }


        void ponteirovelocidade(GameTime gametime)
        {
           // angponteiro2 = calcularadianos(angulacaoVelocidade);
           // (velatual * 240) / velmax = x;

            angulacaoVelocidade = (velocidadeatual * 230) / velocidademaxima;

            angulacaoVelocidade = angulacaoVelocidade - 115;

            angponteiro2 = calcularadianos(angulacaoVelocidade);


            

        }


        float calcularadianos(float angulo){

            angulo = (((float)Math.PI * angulo) / 180);


            return angulo;
        }
        
       
        void calculaparametros(GameTime gametime) // calcula a quantidade de pontos ganhos
        {
                        
            //if( velocidade herói == velmax)   // verifica se a velocidade do heroi está maxima e contabiliza os pontos

            tempodecorrido += (float)gametime.ElapsedGameTime.TotalMilliseconds;


            //pontos += (int)velocidadeatual;

            pixel += (int)velocidadeatual;
           

            if (tempodecorrido >= 1000)
            {
                tempodecorrido = 0;
                contadordePontos = pixel / 20;
                //contadordePontos += (int)velocidadeatual;
                pontos += contadordePontos;

                gasolina --;

                pixel = 0;
            }

            //pontos = contadordePontos * 10;
            


            
            if (gasolina <= 0)
            {

                gasolina = 0;
            }

        }
        



    }
}