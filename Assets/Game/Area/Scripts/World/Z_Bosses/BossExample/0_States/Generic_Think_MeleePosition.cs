using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools.Extensions;

public class Generic_Think_MeleePosition : MonoStateBase
{
    [Range(0,1)]
    public float prob_to_Melee;

    public bool UseCombatDirector;

    protected override void OnBegin()
    {
        Get_Behaviours.followBehaviour.StartLookAt();

        var aux = Random.value;
        if (aux <= prob_to_Melee)
        {
            if (UseCombatDirector)
            {
                Get_Behaviours.combatDirectorComponent.EnterCombat();
                Get_Behaviours.combatDirectorComponent.IAmReady();
            }
            else
            {
                OnAttack();
            }
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
