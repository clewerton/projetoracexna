using System;

namespace TangoGames.RoadFighter.States
{
    /// <summary>
    /// Representa uma transição de uma máquina de estados. Possui equivalência
    /// de valor, ou seja, duas transições com campos equivalentes são 
    /// consideradas equivalentes.
    /// </summary>
    /// <typeparam name="TState">O tipo dos estados.</typeparam>
    /// <typeparam name="TInput">O tipo das entradas.</typeparam>
    public struct Transition<TState, TInput> : IEquatable<Transition<TState, TInput>>
    {
        public Transition(TState state, TInput input)
        {
            State = state;
            Input = input;
        }

        public override bool Equals(object obj)
        {
            return (obj is Transition<TState, TInput>)
                ? Equals((Transition<TState, TInput>) obj)
                : false;
        }

        public bool Equals(Transition<TState, TInput> other)
        {
            return State.Equals(other.State) && Input.Equals(other.Input);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (State.GetHashCode() * 397) ^ Input.GetHashCode();
            }
        }

        public static bool operator ==(Transition<TState, TInput> left, Transition<TState, TInput> right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Transition<TState, TInput> left, Transition<TState, TInput> right)
        {
            return !left.Equals(right);
        }

        /// <summary>
        /// O estado inicial da transição.
        /// </summary>
        public readonly TState State;

        /// <summary>
        /// A entrada da transição.
        /// </summary>
        public readonly TInput Input;
    }
}
