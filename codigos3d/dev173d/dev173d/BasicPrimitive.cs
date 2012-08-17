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
    /// Classe para criar um triângulo colorido
    /// </summary>
    class BasicPrimitive
    {
        /// <summary>
        /// Um array de vértices com posição e cor
        /// </summary>
        VertexPositionColorTexture[] primitives;

        /// <summary>
        /// Um efeito básico para desenho3d
        /// </summary>
        BasicEffect basicEffect;

        /// <summary>
        /// A placa de vídeo
        /// </summary>
        GraphicsDevice graphicsDevice;

        /// <summary>
        /// Matriz de "mundo" - posicionamento, escala e rotação... sempre atualizado
        /// </summary>
        public Matrix World = Matrix.Identity;

        /// <summary>
        /// Posição do objeto em relação a origem
        /// </summary>
        private Vector3 position = Vector3.Zero;

        /// <summary>
        /// Rotação do objeto em relação a origem;
        /// </summary>
        private Vector3 rotation = Vector3.Zero;

        //Vector2 posicaoTextura;
        //Texture2D textura;
        
        /// <summary>
        /// Construtor de um triângulo com cores de vértice fixas
        /// </summary>
        /// <param name="graphicsDevice">A refência da placa de vídeo</param>
        public BasicPrimitive(GraphicsDevice graphicsDevice, ContentManager Content, Color cor)
        {
            this.graphicsDevice = graphicsDevice;

            //Criar os vértices do triangulo:
            primitives = new VertexPositionColorTexture[4];

            primitives[0] = new VertexPositionColorTexture();
            primitives[0].Position = new Vector3(0, 1.0f, 0);
            primitives[0].Color = cor;

            primitives[1] = new VertexPositionColorTexture();
            primitives[1].Position = new Vector3(1.0f, 0, 0);
            primitives[1].Color = cor;

            primitives[2] = new VertexPositionColorTexture();
            primitives[2].Position = new Vector3(-1.0f, 0, 0);
            primitives[2].Color = cor;

            primitives[3] = new VertexPositionColorTexture();
            primitives[3].Position = new Vector3(0, -1.0f, 0);
            primitives[3].Color = cor;

            //Criar novo efeito básico e propriedades:
            basicEffect = new BasicEffect(graphicsDevice);

            //ativando as cores
            basicEffect.VertexColorEnabled = true;

            //basicEffect.TextureEnabled = true;
            //basicEffect.Texture = Content.Load<Texture2D>("GameThumbnail");

            //primitives[0].TextureCoordinate = new Vector2(0, 0);
            //primitives[1].TextureCoordinate = new Vector2(1, 0);
            //primitives[2].TextureCoordinate = new Vector2(1, 1);
            //primitives[3].TextureCoordinate = new Vector2(0, 1);

        }

        /// <summary>
        /// Método de atualização
        /// </summary>
        /// <param name="gameTime">Tempo do jogo</param>
        public void Update(GameTime gameTime)
        {
            
        }

        /// <summary>
        /// Método para desenho
        /// </summary>
        /// <param name="gameTime">Tempo do jogo</param>
        public void Draw(GameTime gameTime, BasicCamera camera)
        {
            //passando o efeito básico
            basicEffect.CurrentTechnique.Passes[0].Apply();

            basicEffect.View = camera.ViewMatrix;
            basicEffect.Projection = camera.ProjectionMatrix;
            
            basicEffect.World = this.World;

            //desenhando as primitivas
            graphicsDevice.DrawUserPrimitives<VertexPositionColorTexture>(
                PrimitiveType.TriangleStrip,//tipo da primitiva (linha ou triangulo)
                primitives,//o conjunto de vértices 
                0,//offset
                2);//contador de primitivas

        }

        public void MoveAndRotation( Vector3 position, Vector3 rotation)
        {
            this.position = position;
            this.rotation = rotation;
            MoveAndRotation();
        }

        private void MoveAndRotation()
        {
            Matrix result = Matrix.Identity;
            //a ordem das operações influi no resultado
            //Matrix result = Matrix.CreateTranslation(position);
            if (rotation.X != 0.0f) { result *= Matrix.CreateRotationX(MathHelper.ToRadians(rotation.X)); }
            if (rotation.Y != 0.0f) { result *= Matrix.CreateRotationY(MathHelper.ToRadians(rotation.Y)); }
            if (rotation.Z != 0.0f) { result *= Matrix.CreateRotationZ(MathHelper.ToRadians(rotation.Z)); }
            result *= Matrix.CreateTranslation(position);

            World = result;
        }


        public Vector3 Position 
        {
            get { return position;}
            set 
            { 
                this.position = value;
                MoveAndRotation();
            } 
        }

        public Vector3 Rotation
        {
            get { return rotation; }
            set
            {
                this.rotation = value;
                MoveAndRotation();
            }
        }
    }
}

/*
 * So lets get back to CullingMode

CullMode specifies how back-facing triangles are culled, if at all.The default value is CullMode.CounterClockwise
When drawing sprites, SpriteBatch.Begin does not save your current render state, and will change certain render state properties that may make 3D objects render incorrectly. This includes setting CullMode to CullMode.CullCounterClockwiseFace. You can choose to either reset the render state yourself after the call to SpriteBatch.End, or call SpriteBatch.Begin and pass in SaveStateMode.SaveState, which will restore the render state after sprites are drawn. (MSDN Remark on RenderState.CullMode Property)
CullMode takes 3 values.As we talked about earlier.Lets see what they are:

    CullClockwiseFace

    CullCounterClockwiseFace

    None


What they do?

CullClockwiseFace:

Cull back faces with clockwise vertices.

CullCounterClockwiseFace:

Cull back faces with counterclockwise vertices.

None:

Do not cull back faces.

Sample Usage:

RasterizerState stat = newRasterizerState();
stat.CullMode = CullMode.CullClockwiseFace;


 */
