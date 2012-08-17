using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace dev173d
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        
        SpriteBatch spriteBatch;

        List<BasicPrimitive> ListObjects;
        
        BasicCamera camera;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            ListObjects = new List<BasicPrimitive>() ;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //cria objeto vermelho no plano frontal
            CreateObject(new Vector3(0, 0, -3.0f)  , new Vector3 (0.0f, 0.0f, 0.0f)  , Color.Red );

            //cria objeto verde no plano lateral direito
            CreateObject(new Vector3(3.0f, 0, 0.0f), new Vector3(0.0f, -90.0f, 0.0f) , Color.Green);

            //cria objeto verde no plano lateral esquerdo
            CreateObject(new Vector3(-3.0f, 0, 0.0f), new Vector3(0.0f, 90.0f, 0.0f), Color.Blue );

            //cria objeto preto no plano acima
            CreateObject(new Vector3(0.0f, 3.0f, 0.0f), new Vector3(90.0f, 0.0f, 0.0f), Color.Black);

            //cria objeto branco no plano abaixo
            CreateObject(new Vector3(0.0f, -3.0f, 0.0f), new Vector3(-90.0f, 0.0f, 0.0f), Color.White);

            //cria objeto amarelo no plano de atras
            CreateObject(new Vector3(0.0f, 0.0f, 3.0f), new Vector3(0.0f, 180.0f, 0.0f), Color.Yellow );

            camera = new BasicCamera(GraphicsDevice);

            //camera posicionada no centro do cubo 6X6X6
            camera.Position = new Vector3(0.0f, 0.0f, 0.0f);
            //olhado para parede frontal
            camera.Target = new Vector3(0.0f, 0.0f, -3.0f);

        }

        private void CreateObject(Vector3 position, Vector3 rotation, Color color ) 
        {
            BasicPrimitive obj = new BasicPrimitive(GraphicsDevice, Content, color);

            obj.MoveAndRotation ( position, rotation);

            ListObjects.Add(obj);

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            KeyboardState keyboardstate = Keyboard.GetState();

            #region Troca alvo da camera

            //vira camera para o lado direito
            if (keyboardstate.IsKeyDown(Keys.Delete) )
            {
                camera.Target = new Vector3(3.0f, 0.0f, 0.0f);
                if (keyboardstate.IsKeyDown(Keys.RightShift) || keyboardstate.IsKeyDown(Keys.LeftShift )) 
                {
                    camera.Position += new Vector3(0.01f, 0, 0);
                }
            }
            //vira camera para o lado esquerdo
            if (keyboardstate.IsKeyDown(Keys.Insert  ))
            {
                camera.Target = new Vector3(-3.0f, 0.0f, 0.0f);
                if (keyboardstate.IsKeyDown(Keys.RightShift) || keyboardstate.IsKeyDown(Keys.LeftShift))
                {
                    camera.Position += new Vector3(-0.01f, 0, 0);
                }
            }
            //vira camera para cima
            if (keyboardstate.IsKeyDown(Keys.PageUp ))
            {
                // NÃO SEI PORQUE NÃO ACEITA Z = 0 PARA VER O OBJETO PRETO EM CIMA NO TARGET ????
                // A POSIÇÃO DA CAMERA É 0,0,0. BASTARIA VIRAR PARA CIMA COM Y = 3 ( 0,3,0 )
                // TEM QUE COLOCAR UM Z DIFERENTE DE ZERO PARA APARECER O OBJETO
                camera.Target = new Vector3(0.0f, 3.0f, 0.000001f);
                if (keyboardstate.IsKeyDown(Keys.RightShift) || keyboardstate.IsKeyDown(Keys.LeftShift))
                {
                    camera.Position += new Vector3(0,0.01f, 0);
                }

            }

            //vira camera para cima
            if (keyboardstate.IsKeyDown(Keys.PageDown))
            {
                // NÃO SEI PORQUE NÃO ACEITA Z = 0 PARA VER O OBJETO BRANCO A BAIXO NO TARGET ????
                // A POSIÇÃO DA CAMERA É 0,0,0. BASTARIA VIRAR PARA BAIXO COM Y = -3 ( 0,-3, 0 )
                // TEM QUE COLOCAR UM Z DIFERENTE DE ZERO PARA APARECER O OBJETO
                camera.Target = new Vector3(0.0f, -3.0f, 0.000001f);
                if (keyboardstate.IsKeyDown(Keys.RightShift) || keyboardstate.IsKeyDown(Keys.LeftShift))
                {
                    camera.Position += new Vector3(0, -0.01f, 0);
                }

            }

            //olha para frente
            if (keyboardstate.IsKeyDown(Keys.Home ))
            {
                camera.Target = new Vector3(0.0f, 0.0f, -3.0f);
                if (keyboardstate.IsKeyDown(Keys.RightShift) || keyboardstate.IsKeyDown(Keys.LeftShift))
                {
                    camera.Position += new Vector3(0, 0, -0.01f);
                }

            }

            //olha para atrás
            if (keyboardstate.IsKeyDown(Keys.End))
            {
                camera.Target = new Vector3(0.0f, 0.0f, 3.0f);
                if (keyboardstate.IsKeyDown(Keys.RightShift) || keyboardstate.IsKeyDown(Keys.LeftShift))
                {
                    camera.Position += new Vector3(0, 0, 0.01f);
                }

            }

            //Control volta a posicão original
            if (keyboardstate.IsKeyDown(Keys.LeftControl) || keyboardstate.IsKeyDown(Keys.RightControl ))
            {
                //camera posicionada no centro do cubo 6X6X6
                camera.Position = new Vector3(0.0f, 0.0f, 0.0f);
            }

            #endregion

            #region Gira Objecto Vermelho

            //gira o objeto vermelho para esquerda 1 grau
            if (keyboardstate.IsKeyDown(Keys.A))
            {
                ListObjects[0].Rotation += new Vector3(0, 1, 0);
            }
            //gira o objeto vermelho para direita 1 grau
            if (keyboardstate.IsKeyDown(Keys.D))
            {
                ListObjects[0].Rotation += new Vector3(0, -1, 0);
            }
            //gira o objeto vermelho para cima  grau
            if (keyboardstate.IsKeyDown(Keys.W))
            {
                ListObjects[0].Rotation += new Vector3(1, 0, 0);
            }
            //gira o objeto vermelho para baixo 1 grau
            if (keyboardstate.IsKeyDown(Keys.S))
            {
                ListObjects[0].Rotation += new Vector3(-1, 0, 0);
            }
            //gira o objeto vermelho para cima  grau
            if (keyboardstate.IsKeyDown(Keys.Q ))
            {
                ListObjects[0].Rotation += new Vector3(0, 0, 1);
            }
            //gira o objeto vermelho para baixo 1 grau
            if (keyboardstate.IsKeyDown(Keys.E))
            {
                ListObjects[0].Rotation += new Vector3(0, 0, -1);
            }

            #endregion

            //atualiza camera
            camera.Update(gameTime);

            foreach (var obj in ListObjects) { obj.Update(gameTime); }
                
            base.Update(gameTime);

        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            foreach (var obj in ListObjects) { obj.Draw(gameTime, camera); }
            
            base.Draw(gameTime);
        }
    }
}
