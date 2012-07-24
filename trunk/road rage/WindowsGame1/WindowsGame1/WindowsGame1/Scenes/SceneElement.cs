using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TangoGames.RoadFighter.Scenes
{
    /// <summary>
    /// Um objeto cujo ciclo de vida é associado à uma cena.
    /// </summary>
    public interface ISceneElement
    {
        /// <summary>
        /// Chamado pela <see cref="Scene">cena</see> para atualizar este elemento.
        /// </summary>
        /// <param name="gameTime">O tempo de jogo, conforme reportado pelo XNA.</param>
        void Update(GameTime gameTime);

        /// <summary>
        /// A cena que contém este elemento.
        /// </summary>
        IScene Scene { get; }
    }

    /// <summary>
    /// Um <see cref="ISceneElement">elemento de cena</see> desenhável.
    /// </summary>
    public interface IDrawableSceneElement : ISceneElement
    {
        /// <summary>
        /// Chamado pela <see cref="ISceneElement.Scene">cena</see> para carregar qualquer conteúdo 
        /// externo.
        /// </summary>
        /// <param name="contentManager">Carrega conteúdo externo.</param>
        /// <param name="graphicsDevice">Oferece acesso a primitivas de renderização.</param>
        void LoadContent(ContentManager contentManager, GraphicsDevice graphicsDevice);

        /// <summary>
        /// Chamado pela <see cref="ISceneElement.Scene">cena</see> para desenhar este elemento.
        /// </summary>
        /// <param name="gameTime">O tempo de jogo, conforme reportado pelo XNA.</param>
        /// <param name="spriteBatch">O sprite batch usado pela cena.</param>
        void Draw(GameTime gameTime, SpriteBatch spriteBatch);
    }

    /// <summary>
    /// Fornece uma implementação básica para <see cref="ISceneElement"/>.
    /// </summary>
    public abstract class AbstractSceneElement : ISceneElement
    {
        protected AbstractSceneElement(IScene scene)
        {
            Scene = scene;
        }

        public virtual void Update(GameTime gameTime) { }

        /// <summary>
        /// A cena que contém este elemento. Esta implementação permite que o objeto seja movido 
        /// de uma cena à outra através deste setter.
        /// </summary>
        public IScene Scene
        {
            get { return _scene; } 
            protected set
            {
                if(_scene != null)
                { // um elemento só pode pertencer a uma cena por vez, remova este da cena anterior
                    _scene.Elements.Remove(this);
                }

                if (value != null)
                { // adicione este elemento à nova cena
                    value.Elements.Add(this);
                }

                _scene = value;
            }
        }

        private IScene _scene;
    }

    /// <summary>
    /// Fornece uma implementação básica para <see cref="IDrawableSceneElement"/>.
    /// </summary>
    public abstract class AbstractDrawableSceneElement : AbstractSceneElement, IDrawableSceneElement
    {
        protected AbstractDrawableSceneElement(IScene scene) : base(scene) {}

        public virtual void LoadContent(ContentManager contentManager, GraphicsDevice graphicsDevice) { }
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch) { }
    }
}
