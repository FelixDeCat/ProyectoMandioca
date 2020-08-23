using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputSenderBase : MonoBehaviour
{
    public Animator animator;
    public void SendBool(string parameter, bool val) => animator.SetBool(parameter, val);
    public void SendOneShotBool(string parameter, bool val) { animator.SetBool(parameter, val); animator.SetBool(parameter, !val); }
    public void SendTrigger(string parameter) => animator.SetTrigger(parameter);
    public void SendFloat(string parameter, float val) => animator.SetFloat(parameter, val);

    public void Execute(bool val) {

        animator.speed = val ? 1 : 0;
    }

    public void StartStateMachine() => SendBool("On", true);
    public void StopStateMachine() => SendBool("On", false);
    public void Distance(float dist) => SendFloat("distance", dist);
    public void OnHit() => SendTrigger("OnHit");
    public void OnDeath() => SendBool("Death", true);
    public void ExitCombat() => SendBool("InCombat", false);

}
