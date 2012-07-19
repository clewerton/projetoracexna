using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TangoGames.RoadFighter.States;

namespace TangoGames.RoadFighter.Widgets
{
    public class Button
    {
        /// <summary>
        /// Registra eventos de clique.
        /// </summary>
        public event EventHandler<ClickEventArgs> OnClick;

        public enum Input
        {
            CursorOnTop, CursorOffTop, Pressed, Released
        }

        public enum StateIds
        {
            Normal, Hovered, Pressed
        }

        public Button(Texture2D texture, SpriteFont font)
        {
            Texture = texture;
            Font = font;
            
            Bounds = new Rectangle(0, 0, 60, 30);
            Background = Color.Gray;
            Foreground = Color.White;
            Text = "OK";

            // mapeando os identificadores para seus respectivos estados
            States = new Dictionary<StateIds, IButtonState>();
            States[StateIds.Normal] = new Normal(this);
            States[StateIds.Hovered] = new Hovered(this);
            States[StateIds.Pressed] = new Pressed(this);

            // criando e configurando as transições para a máquina de estados
            StateMachine = new StateMachine<StateIds, Input>(StateIds.Normal);
            StateMachine.For(StateIds.Normal)
                .When(Input.CursorOnTop).GoTo(StateIds.Hovered);
            StateMachine.For(StateIds.Hovered)
                .When(Input.CursorOffTop).GoTo(StateIds.Normal)
                .When(Input.Pressed).GoTo(StateIds.Pressed);
            StateMachine.For(StateIds.Pressed)
                .When(Input.CursorOffTop).GoTo(StateIds.Normal)
                .When(Input.Released).GoTo(StateIds.Normal);
        }

        public void Update(GameTime gameTime)
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

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
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

            spriteBatch.DrawString(Font, Text, location, currentState.Foreground);
        }

        #region Properties & Fields
        public Rectangle Bounds 
        { 
            get { return _bounds; }
            set { _bounds = value; }
        }

        public Point Location
        {
            get { return _bounds.Location; }
            set { _bounds.Location = value; }
        }

        public Vector2 Size
        {
            get { return new Vector2(_bounds.Width, _bounds.Height); }
            set 
            { 
                _bounds.Width = (int) value.X;
                _bounds.Height = (int) value.Y;
            }
        }

        public Color Background { get; set; }
        public Color Foreground { get; set; }
        public SpriteFont Font { get; set; }
        public Texture2D Texture { get; set; }
        public string Text { get; set; }
        protected StateMachine<StateIds, Input> StateMachine { get; private set; }
        protected IDictionary<StateIds, IButtonState> States { get; private set; }

        private Rectangle _bounds;
        private MouseState _lastMouseState;
        #endregion
    }

    public interface IButtonState
    {
        Button Button { get; }
        Color Background { get; }
        Color Foreground { get; }
        string Text { get; }
    }

    public abstract class AbstractButtonState : IButtonState
    {
        protected AbstractButtonState(Button button)
        {
            Button = button;
        }

        public virtual Button Button { get; private set; }
        public virtual Color Background { get { return Button.Background; } }
        public virtual Color Foreground { get { return Button.Foreground; } }
        public virtual string Text { get { return Button.Text; } }
    }

    public class Normal : AbstractButtonState
    {
        public Normal(Button button) :base(button) {}
    }

    public class Hovered : AbstractButtonState
    {
        public Hovered(Button button) :base(button) {}

        public override Color Background
        {
            get { return Color.BlueViolet; }
        }
    }

    public class Pressed : AbstractButtonState
    {
        public Pressed(Button button) :base(button) {}

        public override Color Background
        {
            get { return Color.YellowGreen; }
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
