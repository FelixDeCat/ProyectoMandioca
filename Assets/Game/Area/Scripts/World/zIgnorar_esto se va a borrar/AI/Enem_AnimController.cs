using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enem_AnimController : AnimControllerBase
{
   public Animator animator;

    #region local callbacks
    Action callback_OnSightEnded;
    Action callback_AnticipationEnded;
    Action callback_Alert;
    Action callback_Death;
    
    #endregion

    #region ANIM_SCRIPTS
    ANIM_SCRIPT_OnSight onsight;
    ANIM_SCRIPT_Anticipation anticipation;
    ANIM_SCRIPT_Alert alert;
    ANIM_SCRIPT_OnDeath death;
    #endregion

    private void Awake()
    {
        animator = GetComponent<Animator>();
        onsight = animator.GetBehaviour<ANIM_SCRIPT_OnSight>();
        anticipation = animator.GetBehaviour<ANIM_SCRIPT_Anticipation>();
        alert = animator.GetBehaviour<ANIM_SCRIPT_Alert>();
        death = animator.GetBehaviour<ANIM_SCRIPT_OnDeath>();

        onsight.ConfigureCallback(OnSightEnded);
        anticipation.ConfigureCallback(OnAnticipationEnded);
        alert.ConfigureCallback(OnAlertEnded);
        death.ConfigureCallback(OndeathEnded);
    }

    void OnSightEnded()
    {
        callback_OnSightEnded.Invoke();
    }
    void OnAnticipationEnded()
    {
        callback_AnticipationEnded.Invoke();
    }
    void OnAlertEnded()
    {
        callback_Alert.Invoke();
    }
    void OndeathEnded()
    {
        callback_Death.Invoke();
    }

    public string Idle_name = "Enemy_Idle";
    public string OnSight_name = "Enemy_OnSight";
    public string Anticipation_name = "Enemy_Anticipation";
    public string Alert_name = "Enemy_Alert";
    public string Pursuit_name = "Enemy_Pursuit";
    public string Death_name = "Enemy_Death";
    public string AttackPose_name = "Enemy_AttackPose";

    #region ToPlay
    public void PlayIdle() => animator.Play(Idle_name);
    public void PlayOnSight() => animator.Play(OnSight_name);
    public void PlayAnticipation() => animator.Play(Anticipation_name);
    public void PlayAlert() => animator.Play(Alert_name);
    public void PlayPursuit() => animator.Play(Pursuit_name);
    public void PlayDeath() => animator.Play(Death_name);
    #endregion

    #region to subscribe
    public void Subscribe_OnSight_Ended(Action _OnSightEnded)
    {
        callback_OnSightEnded = _OnSightEnded;
    }
    public void Subscribe_OnAnticipationEnded(Action _OnAnticipationEnded)
    {
        callback_AnticipationEnded = _OnAnticipationEnded;
    }
    public void Subscribe_OnAlertEnded(Action _OnAlertEnded)
    {
        callback_Alert = _OnAlertEnded;
    }
    public void Subscribe_OnDeathEnded(Action _OnDeathEnded)
    {
        callback_Death = _OnDeathEnded;
    }
    #endregion
}
