using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TangoGames.RoadFighter.Scenes
{
    public interface ISceneElement
    {
        void Update(GameTime gameTime);

        IScene Scene { get; }
    }

    public interface IDrawableSceneElement : ISceneElement
    {
        void LoadContent(ContentManager contentManager, GraphicsDevice graphicsDevice);
        void Draw(GameTime gameTime, SpriteBatch spriteBatch);
    }

    public abstract class AbstractSceneElement : ISceneElement
    {
        protected AbstractSceneElement(IScene scene)
        {
            Scene = scene;
        }

        public virtual void Update(GameTime gameTime) { }
        public IScene Scene
        {
            get { return _scene; } 
            protected set
            {
                if(_scene != null)
                { // um elemento só pode pertencer a uma cena por vez, remova este da cena anterior
                    _scene.Elements.Remove(this);
                }

                _scene = value;

                if (value != null)
                { // adicione esta à nova cena
                    value.Elements.Add(this);
                }
            }
        }

        private IScene _scene;
    }

    public abstract class AbstractDrawableSceneElement : AbstractSceneElement, IDrawableSceneElement
    {
        protected AbstractDrawableSceneElement(IScene scene) : base(scene) {}

        public virtual void LoadContent(ContentManager contentManager, GraphicsDevice graphicsDevice) { }
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch) { }
    }
}
