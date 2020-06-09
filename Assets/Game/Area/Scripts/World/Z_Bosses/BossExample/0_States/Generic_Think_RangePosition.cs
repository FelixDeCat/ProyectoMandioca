using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generic_Think_RangePosition : MonoStateBase
{
    protected override void OnBegin()
    {
        Get_Behaviours.followBehaviour.StartLookAt();

        Get_Behaviours.combatDirectorComponent.EnterCombat();
        Get_Behaviours.combatDirectorComponent.IAmReady(OnAttack);
    }
    public void OnAttack()
    {
        if (isactive)
            Get_InputSender.SendBool("CombatDirAdvice", true);
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
