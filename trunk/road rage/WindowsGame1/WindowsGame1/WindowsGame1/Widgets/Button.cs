using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TangoGames.RoadFighter.Scenes;
using TangoGames.RoadFighter.States;

namespace TangoGames.RoadFighter.Widgets
{
    public class Button : AbstractDrawableSceneElement
    {
        /// <summary>
        /// Registra eventos de clique.
        /// </summary>
        public event EventHandler<ClickEventArgs> OnClick;

        /// <summary>
        /// Os tipos de inputs conhecidos para a máquina de estados.
        /// </summary>
        protected enum Input
        {
            CursorOnTop, CursorOffTop, Pressed, Released
        }

        /// <summary>
        /// Os estados possíveis de um botão.
        /// </summary>
        public enum StateIds
        {
            Normal, Hovered, Pressed
        }

        public Button(IScene scene) : base(scene)
        {
            Bounds = new Rectangle(0, 0, 60, 30);
            Text = "OK";

            // mapeando os identificadores para seus respectivos estados
            States = new Dictionary<StateIds, IButtonState>();
            States[StateIds.Normal] = new Normal(this);
            States[StateIds.Hovered] = new Hovered(this);
            States[StateIds.Pressed] = new Pressed(this);

            // criando e configurando as transições para a máquina de estados
            StateMachine = new StateMachine<StateIds, Input>(StateIds.Normal);
            StateMachine.For(StateIds.Normal)
                .When(Input.CursorOnTop)    .GoTo(StateIds.Hovered);
            StateMachine.For(StateIds.Hovered)
                .When(Input.CursorOffTop)   .GoTo(StateIds.Normal)
                .When(Input.Pressed)        .GoTo(StateIds.Pressed);
            StateMachine.For(StateIds.Pressed)
                .When(Input.CursorOffTop)   .GoTo(StateIds.Normal)
                .When(Input.Released)       .GoTo(StateIds.Normal);
        }

        public override void LoadContent(ContentManager contentManager, GraphicsDevice graphicsDevice)
        {
            // textura vazia
            var dummyTexture = new Texture2D(graphicsDevice, 1, 1);
            dummyTexture.SetData(new Color[] { Color.White });
            
            Texture = dummyTexture;
            Font = contentManager.Load<SpriteFont>("arial");
        }

        public override void Update(GameTime gameTime)
        {
            var currentState = Mouse.GetState();
            
            if (Bounds.Contains(currentState.X, currentState.Y))
            {
                StateMachine.Process(Input.CursorOnTop);

                if (currentState.LeftButton == ButtonState.Pressed)
                {
                    StateMachine.Process(Input.Pressed);
                } 
                else if (_lastMouseState.LeftButton == ButtonState.Pressed)
                {
                    StateMachine.Process(Input.Released);

                    OnClick(this, new ClickEventArgs(gameTime));
                }
            } 
            else
            {
                StateMachine.Process(Input.CursorOffTop);
            }

            _lastMouseState = currentState;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            IButtonState currentState = States[StateMachine.Current];

            // desenhando o fundo
            spriteBatch.Draw(Texture, Bounds, currentState.Background);
            
            // centralizando o texto
            Vector2 size = Font.MeasureString(Text);
            var location = new Vector2(
                Location.X + (Bounds.Width - size.X)/2,
                Location.Y + (Bounds.Height - size.Y)/2
            );

            // escrevendo o texto na posição apropriada
            spriteBatch.DrawString(Font, Text, location, currentState.Foreground);
        }

        #region Properties & Fields
        /// <summary>
        /// O retângulo contendo este botão.
        /// </summary>
        public Rectangle Bounds 
        { 
            get { return _bounds; }
            set { _bounds = value; }
        }

        /// <summary>
        /// A localização deste botão na tela.
        /// </summary>
        public Point Location
        {
            get { return _bounds.Location; }
            set { _bounds.Location = value; }
        }

        /// <summary>
        /// As dimensões deste botão: a propriedade X guarda a largura, e a Y a altura.
        /// </summary>
        public Vector2 Size
        {
            get { return new Vector2(_bounds.Width, _bounds.Height); }
            set 
            { 
                _bounds.Width = (int) value.X;
                _bounds.Height = (int) value.Y;
            }
        }

        /// <summary>
        /// A cor de fundo atual deste botão.
        /// </summary>
        public Color Background 
        { 
            get { return CurrentState.Background; }
            set { CurrentState.Background = value; }
        }

        /// <summary>
        /// A cor do texto deste botão.
        /// </summary>
        public Color Foreground
        {
            get { return CurrentState.Foreground; }
            set { CurrentState.Foreground = value; }
        }

        /// <summary>
        /// O texto a ser escrito neste botão.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// A fonte para escrever o texto deste botão.
        /// </summary>
        public SpriteFont Font { get; set; }

        /// <summary>
        /// A textura para pintar este botão.
        /// </summary>
        public Texture2D Texture { get; set; }
        
        /// <summary>
        /// A máquina de estados deste botão.
        /// </summary>
        protected StateMachine<StateIds, Input> StateMachine { get; private set; }

        /// <summary>
        /// Os estados deste botão.
        /// </summary>
        protected IDictionary<StateIds, IButtonState> States { get; private set; }

        /// <summary>
        /// O estado atual deste botão.
        /// </summary>
        protected IButtonState CurrentState { get { return States[StateMachine.Current]; } }

        private Rectangle _bounds;
        private MouseState _lastMouseState;
        #endregion
    }

    public interface IButtonState
    {
        Button Button { get; }
        Color Background { get; set; }
        Color Foreground { get; set; }
    }

    public abstract class AbstractButtonState : IButtonState
    {
        protected AbstractButtonState(Button button)
        {
            Button = button;
        }

        public virtual Button Button { get; private set; }
        public virtual Color Background { get; set; }
        public virtual Color Foreground { get; set; }
    }

    public class Normal : AbstractButtonState
    {
        public Normal(Button button) :base(button)
        {
            Background = Color.Gray;
            Foreground = Color.White;
        }
    }

    public class Hovered : AbstractButtonState
    {
        public Hovered(Button button) :base(button)
        {
            Background = Color.BlueViolet;
            Foreground = Color.White;
        }
    }

    public class Pressed : AbstractButtonState
    {
        public Pressed(Button button) :base(button)
        {
            Background = Color.YellowGreen;
            Foreground = Color.White;
        }
    }

    /// <summary>
    /// Representa as informações coletadas na ocorrência de um clique.
    /// </summary>
    public class ClickEventArgs : EventArgs
    {
        public ClickEventArgs(GameTime gameTime)
        {
            Time = gameTime;
        }

        /// <summary>
        /// Quando o clique foi feito.
        /// </summary>
        public GameTime Time { get; private set; }
    }
}
