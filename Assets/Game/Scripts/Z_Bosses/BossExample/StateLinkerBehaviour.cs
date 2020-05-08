using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using System;


public class StateLinkerBehaviour : StateMachineBehaviour
{
    public LabelStatesLinkType linker;

    Action update;
    Action enter;
    Action exit; 

    public void Configure(Action _enter, Action _exit, Action _update)
    {
        enter = _enter;
        exit = _exit;
        update = _update;
    }

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex, controller);
        Debug.Log(controller.GetParameterCount());

        enter.Invoke();
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller)
    {
        base.OnStateExit(animator, stateInfo, layerIndex, controller);

        exit.Invoke();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex, controller);

        update.Invoke();

        //esto es para preguntar si el nombre de la transicion coincide
        // controller.GetAnimatorTransitionInfo(layerIndex).IsUserName()


    }
}
