using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DevelopTools;

public class ShootEvent : StateMachineBehaviour
{
    public bool EXECUTE_OnEnter;
    public bool EXECUTE_OnExit;
    public string parameterOnEnter;
    public string parameterOnExit;
    private const string FUNC_NAME = "EVENT_Callback";
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) => 
        Main.instance.eventManager.TriggerEvent(FUNC_NAME, parameterOnEnter);
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) => 
        Main.instance.eventManager.TriggerEvent(FUNC_NAME, parameterOnExit);

    #region en desuso
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{

    //}
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
    #endregion
}
