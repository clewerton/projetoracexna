using System;
using System.Collections.Generic;

namespace TangoGames.RoadFighter.States
{
    public class StateMachine<TStateId, TState, TInput>
    {
        public StateMachine(TStateId initial)
        {
            Current = initial;
            States = new Dictionary<TStateId, TState>();
            Transitions = new Dictionary<Transition<TStateId, TInput>, TStateId>();
        }

        public TStateId GetNextFor(TInput input)
        {
            TStateId next;
            if(! Transitions.TryGetValue(new Transition<TStateId, TInput>(Current, input), out next))
            { // transição desconhecida, não faça uma transição
                return Current;
            }

            if(! States.ContainsKey(next))
            { // estado final desconhecido, reporte um erro
                throw new StateNotFoundException(next);
            }

            return next;
        }

        public TStateId Process(TInput input)
        {
            Current = GetNextFor(input);
            return Current;
        }

        public TransitionAccumulator<TStateId, TState, TInput> On(TStateId from, TInput input)
        {
            return new TransitionAccumulator<TStateId, TState, TInput>(this, from, input);
        }

        public TStateId Current { get; private set; }
        public TState CurrentState { get { return States[Current]; } }
        public IDictionary<TStateId, TState> States { get; private set; }
        public IDictionary<Transition<TStateId, TInput>, TStateId> Transitions { get; private set; }
    }

    public class StateNotFoundException : ApplicationException
    {
        public StateNotFoundException(object stateId)
        {
            StateId = stateId;
        }

        public object StateId { get; private set; }
    }

    public class TransitionAccumulator<TStateId, TState, TInput>
    {
        public TransitionAccumulator(StateMachine<TStateId, TState, TInput> owner, TStateId from, TInput input)
        {
            Owner = owner;
            From = from;
            Input = input;
        }

        public void GoTo(TStateId to)
        {
            Owner.Transitions[new Transition<TStateId, TInput>(From, Input)] = to;
        }

        public StateMachine<TStateId, TState, TInput> Owner { get; private set; }
        public TStateId From { get; private set; }
        public TInput Input { get; private set; }
    }

}
