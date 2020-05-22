using System;
using UnityEngine;
namespace IA2
{
    public class EventFSM<T>
    {
        event Action<string> debug = delegate { };

        private State<T> current;
        public EventFSM(State<T> initial)
        {
            current = initial;
            current.Enter(default(T));
        }
        public EventFSM(State<T> initial, Action<string> deb)
        {
            debug += deb;
            current = initial;
            current.Enter(default(T));
            debug(current.Name);
        }
        public void SendInput(T input)
        {
            State<T> newState;
            if (current.CheckInput(input, out newState))
            {
                current.Exit(input);
                current = newState;
                current.Enter(input);
                debug(current.Name);
            }
        }

        public State<T> Current { get { return current; } }
        public void Update() { current.Update(); }
        public void LateUpdate() { current.LateUpdate(); }
        public void FixedUpdate() { current.FixedUpdate(); }

    }
}
