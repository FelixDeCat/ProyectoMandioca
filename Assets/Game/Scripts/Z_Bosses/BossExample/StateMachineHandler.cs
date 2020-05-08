using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachineHandler : MonoBehaviour
{
    //con todas las state machines
    public Animator current_animator;
    public Transform parentStates;

    StateLinkerBehaviour[] linkersCaptured = new StateLinkerBehaviour[0];

    private void Awake()
    {
        linkersCaptured = current_animator.GetBehaviours<StateLinkerBehaviour>();

        foreach (var s in parentStates.GetComponentsInChildren<MonoStateBase>())
        {
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


    public void Distance(float dist)
    {
        current_animator.SetFloat("distance",dist);
    }
}
