using System.Collections.Generic;
using System;

namespace Tools.StateMachine
{
	public class EventStateMachine<T>
	{
		private EState<T> current;
		Action<string> debug = delegate { };
		public EventStateMachine(EState<T> initial, Action<string> _debug)
		{
			debug = _debug;
			current = initial;
			current.Enter(null);
		}
		public void SendInput(T input)
		{
			EState<T> newState;
			if (current.CheckInput(input, out newState))
			{
				current.Exit(input);
				var oldState = current;
				current = newState;
				debug(current.Name);
				current.Enter(oldState);
			}
		}

		public EState<T> Current { get { return current; } }
		public void Update() { current.Update(); }
		public void LateUpdate() { current.LateUpdate(); }
		public void FixedUpdate() { current.FixedUpdate(); }

	}
}
