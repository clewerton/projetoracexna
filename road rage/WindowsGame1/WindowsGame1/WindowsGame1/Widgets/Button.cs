using System;
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

        public Button(Texture2D texture, SpriteFont font)
        {
            Texture = texture;
            Font = font;

            Bounds = new Rectangle(0, 0, 60, 30);
            Background = Color.Gray;
            Foreground = Color.White;
            Text = "OK";

            var normal = new Normal(this);
            var hovered = new Hovered(this);
            var pressed = new Pressed(this);

            StateMachine = new StateMachine<IButtonState, Input>(normal);
            StateMachine[new Transition<IButtonState, Input>(normal, Input.CursorOnTop)] = hovered;
            StateMachine[new Transition<IButtonState, Input>(hovered, Input.CursorOffTop)] = normal;
            StateMachine[new Transition<IButtonState, Input>(hovered, Input.Pressed)] = pressed;
            StateMachine[new Transition<IButtonState, Input>(pressed, Input.CursorOffTop)] = normal;
            StateMachine[new Transition<IButtonState, Input>(pressed, Input.Released)] = normal;
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
            // desenhando o fundo
            spriteBatch.Draw(Texture, Bounds, StateMachine.Current.Background);
            
            // centralizando o texto
            Vector2 size = Font.MeasureString(Text);
            var location = new Vector2(
                Location.X + (Bounds.Width - size.X)/2,
                Location.Y + (Bounds.Height - size.Y)/2
            );

            spriteBatch.DrawString(Font, Text, location, StateMachine.Current.Foreground);
        }

        #region Properties & Fields
        public StateMachine<IButtonState, Input> StateMachine { get; private set; }

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

        public Color Background { get; set; }
        public Color Foreground { get; set; }
        public SpriteFont Font { get; set; }
        public Texture2D Texture { get; set; }
        public string Text { get; set; }

        private Rectangle _bounds;
        private MouseState _lastMouseState;
        #endregion
    }

    public interface IButtonState
    {
        Button Button { get; }
        Color Background { get; }
        Color Foreground { get; }
    }

    public class Normal : IButtonState
    {
        public Normal(Button button)
        {
            Button = button;
        }

        public Button Button { get; private set; }
        public Color Background
        {
            get { return Button.Background; }
        }
        public Color Foreground
        {
            get { return Button.Foreground; }
        }
    }

    public class Hovered : IButtonState
    {
        public Hovered(Button button)
        {
            Button = button;
        }

        public Button Button { get; private set; }
        public Color Background
        {
            get { return Color.BlueViolet; }
        }
        public Color Foreground
        {
            get { return Button.Foreground; }
        }
    }

    public class Pressed : IButtonState
    {
        public Pressed(Button button)
        {
            Button = button;
        }

        public Button Button { get; private set; }
        public Color Background
        {
            get { return Color.YellowGreen; }
        }
        public Color Foreground
        {
            get { return Button.Foreground; }
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
