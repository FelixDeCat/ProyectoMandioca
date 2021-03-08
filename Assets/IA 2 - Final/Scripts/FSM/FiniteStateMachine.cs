using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IA2Final.FSM {

    public class FiniteStateMachine {

        private const int _MAX_TRANSITIONS_PER_FRAME = 3;

        public delegate void StateEvent(IState from, IState to);

        public event Action OnActive;
        public event Action OnUnActive;

        public IState CurrentState { get; private set; }
        private List<IState> _allStates;

        private Func<IEnumerator, Coroutine> _startCoroutine;

        private bool _isActive;

        public FiniteStateMachine(IState initialState, Func<IEnumerator, Coroutine> startCoroutine){
            CurrentState = initialState.Configure(this);
            _allStates = new List<IState>{CurrentState};

            _startCoroutine = startCoroutine;
        }

        public IEnumerator Update() {
            while (Active) {
                CurrentState.UpdateLoop();
                
                var nextState = CurrentState.ProcessInput();
                var stateTransitions = 0;
                
                while (nextState != CurrentState && stateTransitions < _MAX_TRANSITIONS_PER_FRAME) {
                    var previousState = CurrentState;
                    var transitionParameters = CurrentState.Exit(nextState);

                    Debug.Log("Exiting state '" + CurrentState.Name + "' to state '" + nextState.Name + "'.");
                    
                    CurrentState = nextState;
                    CurrentState.Enter(previousState, transitionParameters);

                    nextState = CurrentState.ProcessInput();

                    stateTransitions++;
                }
                
                yield return null;
            }
        }

        public FiniteStateMachine AddTransition(string transitionName, IState from, IState to) {
            from.Configure(this);
            to.Configure(this);
            
            if (from.Transitions == null) 
                from.Transitions = new Dictionary<string, IState>();

            if (!from.Transitions.ContainsKey(transitionName)) {
                from.Transitions.Add(transitionName, to);
                if (!_allStates.Contains(from)) _allStates.Add(from);
                if (!_allStates.Contains(to))   _allStates.Add(to);
            }

            return this;
        }

        public FiniteStateMachine Clear() {
            foreach (var state in _allStates) {
                state.Transitions = new Dictionary<string, IState>();
                state.HasStarted  = false;
            }

            return this;
        }
        
        public bool Active {
            get { return _isActive; }
            set {
                if (_isActive == value) return;
                _isActive = value;
                if (_isActive) {
                    if (!CurrentState.HasStarted) CurrentState.Enter(CurrentState, null);
                    _startCoroutine(Update());
                    OnActive?.Invoke();
                }
                else
                    OnUnActive?.Invoke();
            }
        }


    }
}