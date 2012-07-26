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
        GameComponentCollection Elements { get; }
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
            Elements = new GameComponentCollection();

            // crie o sprite batch a ser usado pelos elementos
            SpriteBatch = new SpriteBatch(game.GraphicsDevice);

            // comece com tudo desabilitado
            Disable();

            // registre este componente no ciclo de vida do Game
            game.Components.Add(this);
        }

        #region IScene
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

        public GameComponentCollection Elements { get; protected set; }
        #endregion

        #region XNA
        /// <summary>
        /// Inicializa a cena, de acordo com o ciclo de vida do XNA.
        /// 
        /// Faz uso de três métodos para inicializar de fato: <see cref="InitializeBefore"/>, 
        /// <see cref="InitializeElements"/> e <see cref="InitializeAfter"/>, que são invocados
        /// nessa ordem.
        /// </summary>
        public override void Initialize()
        {
            InitializeBefore();
            InitializeElements();
            InitializeAfter();

            base.Initialize();
        }

        /// <summary>
        /// Classes derivadas deverão colocar neste método tudo que precisa ser inicializado antes
        /// dos elementos de cena.
        /// </summary>
        protected virtual void InitializeBefore() { }

        /// <summary>
        /// Inicializa todos os elementos de cena.
        /// </summary>
        protected virtual void InitializeElements()
        {
            foreach (var element in Elements)
            {
                element.Initialize();
            }
        }

        /// <summary>
        /// Classes derivadas deverão colocar neste método tudo que precisa ser inicializado após
        /// os elementos de cena.
        /// </summary>
        protected virtual void InitializeAfter() { }

        /// <summary>
        /// Atualiza a cena, de acordo com o ciclo de vida do XNA.
        /// 
        /// Faz uso de três métodos para atualizar de fato: <see cref="UpdateBefore"/>, 
        /// <see cref="UpdateElements"/> e <see cref="UpdateAfter"/>, que são invocados nessa ordem.
        /// </summary>
        /// <param name="gameTime">O tempo de jogo, conforme reportado pelo XNA.</param>
        public override void Update(GameTime gameTime)
        {
            UpdateBefore(gameTime);
            UpdateElements(gameTime);
            UpdateAfter(gameTime);
        }

        /// <summary>
        /// Classes derivadas deverão colocar neste método tudo que precisa ser atualizado antes
        /// dos elementos de cena.
        /// </summary>
        /// <param name="gameTime">O tempo de jogo, conforme reportado pelo XNA.</param>
        protected virtual void UpdateBefore(GameTime gameTime) { }

        /// <summary>
        /// Classes derivadas deverão colocar neste método tudo que precisa ser atualizado depois
        /// dos elementos de cena.
        /// </summary>
        /// <param name="gameTime">O tempo de jogo, conforme reportado pelo XNA.</param>
        protected virtual void UpdateAfter(GameTime gameTime) { }

        /// <summary>
        /// Atualiza todos os elementos da cena.
        /// </summary>
        /// <param name="gameTime">O tempo de jogo, conforme reportado pelo XNA.</param>
        protected virtual void UpdateElements(GameTime gameTime)
        {
            foreach (var sceneElement in ElementsToUpdate)
            {
                sceneElement.Update(gameTime);
            }
        }

        /// <summary>
        /// Desenha a cena, de acordo com o ciclo de vida do XNA. 
        /// 
        /// Faz uso de três métodos para desenhar de fato: <see cref="DrawBefore"/>, 
        /// <see cref="DrawElements"/> e <see cref="DrawAfter"/>, que são invocados nessa ordem.
        /// </summary>
        /// <param name="gameTime">O tempo de jogo, conforme reportado pelo XNA.</param>
        public override void Draw(GameTime gameTime)
        {
            DrawBefore(gameTime);
            DrawElements(gameTime);
            DrawAfter(gameTime);
        }

        /// <summary>
        /// Classes derivadas deverão colocar neste método tudo que precisa ser desenhado antes
        /// dos elementos de cena.
        /// </summary>
        /// <param name="gameTime">O tempo de jogo, conforme reportado pelo XNA.</param>
        protected virtual void DrawBefore(GameTime gameTime) { }

        /// <summary>
        /// Desenha os elementos da cena. Este método deverá ser sobreescrito caso se deseje um 
        /// controle mais fino de como os elementos deverão ser desenhados.
        /// </summary>
        /// <param name="gameTime">O tempo de jogo, conforme reportado pelo XNA.</param>
        protected virtual void DrawElements(GameTime gameTime)
        {
            foreach (var sceneElement in ElementsToDraw)
            {
                sceneElement.Draw(gameTime);
            }
        }

        /// <summary>
        /// Classes derivadas deverão colocar neste método tudo que precisa ser desenhado depois
        /// dos elementos de cena.
        /// </summary>
        /// <param name="gameTime">O tempo de jogo, conforme reportado pelo XNA.</param>
        protected virtual void DrawAfter(GameTime gameTime) { }
        #endregion

        #region Helpers
        /// <summary>
        /// Os elementos atualizáveis desta cena. Esta propriedade é calculada a partir da 
        /// propriedade <see cref="Elements"/>, então é somente para leitura.
        /// </summary>
        protected IEnumerable<IUpdateable> ElementsToUpdate
        {
            get
            {
                return from e in Elements
                       where e is IUpdateable && (e as IUpdateable).Enabled
                       orderby (e as IUpdateable).UpdateOrder ascending
                       select e as IUpdateable;
            }
        }

        /// <summary>
        /// Os elementos desenháveis desta cena. Esta propriedade é calculada a partir da 
        /// propriedade <see cref="Elements"/>, então é somente para leitura.
        /// </summary>
        protected IEnumerable<IDrawable> ElementsToDraw 
        {
            get 
            { 
                return from e in Elements 
                       where e is IDrawable && (e as IDrawable).Visible
                       orderby (e as IDrawable).DrawOrder ascending 
                       select e as IDrawable; 
            }
        }

        /// <summary>
        /// Retorna o gerenciador de cenas do jogo.
        /// </summary>
        /// <typeparam name="TId">O tipo dos identificadores de cenas no gerenciador.</typeparam>
        /// <returns>O gerenciador de cenas do jogo.</returns>
        protected ISceneManagerService<TId> GetSceneManager<TId>()
        {
            return (ISceneManagerService<TId>) Game.Services.GetService(typeof(ISceneManagerService<TId>));
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

        protected SpriteBatch SpriteBatch { get; set; }
        #endregion
    }
}
