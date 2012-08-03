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
        private IMap map;

        public int pontos = 0;      // total de pomntos do player

        public float gasolina = 0;               //percentual de gasolina do heroi ( é o percentual de tempo decorrido)

        public float checkPointTimer = 90000;    //tempo em milisegundos de duração da gasolina (90000 = 1 min e 30 segundos)

        public float timerCount = 0;             //Contador do tempo decorrido em milisegundos

        private int[] marks = new int[] { 3000, 2000, 1500, 1000, 500, 0 };

        private int nextmark = 0;

        private int checkPoint = 3000;

        private int distance = 0;

        float angulacaoVelocidade = -115;

        int pixel = 0;
       
        Single angponteiro1;
        Single angponteiro2;

        float velocidadeatual;
        int velocidademaxima;

        Texture2D indicadorCombustivel;  // imagem do indicador de gasolina
        Texture2D marcadorPontos;
        Texture2D velocimetro;
        Texture2D ponteiro1;                // ponteiro do indicador de gasolina
        Texture2D ponteiro2;                // ponteiro para o velocimetro
        SpriteFont Arial;
        int contadordePontos = 0;           // calcula o tempo decorrido para atribuição de pontos
        float tempodecorrido = 0;


        public HUD(ContentManager Content, IMap map)
        {

            this.map = map;

            Arial = Content.Load<SpriteFont>("arial");

            velocidademaxima = map.MaxSpeed;
                        
            marcadorPontos = Content.Load<Texture2D>("HUDElementos/Pontos");
            indicadorCombustivel = Content.Load<Texture2D>("HUDElementos/gasolina2");
            ponteiro1 = Content.Load<Texture2D>("HUDElementos/ponteiro");
            velocimetro = Content.Load<Texture2D>("HUDElementos/velocimetro");
            ponteiro2 = Content.Load<Texture2D>("HUDElementos/ponteirovelocimetro");
        }


        /// <summary>
        /// Update da classe
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="velocidade"></param>
        public void Update(GameTime gameTime, Vector2 velocidade)
            
        {
            velocidadeatual = velocidade.Y;

            calculaparametros(gameTime);

            //calcula o angulo do ponteiro no marcador da gasolina
            angponteiro1 = ponteirogasolina( gasolina );
    

            ponteirovelocidade();

        }

                    
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
            
        {
            
            spriteBatch.DrawString(Arial, " " +pontos, new Vector2(10, 140), Color.Red);

            spriteBatch.Draw(marcadorPontos, new Rectangle(0, 100, 204, 53), new Rectangle(18, 10, 204, 53), Color.White);

            spriteBatch.Draw(indicadorCombustivel, new Rectangle(30, 195, indicadorCombustivel.Width, indicadorCombustivel.Height), new Rectangle(0, 0, indicadorCombustivel.Width, indicadorCombustivel.Height), Color.White);
            
            spriteBatch.Draw(ponteiro1, new Rectangle(65,305, 78, 6), null, Color.White, angponteiro1, new Vector2(0, ponteiro1.Height/2), SpriteEffects.None, 0);

            spriteBatch.Draw(velocimetro, new Rectangle(0, 200 + indicadorCombustivel.Height, velocimetro.Width, velocimetro.Height), Color.White);
            spriteBatch.Draw(ponteiro2, new Rectangle(velocimetro.Width / 2 - ponteiro2.Width/2, 200 + indicadorCombustivel.Height + ponteiro2.Height + 45, ponteiro2.Width, ponteiro2.Height),null, Color.White,angponteiro2,new Vector2(ponteiro2.Width/2,ponteiro2.Height), SpriteEffects.None, 0);
            
        }

        /// <summary>
        /// Função controladora do comportamento do medidor de gasolina
        /// </summary>
        private float ponteirogasolina(float percentual) // calcula a angulacao do ponteiro
        {

            float angulacaoGasolina = (-82 * percentual) / 100; // calcula o angulo do ponteiro em relacao a quantidade de gasolina

            angulacaoGasolina = angulacaoGasolina + 41; // ajusta o valor do angulo em relacao ao desenho do ponteiro

            return calcularadianos(angulacaoGasolina);// converte de graus para radianos 

        }

        /// <summary>
        /// Função controladora do comportamento do velocimetro
        /// </summary>
        void ponteirovelocidade()
        {
           
            angulacaoVelocidade = (velocidadeatual * 230) / 20; // regra de 3 pa calcular a angulação do ponteiro em relação a velocidade maxima

            angulacaoVelocidade = angulacaoVelocidade - 115; // faz o ajuste em relação ao angulo máximo

            angponteiro2 = calcularadianos(angulacaoVelocidade);

        }

        /// <summary>
        /// função para converter graus em radianos
        /// </summary>
        /// <param name="angulo"></param>
        /// <returns></returns>
        float calcularadianos(float angulo){

            angulo = (((float)Math.PI * angulo) / 180);

            return angulo;

        }
        
       
        /// <summary>
        /// faz o calculo da distancia percorrida e parametros gerais
        /// </summary>
        /// <param name="gametime"></param>
        void calculaparametros(GameTime gametime) 
        {
            //milisegundo decorridos desde o último frame            
            float ElapseTime = (float)gametime.ElapsedGameTime.TotalMilliseconds;

            //atualiza o tempo decorrido do checkPoint
            timerCount +=  ElapseTime;

            gasolina = ( ( checkPointTimer - timerCount) * 100) / checkPointTimer;

            tempodecorrido += ElapseTime;
                       
            pixel += (int)velocidadeatual; //adiciona por um segundo todos os pixels percorridos
           
            if (tempodecorrido >= 1000) // if ocorrido a cada segundo
            {
                tempodecorrido = 0;
                contadordePontos = pixel / 20; // converte pixel para metros aproximados
                
                pontos += contadordePontos;   // aumenta os metros percorridos por segundo no contador total

                //gasolina --;

                pixel = 0;
            }

            
            
            if (gasolina <= 0)
            {

                gasolina = 0;
            }


            //calculo da distancia para o check point
            distance = checkPoint - pontos;
            if ( nextmark < marks.Count() &&  marks[nextmark] > distance) 
            {
                map.FlagSign(marks[nextmark]);
                nextmark++;
            }
        }
        



    }
}