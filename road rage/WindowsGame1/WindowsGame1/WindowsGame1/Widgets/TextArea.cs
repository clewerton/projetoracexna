using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TangoGames.RoadFighter.Widgets
{
    public class TextArea : DrawableGameComponent
    {
        public TextArea(Game game) : base(game)
        {
            Background = new Color(Color.Gray.R, Color.Gray.G, Color.Gray.B, 0.2f);
            Foreground = Color.White;
            Padding = 5;
            Font = Game.Content.Load<SpriteFont>("arial");

            SpriteBatch = new SpriteBatch(Game.GraphicsDevice);
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Begin();

            SpriteBatch.Draw(Texture, new Rectangle(Location.X, Location.Y, (int) Size.X, (int) Size.Y), Color.White);
            SpriteBatch.DrawString(Font, Text, new Vector2(Location.X + Padding, Location.Y + Padding), Foreground);

            SpriteBatch.End();
        }

        private Texture2D _texture;
        public Texture2D Texture
        {
            get
            {
                if (_texture == null)
                {
                    var dummyTexture = new Texture2D(Game.GraphicsDevice, 1, 1);
                    dummyTexture.SetData(new Color[] { Color.FromNonPremultiplied(Background.R, Background.G, Background.B, Background.A) });

                    return dummyTexture;
                }

                return _texture;
            }
            set { _texture = value; }
        }

        public Point Location { get; set; }
        public Vector2 Size 
        {
            get {
                return Font.MeasureString(Text) + new Vector2(2 * Padding, 2 * Padding); 
            }
        }
        public Color Background { get; set; }
        public Color Foreground { get; set; }
        public int Padding { get; set; }
        public SpriteFont Font { get; set; }
        public string Text { get; set; }

        private SpriteBatch SpriteBatch { get; set; }
    }
}
