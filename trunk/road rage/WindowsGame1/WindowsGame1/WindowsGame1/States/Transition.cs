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

    
}
