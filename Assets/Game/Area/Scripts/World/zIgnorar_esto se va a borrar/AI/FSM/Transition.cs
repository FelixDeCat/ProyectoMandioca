using System;

namespace IA2
{
	public class Transition<T>
    {

        //¿que me guardo en el Transition?
        //2 cosas nomás

        //* El valor del Input
        T input;
        public T Input { get { return input; } }

        //* El State al cual estoy apuntando
        State<T> targetState;
        public State<T> TargetState { get { return targetState;  } }

        //
        public event Action<T> OnTransition = delegate { };
        public void OnTransitionExecute(T input) { OnTransition(input); }

        // Su constructor
		public Transition(T input, State<T> targetState) {
			this.input = input;
			this.targetState = targetState;
		}
	}
}