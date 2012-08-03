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
    interface IHUD
    { 

    }

    public class HUD : DrawableGameComponent, IHUD
    {

        #region Variaveis Privadas

        private Scene scene;

        private IMap map;

        private int pontos = 0;                   // total de pontos em metros do player 

        private float gas = 0;                    //percentual de gasolina do heroi ( é o percentual de tempo decorrido)

        private float checkPointTimer = 90000;    //tempo em milisegundos de duração da gasolina (90000 = 1 min e 30 segundos)

        private float timerCount = 0;             //Contador do tempo decorrido em milisegundos

        private float pixelsCount = 0;            //Quantidade e pixels percorridos 

        private Vector2 speedMap;                 //Velocidade do atual do rolamento

        private float maxSpeed = 20.0F;           //Velocidade Máxima;

        private Single angponteiro1;
        private Single angponteiro2;

        private Texture2D indicadorCombustivel;  // imagem do indicador de gasolina
        private Texture2D marcadorPontos;
        private Texture2D velocimetro;
        private Texture2D ponteiro1;                // ponteiro do indicador de gasolina
        private Texture2D ponteiro2;                // ponteiro para o velocimetro
        private SpriteFont Arial;

        #endregion

        #region metodos Públicos

        public HUD(Scene scene, IMap map)
            : base(scene.Game)
        {
            this.scene = scene;

            this.map = map;

            Arial = scene.Game.Content.Load<SpriteFont>("arial");

            maxSpeed = map.MaxSpeedGlobal ;

            marcadorPontos = scene.Game.Content.Load<Texture2D>("HUDElementos/Pontos");
            indicadorCombustivel = scene.Game.Content.Load<Texture2D>("HUDElementos/gasolina2");
            ponteiro1 = scene.Game.Content.Load<Texture2D>("HUDElementos/ponteiro");
            velocimetro = scene.Game.Content.Load<Texture2D>("HUDElementos/velocimetro");
            ponteiro2 = scene.Game.Content.Load<Texture2D>("HUDElementos/ponteirovelocimetro");

        }

        /// <summary>
        /// Update da classe
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="velocidade"></param>
        public override void Update(GameTime gameTime)
        
        {   //atualiza a velocidade atual do rolamento
            speedMap = map.Velocity;

            //calcula paramentros e acumuladores
            calculaparametros(gameTime);

            //calcula o angulo do ponteiro no marcador da gasolina
            angponteiro1 = AnglePointer( gas , -82 );

            //calcula o angulo do ponteiro de velocidade
            angponteiro2 = AnglePointer( (speedMap.Y * 100) / maxSpeed , 230);

        }

        /// <summary>
        /// desenho da classe
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="spriteBatch"></param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
            
        {
            
            spriteBatch.DrawString(Arial, " " +pontos, new Vector2(10, 140), Color.Red);

            spriteBatch.Draw(marcadorPontos, new Rectangle(0, 100, 204, 53), new Rectangle(18, 10, 204, 53), Color.White);

            spriteBatch.Draw(indicadorCombustivel, new Rectangle(30, 195, indicadorCombustivel.Width, indicadorCombustivel.Height), new Rectangle(0, 0, indicadorCombustivel.Width, indicadorCombustivel.Height), Color.White);
            
            spriteBatch.Draw(ponteiro1, new Rectangle(65,305, 78, 6), null, Color.White, angponteiro1, new Vector2(0, ponteiro1.Height/2), SpriteEffects.None, 0);

            spriteBatch.Draw(velocimetro, new Rectangle(0, 200 + indicadorCombustivel.Height, velocimetro.Width, velocimetro.Height), Color.White);

            spriteBatch.Draw(ponteiro2, new Rectangle(velocimetro.Width / 2 - ponteiro2.Width/2, 200 + indicadorCombustivel.Height + ponteiro2.Height + 45, ponteiro2.Width, ponteiro2.Height),null, Color.White,angponteiro2,new Vector2(ponteiro2.Width/2,ponteiro2.Height), SpriteEffects.None, 0);
            
        }

        #endregion

        #region metodos privados

        /// <summary>
        /// Calcula angulo do ponteiro baseado no percentual 
        /// </summary>
        /// <param name="ratio">Percentual</param>
        /// <param name="adjust">Ajuste de angulo</param>
        /// <returns></returns>
        private float AnglePointer(float ratio, int adjust)
        {
            if (ratio<0) { ratio = 0; }

            if (ratio > 100) { ratio = 100; }

            float angle = (adjust * ratio) / 100;           // calcula o angulo do ponteiro em relacao ao percentual

            angle = angle - ( adjust / 2 );     // ajusta o valor do angulo em relacao ao desenho do ponteiro

            return calcularadianos(angle);                  // converte de graus para radianos 

        }

        /// <summary>
        /// função para converter graus em radianos
        /// </summary>
        /// <param name="angulo"></param>
        /// <returns></returns>
        private float calcularadianos(float angulo){

            angulo = (((float)Math.PI * angulo) / 180);

            return angulo;

        }
         
        /// <summary>
        /// faz o calculo da distancia percorrida e parametros gerais
        /// </summary>
        /// <param name="gametime"></param>
        private void calculaparametros(GameTime gametime) 
        {
            //milisegundo decorridos desde o último frame 
            //deveria ser (float)gametime.ElapsedGameTime.TotalMilliseconds mas teriamos que ajusta a velocidade de pixels de acordo com a performace de tempo de cada frame.
            //optamos em colocar 16 milisegundos fixos baseado em 60 FPS fixo para não afetar a contagem de pixel do calculo da distancia percorrida
            float ElapseTime = 16;

            //atualiza o tempo decorrido do checkPoint
            timerCount +=  ElapseTime;

            //acumula quantidade de pixels de deslocamento
            pixelsCount += speedMap.Y;

            //calcula percentual de gasolina
            gas = ( ( checkPointTimer - timerCount) * 100) / checkPointTimer;

            //atualiza contador de pontos ( razão 20 pixel por metros )
            pontos = (int) ( pixelsCount / map.RatioPxMt ); //pontos em metros

        }

        #endregion

    }

}