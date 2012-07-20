using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TangoGames.RoadFighter.Scenes;
using TangoGames.RoadFighter.Widgets;

namespace TangoGames.RoadFighter.Levels
{
    public class Menu : Scene
    {
        public Menu(Game game) : base(game) {}

        protected override void LoadContent()
        {
            base.LoadContent();

            // configurando o SpriteBatch
            SpriteBatch = new SpriteBatch(Game.GraphicsDevice);

            // configurando os botões da tela
            //var dummyTexture = new Texture2D(GraphicsDevice, 1, 1);
            //dummyTexture.SetData(new Color[] { Color.White });
            var texture = Game.Content.Load<Texture2D>("Widgets/Button");
            var arial = Game.Content.Load<SpriteFont>("arial");

            var clientBounds = Game.Window.ClientBounds;

            ToIntro = new Button(texture, arial);
            ToIntro.OnClick += (sender, args) => GetSceneManager<MainGame.Scenes>().GoTo(MainGame.Scenes.Intro);
            ToIntro.Location = new Point((clientBounds.Width - ButtonWidth) / 2, (clientBounds.Height - ButtonHeight) / 2 - ButtonHeight - Padding);
            ToIntro.Size = new Vector2(ButtonWidth, ButtonHeight);
            ToIntro.Text = "To INTRO";

            ToFase = new Button(texture, arial);
            ToFase.OnClick += (sender, args) => GetSceneManager<MainGame.Scenes>().GoTo(MainGame.Scenes.Fase);
            ToFase.Location = new Point((clientBounds.Width - ButtonWidth) / 2, (clientBounds.Height - ButtonHeight) / 2);
            ToFase.Size = new Vector2(ButtonWidth, ButtonHeight);
            ToFase.Text = "To FASE";

            ToEnd = new Button(texture, arial);
            ToEnd.OnClick += (sender, args) => GetSceneManager<MainGame.Scenes>().GoTo(MainGame.Scenes.End);
            ToEnd.Location = new Point((clientBounds.Width - ButtonWidth) / 2, (clientBounds.Height + ButtonHeight) / 2 + Padding);
            ToEnd.Size = new Vector2(ButtonWidth, ButtonHeight);
            ToEnd.Text = "To END";
        }

        public override void Update(GameTime gameTime)
        {
            ToIntro.Update(gameTime);
            ToFase.Update(gameTime);
            ToEnd.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            Game.GraphicsDevice.Clear(Color.Brown);

            SpriteBatch.Begin();
            ToIntro.Draw(gameTime, SpriteBatch);
            ToFase.Draw(gameTime, SpriteBatch);
            ToEnd.Draw(gameTime, SpriteBatch);
            SpriteBatch.End();
        }

        public Button ToIntro { get; private set; }
        public Button ToFase { get; private set; }
        public Button ToEnd { get; private set; }

        private SpriteBatch SpriteBatch { get; set; }
        private const int ButtonWidth = 150;
        private const int ButtonHeight = 60;
        private const int Padding = 5;
    }
}
