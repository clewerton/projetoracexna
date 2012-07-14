using System;
using System.Collections.Generic;

namespace TangoGames.RoadFighter.Scenes
{
    /// <summary>
    /// Representa cenas. Cenas são geridas pelo gerenciador de cenas, e podem ser ativadas ou 
    /// desativadas.
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
    /// Representa um gerenciador de cenas. 
    /// 
    /// <p>
    /// Armazena as cenas disponíveis, mantém qual está ativa, e faz a transição entre uma cena e
    /// outra. Apenas uma cena pode estar ativa por vez.
    /// </p>
    /// <p>
    /// As cenas são associadas e acessíveis a partir de um identificador. Os identificadores 
    /// deverão ser do tipo <code>TId</code>.
    /// </p>
    /// </summary>
    /// <typeparam name="TId">O tipo do objetos que serão usados como identificadores.</typeparam>
    public interface ISceneManagerService<TId>
    {
        /// <summary>
        /// Acessa a cena associada a <code>id</code>, e associa uma cena a <code>id</code>. 
        /// 
        /// Caso já haja uma cena com este identificador, ela será desativada antes da troca 
        /// ocorrer.
        /// </summary>
        /// <param name="id">O identificador da cena desejada.</param>
        /// <returns>A cena associada ao identificador <code>id</code>.</returns>
        /// <exception cref="ArgumentNullException">Quando se tenta guardar uma cena nula.</exception>
        IScene this[TId id] { get; set; }

        /// <summary>
        /// A cena atualmente ativa. 
        /// 
        /// Esta propriedade é somente para leitura; use <see cref="GoTo"/> para mudar qual cena 
        /// está ativa.
        /// </summary>
        IScene Current { get; }

        /// <summary>
        /// Muda a cena ativa para aquela associada a <code>id</code>. A cena corrente será 
        /// desativada (<see cref="IScene.Leave"/>), e a nova será ativada 
        /// (<see cref="IScene.Enter"/>).
        /// 
        /// Caso a nova cena seja a mesma que já está ativa, ela será recarregada.
        /// </summary>
        /// <param name="id">O identificador da cena a ativar.</param>
        /// <exception cref="ArgumentNullException">Se o identificador dado é nulo.</exception>
        /// <exception cref="SceneNotFoundException">Se não há cena associada a este identificador.
        /// </exception>
        void GoTo(TId id);

        /// <summary>
        /// Desativa a cena corrente. Após a execução deste método, nenhuma cena estará ativa.
        /// </summary>
        void Stop();
    }

    /// <summary>
    /// Lançado quando não há uma cena associada ao identificador dado.
    /// </summary>
    public class SceneNotFoundException : ApplicationException
    {
        /// <summary>
        /// Cria uma nova instância e armazena o identificador desconhecido.
        /// </summary>
        /// <param name="id">O identificador desconhecido.</param>
        public SceneNotFoundException(object id) 
            : base("Scene not found: " + id)
        {
            Id = id;
        }

        /// <summary>
        /// O identificador desconhecido.
        /// </summary>
        public object Id { get; private set; }
    }

    /// <summary>
    /// Uma implementação simples de gerenciador de cenas.
    /// </summary>
    public class SceneManager<TId> : Dictionary<TId, IScene>, ISceneManagerService<TId>
    {
        #region ISceneManagerService Operations
        public new IScene this[TId id] 
        { 
            get { return base[id]; }
            set
            {
                if (id == null) // null is not a valid argument!
                {
                    throw new ArgumentNullException();
                }

                if (! ContainsKey(id)) // new scene, just add it
                {
                    base[id] = value;
                    return;
                }

                // finalize the old one before swappage
                var old = base[id];

                if (old.Equals(Current)) // if old is running, leave it before swapping
                {
                    old.Leave();
                    Current = null;
                }

                // the changing of the guard
                base[id] = value;
            }
        }

        public void GoTo(TId id)
        {
            // throw up on null ids, since some types are not nullable
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            // throw up on unknown ids
            if (! ContainsKey(id))
            {
                throw new SceneNotFoundException(id);
            }

            // set the new current
            Current = this[id];
        }

        public void Stop()
        {
            Current = null;
        }

        public IScene Current
        {
            get { return _current; }
            private set
            {
                if(_current != null) { _current.Leave(); }

                _current = value;

                if (_current != null) { _current.Enter(); }
            }
        }
        #endregion
        
        #region Properties & Fields
        private IScene _current;
        #endregion   
    }
}
