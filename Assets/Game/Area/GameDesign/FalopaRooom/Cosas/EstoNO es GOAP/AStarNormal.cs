using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using U = Utility;

namespace GOAP
{
    public class AStarNormal<T> where T : class
    {
        public class Arc
        {
            public T endpoint;
            public float cost;
            public Arc(T ep, float c)
            {
                endpoint = ep;
                cost = c;
            }
        }

        public static IEnumerator Run
        (
            T from,
            Func<T, float> heuristic,               //Current, Goal -> Heuristic cost
            Func<T, bool> predicate,                //Current -> Satisfies
            Func<T, IEnumerable<Arc>> explode,      //Current -> (Endpoint, Cost)[]
            Action<IEnumerable<T>> end_callback
        )
        {
            int watchdog = 0;

            var initialState = new AStarState<T>();
            initialState.open.Add(from);
            initialState.costs[from] = 0;
            initialState.fitnesses[from] = heuristic(from);
            initialState.previous[from] = null;
            initialState.current = from;

            var state = initialState;
            while (state.open.Count > 0 && !state.finished)
            {
                //Debugger gets buggy af with this, can't watch variable:
                state = state.Clone();

                var candidate = state.open.OrderBy(x => state.fitnesses[x]).First();
                state.current = candidate;

                //DebugGoap(state);

                if (predicate(candidate))
                {
                    U.Log("SATISFIED");
                    state.finished = true;
                }
                else
                {
                    state.open.Remove(candidate);
                    state.closed.Add(candidate);
                    var neighbours = explode(candidate);
                    if (neighbours == null || !neighbours.Any())
                        continue;

                    var gCandidate = state.costs[candidate];

                    foreach (var ne in neighbours)
                    {

                        if(state.closed.Contains(ne.endpoint))
                            continue;

                        var gNeighbour = gCandidate + ne.cost;
                        state.open.Add(ne.endpoint);

                        if (gNeighbour > state.costs.DefaultGet(ne.endpoint, () => gNeighbour))
                            continue;

                        state.previous[ne.endpoint] = candidate;
                        state.costs[ne.endpoint] = gNeighbour;
                        state.fitnesses[ne.endpoint] = gNeighbour + heuristic(ne.endpoint);
                    }
                }
                watchdog++;//TimeSlicing 1 - Sumo 1 a los nodos que mire

                if (watchdog % 3 == 0)//TimeSlicing 1 - Si los que mire es multiplo de 10 entonces hago una pausa
                {
                    yield return null;//TimeSlicing 1 - Aca se hace una pausa y se retoma al frame siguiente, dejando que el juego siga corriendo
                }
            }

            if (!state.finished)
            {
                end_callback(null);//TimeSlicing 1 - se llama al callback para mandar el path, en este caso no encontro
            }
            else
            {
                //Climb reversed tree.
                var seq =
                    U.Generate(state.current, n => state.previous[n])
                    .TakeWhile(n => n != null)
                    .Reverse();

                end_callback(seq);//TimeSlicing 1 - se llama al callback para mandar el path
            }

        }

        static void DebugGoap(AStarState<T> state)
        {
            var candidate = state.current;
            U.Log("OPEN SET " + state.open.Aggregate("", (a, x) => a + x.ToString() + "\n\n"));
            U.Log("CLOSED SET " + state.closed.Aggregate("", (a, x) => a + x.ToString() + "\n\n"));
            U.Log("CHOSEN CANDIDATE COST " + state.fitnesses[candidate] + ":" + candidate.ToString());
            if (state is AStarState<GoapState>)
            {
                U.Log("SEQUENCE FOR CANDIDATE" +
                    U.Generate(state.current, n => state.previous[n])
                        .TakeWhile(x => x != null)
                        .Reverse()
                        .Select(x => x as GoapState)
                        .Where(x => x != null && x.generatingAction != null)
                        .Aggregate("", (a, x) => a + "-->" + x.generatingAction.Name)
                );

                var prevs = state.previous as Dictionary<GoapState, GoapState>;
                U.Log("Other candidate chains:\n"
                    + prevs
                        .Select(kv => kv.Key)
                        .Where(y => !prevs.ContainsValue(y))
                        .Aggregate("", (a, y) => a +
                            U.Generate(y, n => prevs[n])
                                .TakeWhile(x => x != null)
                                .Reverse()
                                .Select(x => x as GoapState)
                                .Where(x => x != null && x.generatingAction != null)
                                .Aggregate("", (a2, x) => a2 + "-->" + x.generatingAction.Name + "(" + x.step + ")")
                            + " (COST: g" + (state.costs)[y as T] + "   f" + state.fitnesses[y as T] + ")"
                            + "\n"
                        )
                );
            }
        }
    }
}

