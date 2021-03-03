using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using IA2Final.FSM;
using UnityEngine;

namespace IA2Final
{
    public class GoapPlanner
    {

        public event Action<IEnumerable<GOAPAction>> OnPlanCompleted;
        public event Action OnCantPlan;

        private const int _WATCHDOG_MAX = 200;

        private int _watchdog;

        public void Run(GOAPState from, GOAPState to, IEnumerable<GOAPAction> actions,
                        Func<IEnumerator, Coroutine> startCoroutine)
        {
            _watchdog = _WATCHDOG_MAX;

            var astar = new AStar<GOAPState>();
            astar.OnPathCompleted += OnPathCompleted;
            astar.OnCantCalculate += OnCantCalculate;

            var astarEnumerator = astar.Run(from,
                                            state => Satisfies(state, to),
                                            node => Explode(node, actions, ref _watchdog),
                                            state => GetHeuristic(state, to));

            startCoroutine(astarEnumerator);
        }

        public static FiniteStateMachine ConfigureFSM(IEnumerable<GOAPAction> plan, Func<IEnumerator, Coroutine> startCoroutine)
        {
            var prevState = plan.First().linkedState;
            foreach(var action in plan) action.linkedState.Transitions = new Dictionary<string, IState>();

            var fsm = new FiniteStateMachine(prevState, startCoroutine);

            foreach (var action in plan.Skip(1))
            {
                if (prevState == action.linkedState) continue;
                fsm.AddTransition("On" + action.linkedState.Name, prevState, action.linkedState);

                prevState = action.linkedState;
            }

            return fsm;
        }

        private void OnPathCompleted(IEnumerable<GOAPState> sequence)
        {
            foreach (var act in sequence.Skip(1))
            {
                Debug.Log(act);
            }

            Debug.Log("WATCHDOG " + _watchdog);

            var plan = sequence.Skip(1).Select(x => x.generatingAction);

            OnPlanCompleted?.Invoke(plan);
        }

        private void OnCantCalculate()
        {
            OnCantPlan?.Invoke();
        }

        private static float GetHeuristic(GOAPState from, GOAPState goal, int seed = 0, float seedF = 0) =>
            goal.values.boolValues.Count(kv => !kv.In(from.values.boolValues)) +
            goal.values.intValues.Aggregate(seed, (s, g) => { s += Mathf.Abs(from.values.intValues[g.Key] - g.Value); return s; }) +
            goal.values.floatValues.Aggregate(seedF, (s, g) => { s += Mathf.Abs(from.values.floatValues[g.Key] - g.Value); return s; }) +
            goal.values.stringValues.Count(kv => !kv.In(from.values.stringValues));
        private static bool Satisfies(GOAPState state, GOAPState to) => to.values.boolValues.All(kv => kv.In(state.values.boolValues)) &&
            to.values.intValues.All(kv => kv.In(state.values.intValues)) && to.values.floatValues.All(kv => kv.In(state.values.floatValues)) &&
            to.values.stringValues.All(kv => kv.In(state.values.stringValues));

        private static IEnumerable<WeightedNode<GOAPState>> Explode(GOAPState node, IEnumerable<GOAPAction> actions,
                                                                    ref int watchdog)
        {
            if (watchdog == 0) return Enumerable.Empty<WeightedNode<GOAPState>>();
            watchdog--;

            return actions.Where(action => action.preconditions.All(x=> x.Invoke(node.values)))
                          .Aggregate(new List<WeightedNode<GOAPState>>(), (possibleList, action) =>
                          {
                              var newState = new GOAPState(node);
                              for (int i = 0; i < action.effects.Count; i++) action.effects[i](newState.values);
                              newState.generatingAction = action;
                              newState.step = node.step + 1;

                              possibleList.Add(new WeightedNode<GOAPState>(newState, action.cost));
                              return possibleList;
                          });
        }
    }
}
