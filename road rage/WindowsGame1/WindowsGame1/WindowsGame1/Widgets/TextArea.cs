using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TangoGames.RoadFighter.Utils;

namespace TangoGames.RoadFighter.Widgets
{
    public class TextArea : DrawableGameComponent
    {
        public TextArea(Game game) : base(game)
        {
            Shader = new Texture2D(Game.GraphicsDevice, 1, 1);
            Shader.SetData(new Color[] { Color.Transparent });

            Background = Color.Gray;
            Foreground = Color.White;
            Padding = 5;
            Font = Game.Content.Load<SpriteFont>("arial");

            SpriteBatch = new SpriteBatch(Game.GraphicsDevice);
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Begin();

            SpriteBatch.Draw(Shader, new Rectangle(Location.X, Location.Y, (int) Size.X, (int) Size.Y), Background);
            SpriteBatch.DrawString(Font, Text, new Vector2(Location.X + Padding, Location.Y + Padding), Foreground);

            SpriteBatch.End();
        }

        public Point Location { get; set; }

        public Vector2 Size 
        {
            get 
            {
                return Font.MeasureString(Text) + new Vector2(2 * Padding, 2 * Padding); 
            }
        }

        public int Alpha 
        { 
            get
            {
                var data = new Color[1];
                Shader.GetData(data);

                return data[0].A;
            }
            set
            {
                Shader.SetData(new Color[] { Color.Transparent.WithAlpha(value) });
            }
        }
        
        private Texture2D Shader { get; set; }
        public Color Background { get; set; }
        public Color Foreground { get; set; }
        public int Padding { get; set; }
        public SpriteFont Font { get; set; }
        public string Text { get; set; }

        private SpriteBatch SpriteBatch { get; set; }
    }
}
