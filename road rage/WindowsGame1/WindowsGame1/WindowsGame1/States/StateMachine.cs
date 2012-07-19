using System;
using System.Collections.Generic;

namespace TangoGames.RoadFighter.States
{
    public class StateMachine<TState, TInput>
    {
        public StateMachine(TState initial)
        {
            Current = initial;
            Transitions = new Dictionary<Transition<TState, TInput>, TState>();
        }

        public TState GetNextFor(TInput input)
        {
            TState next;
            if(! Transitions.TryGetValue(new Transition<TState, TInput>(Current, input), out next))
            { // transição desconhecida, não faça uma transição
                return Current;
            }

            return next;
        }

        public TState Process(TInput input)
        {
            Current = GetNextFor(input);
            return Current;
        }

        public TransitionAccumulator<TState, TInput> For(TState from)
        {
            return new TransitionAccumulator<TState, TInput>(this, from);
        }

        public TState Current { get; private set; }
        public IDictionary<Transition<TState, TInput>, TState> Transitions { get; private set; }
    }

    public class StateNotFoundException : ApplicationException
    {
        public StateNotFoundException(object stateId)
        {
            StateId = stateId;
        }

        public object StateId { get; private set; }
    }

    public class TransitionAccumulator<TStateId, TInput>
    {
        public TransitionAccumulator(StateMachine<TStateId, TInput> owner, TStateId from)
        {
            Owner = owner;
            From = from;
        }

        public TransitionAccumulator<TStateId, TInput> When(TInput input)
        {
            Input = input;

            return this;
        }

        public TransitionAccumulator<TStateId, TInput> GoTo(TStateId to)
        {
            if(to == null)
            {
                throw new ArgumentNullException("to", "No end state given!");
            }

            Owner.Transitions[new Transition<TStateId, TInput>(From, Input)] = to;
            return new TransitionAccumulator<TStateId, TInput>(Owner, From);
        }

        public StateMachine<TStateId, TInput> Owner
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
        public TStateId From
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
        public TInput Input
        {
            get { return _input; }
            private set
            {
                if (value == null)
                {
                    throw new ArgumentException("value", "No input given!");
                }

                _input = value;
            }
        }

        private StateMachine<TStateId, TInput> _owner;
        private TStateId _from;
        private TInput _input;
    }

}
