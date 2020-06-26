using System.Collections.Generic;

namespace Tools.StateMachine
{
    public class ConfigureState<T>
    {
		EState<T> _state;

		Dictionary<T, TransitionState<T>> transitions = new Dictionary<T, TransitionState<T>>();

		public ConfigureState(EState<T> state)
        {
            _state = state;
        }

        public ConfigureState<T> SetTransition(T input, EState<T> target)
        {
            transitions.Add(input, new TransitionState<T>(input, target));
            return this;
        }

		public void Done()
        {
            _state.Configure(transitions);
        }
    }

    public static class ConfigureState
    {
        public static ConfigureState<T> Create<T>(EState<T> state)
        {
            return new ConfigureState<T>(state);
        }
    }
}
