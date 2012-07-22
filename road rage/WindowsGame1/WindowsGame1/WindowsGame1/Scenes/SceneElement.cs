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
        public virtual void Update(GameTime gameTime) { }
        public IScene Scene { get; protected set; }
    }

    public abstract class AbstractDrawableSceneElement : AbstractSceneElement, IDrawableSceneElement
    {
        public virtual void LoadContent(ContentManager contentManager, GraphicsDevice graphicsDevice) { }
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch) { }
    }
}
