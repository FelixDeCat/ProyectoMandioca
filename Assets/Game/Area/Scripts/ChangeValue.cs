using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeValue : StateMachineBehaviour
{
    [Header("parameter")]
    public string parameterName;

    [Header("type")]
    public bool isFloat;
    public bool isInt;
    public bool isTrigger;
    public bool isBoolean;

    public bool useRestTrigger;

    [Header("values")]
    public bool boolean_value;
    public float float_value;
    public int int_value;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (isBoolean)
        {
            animator.SetBool(parameterName, boolean_value);
        }
        else if (isTrigger)
        {
            if (useRestTrigger)
            {
                animator.ResetTrigger(parameterName);
            }
            else
            {
                animator.SetTrigger(parameterName);
            }
        }
        else if (isInt)
        {
            animator.SetInteger(parameterName, int_value);
        }
        else if (isFloat)
        {
            animator.SetFloat(parameterName, float_value);
        }
    }
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (isBoolean)
        {
            animator.SetBool(parameterName, boolean_value);
        }
    }
}
