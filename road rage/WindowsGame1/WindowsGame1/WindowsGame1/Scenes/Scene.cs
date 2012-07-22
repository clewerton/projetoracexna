using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TangoGames.RoadFighter.Scenes
{
    /// <summary>
    /// Representa cenas. Cenas são geridas pelo <see cref="ISceneManagerService{TId}">gerenciador 
    /// de cenas</see>, e podem ser ativadas ou desativadas.
    /// </summary>
    public interface IScene
    {
        /// <summary>
        /// Chamado quando esta cena é ativada.
        /// </summary>
        void Enter();

        /// <summary>
        /// Chamado quando esta cena é desativada.
        /// </summary>
        void Leave();

        /// <summary>
        /// Chamado quando esta cena é pausada.
        /// </summary>
        void Pause();

        /// <summary>
        /// Chamado quando esta cena é resumida.
        /// </summary>
        void Resume();

        /// <summary>
        /// Retorna verdadeiro se esta cena estiver pausada.
        /// </summary>
        bool Paused { get; }

        /// <summary>
        /// A lista de elementos nesta cena.
        /// </summary>
        IList<ISceneElement> Elements { get; }
    }

    /// <summary>
    /// Uma implementação simples de <see cref="IScene"/>, usando a infraestrutura de 
    /// <see cref="DrawableGameComponent">GameComponents</see> desenháveis provida pelo XNA. 
    /// </summary>
    public class Scene : DrawableGameComponent, IScene
    {
        /// <summary>
        /// Cria uma nova instância.
        /// 
        /// Toda cena é registrada no jogo e começa desabilitada, deixando para o 
        /// <see cref="ISceneManagerService{TId}">gerenciador de cenas</see> a responsabilidade de 
        /// ativá-la no momento adequado.
        /// </summary>
        /// <param name="game">A instância de <see cref="Game"/> controlando o jogo.</param>
        public Scene(Game game) : base(game)
        {
            // crie a lista de elementos da cena
            Elements = new List<ISceneElement>();

            // crie o sprite batch a ser usado pelos elementos
            SpriteBatch = new SpriteBatch(game.GraphicsDevice);

            // comece com tudo desabilitado
            Disable();

            // registre este componente no ciclo de vida do Game
            game.Components.Add(this);
        }

        #region IScene Operations
        protected override void LoadContent()
        {
            // carregue o conteúdo de cada elemento desenhável
            foreach (var sceneElement in DrawableElements)
            {
                sceneElement.LoadContent(Game.Content, Game.GraphicsDevice);
            }

            // carregue o conteúdo padrão da superclasse
            base.LoadContent();
        }

        public virtual void Enter()
        {
            Enable();
        }

        public virtual void Leave()
        {
            Disable();
        }

        public virtual void Pause()
        {
            Enabled = false; // desabilita o Update
        }

        public virtual void Resume()
        {
            Enabled = true; // habilita o Update
        }

        public bool Paused { get { return ! Enabled; } }

        public IList<ISceneElement> Elements { get; protected set; }
        #endregion

        public override void Update(GameTime gameTime)
        {
            foreach (var sceneElement in Elements)
            {
                sceneElement.Update(gameTime);
            }
        }

        /// <summary>
        /// Desenha a cena, de acordo com o ciclo de vida do XNA. 
        /// 
        /// Faz uso de três métodos para desenhar de fato: <see cref="DrawBefore"/>, 
        /// <see cref="DrawElements"/> e  <see cref="DrawAfter"/>, que são invocados nessa ordem.
        /// Este método prepara o SpriteBatch a ser repassado aos métodos.
        /// </summary>
        /// <param name="gameTime">O tempo de jogo, conforme reportado pelo Game.</param>
        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Begin();

            DrawBefore(gameTime, SpriteBatch);
            DrawElements(gameTime, SpriteBatch);
            DrawAfter(gameTime, SpriteBatch);

            SpriteBatch.End();
        }

        /// <summary>
        /// Classes derivadas deverão colocar neste método tudo que precisa ser desenhado antes
        /// dos elementos de cena.
        /// </summary>
        /// <param name="gameTime">O tempo de jogo, conforme reportado pelo Game.</param>
        /// <param name="spriteBatch">O sprite batch usado para desenhar este quadro, já pronto 
        /// para uso.</param>
        public virtual void DrawBefore(GameTime gameTime, SpriteBatch spriteBatch) {}

        /// <summary>
        /// Desenha os elementos da cena. Este método deverá ser sobreescrito caso se deseje um 
        /// controle mais fino de como os elementos deverão ser desenhados.
        /// </summary>
        /// <param name="gameTime">O tempo de jogo, conforme reportado pelo Game.</param>
        /// <param name="spriteBatch">O sprite batch usado para desenhar este quadro, já pronto 
        /// para uso.</param>
        public virtual void DrawElements(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (var sceneElement in DrawableElements)
            {
                sceneElement.Draw(gameTime, spriteBatch);
            }
        }

        /// <summary>
        /// Classes derivadas deverão colocar neste método tudo que precisa ser desenhado depois
        /// dos elementos de cena.
        /// </summary>
        /// <param name="gameTime">O tempo de jogo, conforme reportado pelo Game.</param>
        /// <param name="spriteBatch">O sprite batch usado para desenhar este quadro, já pronto 
        /// para uso.</param>
        public virtual void DrawAfter(GameTime gameTime, SpriteBatch spriteBatch) {}

        /// <summary>
        /// Os elementos desenháveis desta cena. Esta propriedade é calculada a
        /// partir da propriedade <see cref="Elements" />, então é somente para
        /// leitura.
        /// </summary>
        protected IEnumerable<IDrawableSceneElement> DrawableElements 
        {
            get { return from e in Elements where e is IDrawableSceneElement select e as IDrawableSceneElement; }
        }

        // habilita o componente no ciclo de vida do XNA
        private void Enable()
        {
            Resume();
            Visible = true; // habilita o Draw
        }

        // desabilita o componente no ciclo de vida do XNA
        private void Disable()
        {
            Pause();
            Visible = false; // desabilita o Draw
        }

        /// <summary>
        /// Retorna o gerenciador de cenas do jogo.
        /// </summary>
        /// <typeparam name="TId">O tipo dos identificadores de cenas no gerenciador.</typeparam>
        /// <returns>O gerenciador de cenas do jogo.</returns>
        public ISceneManagerService<TId> GetSceneManager<TId>()
        {
            return (ISceneManagerService<TId>) Game.Services.GetService(typeof(ISceneManagerService<TId>));
        }

        public SpriteBatch SpriteBatch { get; private set; }
    }
}
