using System.Collections.Generic;

namespace IA2 {
	public class StateConfigurer<T>
    {

        /// <summary> aca me guardo una referencia del State que tengo que rellenarle con transiciones </summary>
		State<T> _state;

        /// <summary>
        /// 
        /// </summary>
		Dictionary<T, Transition<T>> transitions = new Dictionary<T, Transition<T>>();

        /// <summary> Su constructor que va a ser llamado por la extension de mas abajo </summary>
        /// <param name="state"> Recibe un State<T> para guardarlo como referencia y luego poder modificarlo mas adelante </param>
		public StateConfigurer(State<T> state) {
			_state = state;
		}

        /// <summary> Recibe dos parametros para construir una nueva transicion y guardarlo en nuestro diccionario
        /// luego me devuelvo a mi mismo para que desde afuera pueda encadenar varios "SetTransition" </summary>
        /// <param name="input"> input que lo usamos como "KEY" de nuestro diccionario de transiciones y para crear una nueva transicion para guardarlo como "VALUE" en nuestro dicciconario </param>
        /// <param name="target"> target que es un "State" que tambien lo usamos para crear la transicion que vamos a guardar como "VALUE" en nuestro diccionario </param>
        /// <returns> me devuelvo a mi mismo para que desde afuera pueda encadenar varios "SetTransition(T, State<T>)" como si fuera una linea de LINQ</returns>
        public StateConfigurer<T> SetTransition(T input, State<T> target) {
			transitions.Add(input, new Transition<T>(input, target));
			return this;
		}

        /// <summary> recordar siempre cerrar la cadena para que se guarden las transiciones en el diccionario del estado que estoy modificando </summary>
		public void Done() {
			_state.Configure(transitions);
		}
	}

	public static class StateConfigurer
    {
        // funcion estatica que crea y devuelve un StateConfigurer
        // es mas comodo para no tener que hacer por ejemplo
        // StateConfigurer<tipo> stateconf = new StateConfigurer<tipo>();
        // en cambio haces StateConfigurer.Create(value); y ya esta
        public static StateConfigurer<T> Create<T>(State<T> state)
        {
			return new StateConfigurer<T>(state);
		}
	}
}