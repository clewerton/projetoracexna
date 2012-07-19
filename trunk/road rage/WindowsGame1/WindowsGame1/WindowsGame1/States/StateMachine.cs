
using System;
using System.Collections.Generic;

namespace TangoGames.RoadFighter.States
{
    public class Transition<TState, TInput>
    {
        public Transition(TState state, TInput input)
        {
            State = state;
            Input = input;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;

            var other = obj as Transition<TState, TInput>;
            return other != null && object.Equals(State, other.State) && object.Equals(Input, other.Input);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (State.GetHashCode() * 397) ^ Input.GetHashCode();
            }
        }

        public TState State { get; private set; }
        public TInput Input { get; private set; }
    }

    public class StateMachine<TState, TInput> : Dictionary<Transition<TState, TInput>, TState>
    {
        public StateMachine(TState initial)
        {
            Current = initial;
        }

        public TState GetNextFor(TInput input)
        {
            TState next;
            if(! TryGetValue(new Transition<TState, TInput>(Current, input), out next))
            {
                return Current; // não faça uma transição
            }

            return next;
        }

        public TState Process(TInput input)
        {
            Current = GetNextFor(input);
            return Current;
        }

        public TState Current { get; private set; }
    }
}
