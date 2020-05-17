using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachineHandler : MonoBehaviour
{
    //con todas las state machines
    public Animator current_animator;
    public Transform parentStates;

    StateLinkerBehaviour[] linkersCaptured = new StateLinkerBehaviour[0];

    internal void Initialize(SensorsAndBehaviours sens_and_beha, FastSubscriberPerState fastSubscriber)
    {
        linkersCaptured = current_animator.GetBehaviours<StateLinkerBehaviour>();
        foreach (var s in parentStates.GetComponentsInChildren<MonoStateBase>())
        {
            s.Configure(sens_and_beha, fastSubscriber);
            LinkState(s);
        }
    }

    public void LinkState(MonoStateBase stateBase)
    {
        for (int i = 0; i < linkersCaptured.Length; i++)
        {
            if (linkersCaptured[i].linker == stateBase._monoStateBaseOptions.linker)
            {

                linkersCaptured[i].Configure(stateBase.Begin, stateBase.Exit, stateBase.Refresh);
            }
        }

    }

    public void BeginBoss() => current_animator.SetTrigger("start");
    public void Distance(float dist) => current_animator.SetFloat("distance", dist);
}
