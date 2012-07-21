using System;
using System.Collections.Generic;

namespace TangoGames.RoadFighter.States
{
    /// <summary>
    /// Uma máquina de estados simples. 
    /// 
    /// Inspirações para esta implementação:
    /// * http://stackoverflow.com/questions/5923767/simple-state-machine-example-in-c
    /// * http://code.google.com/p/stateless/
    /// </summary>
    /// <typeparam name="TState">O tipo dos estados.</typeparam>
    /// <typeparam name="TInput">O tipo da entrada das transições.</typeparam>
    public class StateMachine<TState, TInput>
    {
        /// <summary>
        /// Cria uma nova máquina de estados.
        /// </summary>
        /// <param name="initial">O estado inicial.</param>
        public StateMachine(TState initial)
        {
            Current = initial;
            Transitions = new Dictionary<Transition<TState, TInput>, TState>();
        }

        /// <summary>
        /// Calcula o novo estado dada uma entrada e o estado corrente. 
        /// 
        /// Não faz a transição de fato; use <see cref="Process"/> para tal.
        /// </summary>
        /// <param name="input">A entrada dada.</param>
        /// <returns>O estado resultante de uma transição dado o estado 
        /// corrente e a entrada <code>input</code>.</returns>
        public TState GetNextFor(TInput input)
        {
            TState next;
            if(! Transitions.TryGetValue(new Transition<TState, TInput>(Current, input), out next))
            { // transição desconhecida, não faça uma transição
                return Current;
            }

            return next;
        }

        /// <summary>
        /// Lê a entrada dada e faz a transição do estado corrente para o 
        /// <see cref="GetNextFor">estado resultante</see>, que passa a ser o
        /// novo estado corrente.
        /// </summary>
        /// <param name="input">A entrada dada.</param>
        /// <returns>O novo estado corrente.</returns>
        public TState Process(TInput input)
        {
            Current = GetNextFor(input);
            return Current;
        }

        /// <summary>
        /// Uma mini-DSL para facilitar o cadastro de novas transições.
        /// </summary>
        /// <param name="from">O estado inicial da nova transição a ser 
        /// cadastrada.</param>
        /// <returns>Um objeto responsável por cadastrar uma nova transição 
        /// saindo de <code>from</code>.</returns>
        public WhenCollector<TState, TInput> For(TState from)
        {
            return new WhenCollector<TState, TInput>(this, from);
        }

        /// <summary>
        /// O estado corrente.
        /// </summary>
        public TState Current { get; private set; }

        /// <summary>
        /// A tabela de transições.
        /// </summary>
        public IDictionary<Transition<TState, TInput>, TState> Transitions { get; private set; }
    }

    /// <summary>
    /// Fornece uma mini-DSL para cadastrar novas transições. Esta classe 
    /// coleta a entrada da nova transição, e a repassa a uma outra classe, que
    /// irá coletar o estado de destino e cadastrar a transição de fato.
    /// </summary>
    /// <typeparam name="TState">O tipo dos estados da máquina de estados original.</typeparam>
    /// <typeparam name="TInput">O tipo de entrada da máquina de estados original.</typeparam>
    public class WhenCollector<TState, TInput>
    {
        /// <summary>
        /// Cria uma nova instância.
        /// </summary>
        /// <param name="owner">A máquina de estados de origem.</param>
        /// <param name="from">O estado inicial desta nova transição.</param>
        /// <exception cref="ArgumentNullException">Um dos argumentos dados foi nulo.</exception>
        internal WhenCollector(StateMachine<TState, TInput> owner, TState from)
        {
            Owner = owner;
            From = from;
        }

        /// <summary>
        /// Guarda a entrada da nova transição.
        /// </summary>
        /// <param name="input">A entrada da nova transição.</param>
        /// <returns>Um objeto para coletar o estado de destino e cadastrar a nova transição.</returns>
        /// <exception cref="ArgumentNullException">A entrada dada foi nula.</exception>
        public GoToCollector<TState, TInput> When(TInput input)
        {
            return new GoToCollector<TState, TInput>(Owner, From, input);
        }

        /// <summary>
        /// A máquina de estados onde a transição será cadastrada.
        /// </summary>
        public StateMachine<TState, TInput> Owner
        {
            get { return _owner; }
            private set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value", "No owner given!");
                }

                _owner = value;
            }
        }

        /// <summary>
        /// O estado de origem da transição.
        /// </summary>
        public TState From
        {
            get { return _from; }
            private set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value", "No starting state given!");
                }

                _from = value;
            }
        }

        private StateMachine<TState, TInput> _owner;
        private TState _from;
    }

    /// <summary>
    /// Fornece uma mini-DSL para cadastrar novas transições. Objetos desta 
    /// classe recebem a máquina de estados, o estado inicial e a entrada 
    /// lida, e se responsabilizam por coletar o estado de destino e cadastrar
    /// a transição de fato.
    /// </summary>
    /// <typeparam name="TState">O tipo dos estados da máquina de estados original.</typeparam>
    /// <typeparam name="TInput">O tipo de entrada da máquina de estados original.</typeparam>
    public class GoToCollector<TState, TInput> 
    {
        /// <summary>
        /// Cria uma nova instância.
        /// </summary>
        /// <param name="owner">A máquina de estados de origem.</param>
        /// <param name="from">O estado inicial desta nova transição.</param>
        /// <param name="input">A entrada da nova transição.</param>
        /// <exception cref="ArgumentNullException">Um dos argumentos dados foi nulo.</exception>
        internal GoToCollector(StateMachine<TState, TInput> owner, TState from, TInput input)
        {
            Owner = owner;
            From = from;
            Input = input;
        }

        /// <summary>
        /// Obtém o estado destino e cadastra a transição final na máquina de estados.
        /// </summary>
        /// <param name="to">O estado de destino da nova transição.</param>
        /// <returns>Um novo acumulador de transições, para o usuário cadastrar
        /// uma nova transição com a mesma máquina de estados e o mesmo estado 
        /// inicial.</returns>
        /// <exception cref="ArgumentNullException">O estado de destino dado foi nulo.</exception>
        public WhenCollector<TState, TInput> GoTo(TState to)
        {
            if (to == null)
            {
                throw new ArgumentNullException("to", "No end state given!");
            }

            Owner.Transitions[new Transition<TState, TInput>(From, Input)] = to;
            return new WhenCollector<TState, TInput>(Owner, From);
        }

        /// <summary>
        /// A máquina de estados onde a transição será cadastrada.
        /// </summary>
        public StateMachine<TState, TInput> Owner
        {
            get { return _owner; }
            private set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value", "No owner given!");
                }

                _owner = value;
            }
        }

        /// <summary>
        /// O estado de origem da transição.
        /// </summary>
        public TState From
        {
            get { return _from; }
            private set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value", "No starting state given!");
                }

                _from = value;
            }
        }

        /// <summary>
        /// A entrada que aciona a transição.
        /// </summary>
        public TInput Input
        {
            get { return _input; }
            private set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value", "No input given!");
                }

                _input = value;
            }
        }

        private StateMachine<TState, TInput> _owner;
        private TState _from;
        private TInput _input;
    }

}
