using System.Collections.Generic;

namespace TangoGames.RoadFighter.States
{
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
