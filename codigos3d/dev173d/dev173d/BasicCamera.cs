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
    /// Classe para criar câmeras básicas
    /// </summary>
    class BasicCamera
    {
        Vector3 position;
        /// <summary>
        /// Representa a posição da câmera, onde ela está.
        /// </summary>
        public Vector3 Position { get { return position; } set { position = value; } }

        Vector3 target;
        /// <summary>
        /// Representa o alvo da câmera, para onde ela aponta.
        /// </summary>
        public Vector3 Target { get { return target; } set { target = value; } }

        Vector3 orientation;
        /// <summary>
        /// Representa a orientação da câmera, qual o seu "cima".
        /// </summary>
        public Vector3 Orientation { get { return orientation; } set { orientation = value; } }

        float apertureAngle;
        /// <summary>
        /// Representa o ângulo de abertura da "lente" da câmera.
        /// </summary>
        public float ApertureAngle { get { return apertureAngle; } set { apertureAngle = value; } }

        float screenRatio;
        /// <summary>
        /// Representa a razão / proporção da tela / monitor (4/3, 16/9...).
        /// </summary>
        public float ScreenRatio { get { return screenRatio; } set { screenRatio = value; } }

        float nearPlane;
        /// <summary>
        /// Representa o plano de corte de proximidade, mais perto que isso a câmera não mostra
        /// </summary>
        public float NearPlane { get { return nearPlane; } set { nearPlane = value; } }

        float farPlane;
        /// <summary>
        /// Representa o plano de corte de distância, mais longe que isso a câmera não mostra
        /// </summary>
        public float FarPlane { get { return farPlane; } set { farPlane = value; } }

        Matrix viewMatrix;
        /// <summary>
        /// Representa a matriz de visualização da câmera
        /// </summary>
        public Matrix ViewMatrix { get { return viewMatrix; } set { viewMatrix = value; } }

        Matrix projectionMatrix;
        /// <summary>
        /// Representa a matriz de projeção da câmera
        /// </summary>
        public Matrix ProjectionMatrix { get { return projectionMatrix; } set { projectionMatrix = value; } }

        /// <summary>
        /// Lista de câmeras
        /// </summary>
        public static List<BasicCamera> listCameras = new List<BasicCamera>();

        /// <summary>
        /// Construtor padrão com valores padrões
        /// </summary>
        public BasicCamera(GraphicsDevice graphicsDevice)
        {
            Position = new Vector3(0, 0, 3);
            Target = Vector3.Zero;
            Orientation = Vector3.Up;

            ApertureAngle = 45.0f;
            ScreenRatio = graphicsDevice.Viewport.AspectRatio;
            NearPlane = 1.0f;
            FarPlane = 100.0f;
            
            ViewMatrix = Matrix.CreateLookAt(
                Position,
                Target,
                Orientation);

            ProjectionMatrix = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.ToRadians(ApertureAngle),
                ScreenRatio,
                NearPlane,
                FarPlane);
        }

        public void Update(GameTime gameTime)
        {

            #region a câmera tem que ser sempre atualizada

            ViewMatrix = Matrix.CreateLookAt(
                Position,
                Target,
                Orientation);

            ProjectionMatrix = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.ToRadians(ApertureAngle),
                ScreenRatio,
                NearPlane,
                FarPlane);

            #endregion

        }
    }
}
