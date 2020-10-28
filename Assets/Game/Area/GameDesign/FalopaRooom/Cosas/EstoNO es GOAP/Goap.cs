using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Collections;

namespace GOAP
{
    public class Goap : MonoBehaviour
    {

        //TimeSlicing 2 - Aca tambien cambie el Execute a que sea un IEnumerator y tenga un callback al finalizar
        //Solo hace un yield al Ienumerator del Run del AStarNormal, es decir que va a esperar a que el AStar termine para saguir
        public static IEnumerator Execute(GoapState from, Func<GoapState, int> to, IEnumerable<GoapAction> actions, Action<IEnumerable<GoapAction>> callback, Action noPathFind_callback)
        {
            int watchdog = 200;


            yield return AStarNormal<GoapState>.Run(
                from,
                (curr) => to(curr),
                curr => to(curr) == 0,
                curr =>
                {
                    if (watchdog == 0)
                    {
                        Debug.Log("ESTO ACA PUEDE SER QUE ENTRE?");
                        return Enumerable.Empty<AStarNormal<GoapState>.Arc>();
                    }
                        
                    else
                        watchdog--;

                    return actions.Where(action => action.preconditions(curr))//curr.worldStateSnap))
                                  .Aggregate(new FList<AStarNormal<GoapState>.Arc>(), (possibleList, action) =>
                                  {
                                      var newState = new GoapState(curr);
                                      action.effects(newState);//(newState.worldStateSnap);
                                      newState.generatingAction = action;
                                      newState.step = curr.step + 1;
                                      return possibleList + new AStarNormal<GoapState>.Arc(newState, action.Cost);
                                  });
                },
                (p) =>
                {
                    Debug.Log("este es el que bvbvusadsadaSFDADWSA  " + p);
                    //TimeSlicing 2 - Este es el callBack final del AStar y lo estoy usando para hacer el callback final del goap
                    if (p != null)
                    {
                        callback?.Invoke(p.Skip(1).Select(x => x.generatingAction));
                    }
                    else
                    {
                        noPathFind_callback?.Invoke();
                    }
                });
        }
    }
}

