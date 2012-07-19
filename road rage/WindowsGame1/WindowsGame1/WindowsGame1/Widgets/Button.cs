using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TangoGames.RoadFighter.Widgets
{
    public class Button
    {
        /// <summary>
        /// Registra eventos de clique.
        /// </summary>
        public event EventHandler<ClickEventArgs> OnClick; 

        public Button(Texture2D texture, SpriteFont font)
        {
            Texture = texture;
            Font = font;

            Bounds = new Rectangle(0, 0, 60, 30);
            Background = Color.Gray;
            Foreground = Color.White;
            Text = "OK";
        }

        public void Update(GameTime gameTime)
        {
            var currentState = Mouse.GetState();

            if (Bounds.Contains(currentState.X, currentState.Y))
            {
                if (_lastMouseState.LeftButton == ButtonState.Pressed
                    && currentState.LeftButton == ButtonState.Released)
                {
                    OnClick(this, new ClickEventArgs(gameTime));
                }
            }
            _lastMouseState = currentState;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // desenhando o fundo
            spriteBatch.Draw(Texture, Bounds, Background);
            
            // centralizando o texto
            Vector2 size = Font.MeasureString(Text);
            var location = new Vector2(
                Location.X + (Bounds.Width - size.X)/2,
                Location.Y + (Bounds.Height - size.Y)/2
            );

            spriteBatch.DrawString(Font, Text, location, Foreground);
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

        public Color Background { get; set; }
        public Color Foreground { get; set; }
        public SpriteFont Font { get; set; }
        public Texture2D Texture { get; set; }
        public string Text { get; set; }

        private Rectangle _bounds;
        private MouseState _lastMouseState;
        #endregion
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
