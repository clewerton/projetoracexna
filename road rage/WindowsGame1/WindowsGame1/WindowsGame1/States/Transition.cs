namespace TangoGames.RoadFighter.States
{
    public class Transition<TStateId, TInput>
    {
        public Transition(TStateId state, TInput input)
        {
            State = state;
            Input = input;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;

            var other = obj as Transition<TStateId, TInput>;
            return other != null && object.Equals(State, other.State) && object.Equals(Input, other.Input);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (State.GetHashCode() * 397) ^ Input.GetHashCode();
            }
        }

        public TStateId State { get; private set; }
        public TInput Input { get; private set; } 
    }
}
