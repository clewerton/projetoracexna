using Microsoft.Xna.Framework;

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
            // comece com tudo desabilitado
            Disable();

            // registre este componente no ciclo de vida do Game
            game.Components.Add(this);
        }

        #region IScene Operations
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
        #endregion

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
    }
}
