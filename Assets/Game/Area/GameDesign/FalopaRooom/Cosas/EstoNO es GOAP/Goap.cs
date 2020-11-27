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

                //from
                from,

                //heuristic
                (curr) => to(curr),

                //predicate
                curr => to(curr) == 0,

                //explode
                curr =>
                {
                    if (watchdog == 0)
                    {
                        return Enumerable.Empty<AStarNormal<GoapState>.Arc>();
                    }

                    else
                        watchdog--;

                    return actions
                                    //filtro todas las goap actions por las que cumplen las pre condiciones
                                    .Where(goap_action => goap_action.preconditions(curr))


                                    .Aggregate(new FList<AStarNormal<GoapState>.Arc>(), (possibleList, action) =>
                                    {
                                        var newState = new GoapState(curr);//creo una copia de mi general goap state
                                        action.effects(newState); //le voy agregando los efectos del current state al general
                                        newState.generatingAction = action;
                                        newState.step = curr.step + 1;
                                        return possibleList + new AStarNormal<GoapState>.Arc(newState, action.Cost);
                                    });
                },

                //EndCallback
                (p) =>
                {
                    if (p != null)
                    {
                        if (callback != null)
                        {
                            callback.Invoke(p.Skip(1).Select(x => x.generatingAction));
                        }
                    }
                    else
                    {
                        noPathFind_callback?.Invoke();
                    }
                });
        }
    }
}

