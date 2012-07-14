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
        /// Chamada quando esta cena é ativada.
        /// </summary>
        void Enter();

        /// <summary>
        /// Chamada quando esta cena é desativada.
        /// </summary>
        void Leave();
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

        public virtual void Enter()
        {
            Enable();
        }

        public virtual void Leave()
        {
            Disable();
        }

        // habilita o componente no ciclo de vida do XNA
        private void Enable()
        {
            Enabled = true; // habilita o Update
            Visible = true; // habilita o Draw
        }

        // desabilita o componente no ciclo de vida do XNA
        private void Disable()
        {
            Enabled = false; // desabilita o Update
            Visible = false; // desabilita o Draw
        }
    }
}
