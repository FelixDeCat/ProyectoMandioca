using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generic_Think_RangePosition : MonoStateBase
{

    public bool UseCombatDirector;

    protected override void OnBegin()
    {
        Get_Behaviours.followBehaviour.StartLookAt();

        if (UseCombatDirector)
        {
            Get_Behaviours.combatDirectorComponent.EnterCombat(Main.instance.GetChar().transform);
            Get_Behaviours.combatDirectorComponent.IAmReady();
        }
        else
        {
            OnAttack();
        }
    }
    public void OnAttack()
    {
        if (_monoStateBaseOptions.isactive) 
        {
            Get_InputSender.SendBool("CombatDirAdvice", true);
        }
            
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
