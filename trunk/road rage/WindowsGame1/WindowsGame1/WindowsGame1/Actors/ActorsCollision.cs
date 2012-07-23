﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TangoGames.RoadFighter.Actors
{
    public interface IActorsCollision
    {
        Boolean collided(BasicDrawingActor AtorColidente);
        Texture2D Texture { get; set; }
        //ActorsCollision CollisionComponent { get; }
    }

    public class ActorsCollision
    {
#region Declaracao de variaveis
        /// <summary>
        /// Ator Base é a instancia de ator base que usará a classe de colisão
        /// </summary>
        BasicDrawingActor AtorBase;

        /// <summary>
        /// Array bitmap da imagem de colisão do ator
        /// </summary>
        Color[] DadosTextura;
#endregion

        public ActorsCollision(BasicDrawingActor AtorBase)
        {
            // Preserva a instancia do ator base
            this.AtorBase = AtorBase;
            
            DadosTextura =  new Color[AtorBase.Texture.Width * AtorBase.Texture.Height];
            AtorBase.Texture.GetData(DadosTextura);
        }

        public Boolean CollisionTestRetangle(BasicDrawingActor AtorColidente)
        {  
           return false;
           //if (AtorBase.Retangulo
           //Rectangle RetanguloMoto1 =
//                new Rectangle((int)posicaoMoto1.X, (int)posicaoMoto1.Y,
//                texturaMoto.Width, texturaMoto.Height);

//            // Define o retangulo delimitador 2
//            Rectangle RetanguloMoto2 =
//                new Rectangle((int)posicaoMoto2.X, (int)posicaoMoto2.Y,
//                texturaMoto.Width, texturaMoto.Height);return false;
        }
//               // A imagem
//        Texture2D texturaMoto;
//        // Os dados de cores para as imagens, usado para colisão por pixel
//        Color[] dadosTexturaMoto1;////////////////////////
//        Color[] dadosTexturaMoto2;///////////////////////
//        // Posicao 
//        Vector2 posicaoMoto1;//////////
//        Vector2 posicaoMoto2;/////////
//        // Constante de velocidade 
//        const int velocidadeMoto = 2;
//// Variavel para quando uma colisão for detectada
//        bool motoAtingida = false;
//        public Game1()
//        {
//            graphics = new GraphicsDeviceManager(this);
//            Content.RootDirectory = "Content";
//        }
//        protected override void Initialize()
//        {
//            //Posicao inicial 
//            posicaoMoto1 = new Vector2(10, 300);
//            posicaoMoto2 = new Vector2(757, 300);
//            base.Initialize();
//        }

//        protected override void LoadContent()
//        {
//            spriteBatch = new SpriteBatch(GraphicsDevice);
//            // Carrega a textura 
//            texturaMoto = Content.Load<Texture2D>("moto");
//            // Extrai os dados para colisão 
//            dadosTexturaMoto1 =
//                new Color[texturaMoto.Width * texturaMoto.Height];
//            //guarda no array
//            texturaMoto.GetData(dadosTexturaMoto1);
                        
//            dadosTexturaMoto2 =
//                new Color[texturaMoto.Width * texturaMoto.Height];
//            texturaMoto.GetData(dadosTexturaMoto2);
//        }
//        protected override void UnloadContent()
//        {
            
//        }
//        protected override void Update(GameTime gameTime)
//        {
//            // Pegando o estado do teclado
//            KeyboardState teclado = Keyboard.GetState();
//            //1
//            #region
//            if (teclado.IsKeyDown(Keys.D))
//                posicaoMoto1.X += velocidadeMoto;
//            if (teclado.IsKeyDown(Keys.A))
//                posicaoMoto1.X -= velocidadeMoto;
//            if (teclado.IsKeyDown(Keys.W))
//                posicaoMoto1.Y -= velocidadeMoto;
//            if (teclado.IsKeyDown(Keys.S))
//                posicaoMoto1.Y += velocidadeMoto;
//            #endregion
//            //  2
//            #region
//            if (teclado.IsKeyDown(Keys.Right))
//                posicaoMoto2.X += velocidadeMoto;
//            if (teclado.IsKeyDown(Keys.Left))
//                posicaoMoto2.X -= velocidadeMoto;
//            if (teclado.IsKeyDown(Keys.Up))
//                posicaoMoto2.Y -= velocidadeMoto;
//            if (teclado.IsKeyDown(Keys.Down))
//                posicaoMoto2.Y += velocidadeMoto;
//            #endregion

//            // Define o retangulo delimitador  1
//            Rectangle RetanguloMoto1 =
//                new Rectangle((int)posicaoMoto1.X, (int)posicaoMoto1.Y,
//                texturaMoto.Width, texturaMoto.Height);

//            // Define o retangulo delimitador 2
//            Rectangle RetanguloMoto2 =
//                new Rectangle((int)posicaoMoto2.X, (int)posicaoMoto2.Y,
//                texturaMoto.Width, texturaMoto.Height);

//            motoAtingida = false;

//            if (ColisaoPorPixel(RetanguloMoto1, dadosTexturaMoto1, RetanguloMoto2, dadosTexturaMoto2))
//            {
//                motoAtingida = true;
//            }

//            if (motoAtingida)
//                Window.Title = "bateu";
//            else
//                Window.Title = "não bateu";

//            base.Update(gameTime);
//        }

//        protected override void Draw(GameTime gameTime)
//        {

//            GraphicsDevice.Clear(Color.Black);

//            spriteBatch.Begin();

//            // Desenhando 
//            spriteBatch.Draw(texturaMoto, posicaoMoto1, Color.White);

//            spriteBatch.Draw(texturaMoto, posicaoMoto2, Color.Red);

//            spriteBatch.End();
//            base.Draw(gameTime);
//        }

//        // Determina se há sobreposição dos pixels não-transparentes entre os dois retangulos
//        public bool ColisaoPorPixel(Rectangle retangulo1, Color[] dados1, Rectangle retangulo2, Color[] dados2)
//        {
//            // Encontra os limites do retangulo de interseção
//            int cima = Math.Max(retangulo1.Top, retangulo2.Top);
//            int baixo = Math.Min(retangulo1.Bottom, retangulo2.Bottom);
//            int esquerda = Math.Max(retangulo1.Left, retangulo2.Left);
//            int direita = Math.Min(retangulo1.Right, retangulo2.Right);

//            // Verifica todos os pontos dentro do limite de intereseção 
//            //TODO verificar se os retangulos estao um ao lado do outro
//            for (int y = cima; y < baixo; y++)
//            {
//                for (int x = esquerda; x < direita; x++)
//                {
//                    // Verifica a cor de ambos os pixels neste momento
//                    Color color1 = dados1[(x - retangulo1.Left) +
//                    (y - retangulo1.Top) * retangulo1.Width];
//                    Color color2 = dados2[(x - retangulo2.Left) +
//                    (y - retangulo2.Top) * retangulo2.Width];

//                    // Se ambos os píxels não são completamente diferentes
//                    if (color1.A != 0 && color2.A != 0)
//                    {
//                        // Um cruzamento de pixel foi encontrado
//                        return true;
//                    }
//                }
//            }
//            // Não foi encontrado cruzamento entre os pixels
//            return false;
//        }
//    }
  }
}
