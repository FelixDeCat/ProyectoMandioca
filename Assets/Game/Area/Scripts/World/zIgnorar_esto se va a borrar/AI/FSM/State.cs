using System;
using System.Collections.Generic;

namespace IA2 {
	public class State<T> {

        // un Nombre, aca no lo usamos, pero por las dudas...
        // en realidad sirve para nosotros mismos a la hora
        // de programar los estados podemos verlo visualmente que es lo que va a hacer
        string _stateName;
        public string Name { get { return _stateName; } }

		public event Action<T> OnEnter = delegate {};
		public event Action OnUpdate = delegate {};
        public event Action OnLateUpdate = delegate { };
        public event Action OnFixedUpdate = delegate { };
		public event Action<T> OnExit = delegate {};

        public void Enter(T input) { OnEnter(input); }
        public void Update() { OnUpdate(); }
        public void LateUpdate() { OnLateUpdate(); }
        public void FixedUpdate() { OnFixedUpdate(); }
        public void Exit(T input) { OnExit(input); }
		

        // Su Constructor
		public State(string name) { _stateName = name; }

        // Me guardo un diccionario de Inputs con su Transicion
        // en el cual...
        // * La Key es el Input
        // * El Value es La transicion que contiene el input y la direccion al estado que apunta
        // Tengo un Configure que lo va a ir rellenando el "StateConfigurer"
        Dictionary<T, Transition<T>> transitions;
        public State<T> Configure(Dictionary<T, Transition<T>> transitions) {
			this.transitions = transitions;
			return this;
		}

        // Esto lo vamos a usar para que desde afuera nosotros 
        // podamos asignarle funciones o Lambdas al Evento "OnTransition"
        public Transition<T> GetTransition(T input) { return transitions[input]; }


        // Esto lo vamos a usar cuando recibamos los Inputs en pleno tiempo de ejecucion
        // recibimos 2 cosas y devolvemos 2
        // recibimos... Un Input y Un State
        // * El Input lo necesitamos para saber si basicamente nuestro diccionario contiene esa Key
        // * El State como es un out lo vamos a rellenar con el State resultante de nuestro diccionario
        // De paso Ejecutamos el Evento "OnTransitionExecute" para que se dispare y podamos hacer algo en el medio
        // extra: si no contiene la Key Me devuelvo a mi mismo acompañado de un Return FALSE, por lo tanto el que me 
        // llama obtiene algo pero sabe que yo no tengo nada para darle, por lo tanto se va a seguir quedando en este estado
        public bool CheckInput(T input, out State<T> next) {
			if(transitions.ContainsKey(input)) {
				var transition = transitions[input];
				transition.OnTransitionExecute(input);
				next = transition.TargetState;
				return true;
			}
			next = this;
			return false;
		}
	}
}