using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools.Extensions;

public class Generic_Think_MeleePosition : MonoStateBase
{


    protected override void OnBegin()
    {
        Get_Behaviours.followBehaviour.StartLookAt();

        var aux = Random.value;
        if (aux <= 0.3)
        {
            Get_Behaviours.combatDirectorComponent.EnterCombat();
            Get_Behaviours.combatDirectorComponent.IAmReady(OnAttack);
        }
        else
        {
            Get_InputSender.SendBool("CloseFight", false);
        }
    }
    public void OnAttack()
    {
        if (_monoStateBaseOptions.isactive)
            Get_InputSender.SendBool("CloseFight", true);
    }
    protected override void OnExit()
    {
        Get_Behaviours.followBehaviour.StopLookAt();

        Get_Behaviours.combatDirectorComponent.ExitCombat();
        Get_Behaviours.combatDirectorComponent.IAmNotReady();
    }

    protected override void OnOneAwake() { }
    protected override void OnUpdate() { }
}
