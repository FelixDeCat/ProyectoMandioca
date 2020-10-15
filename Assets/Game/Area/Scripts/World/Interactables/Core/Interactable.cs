using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

[SerializeField]
public abstract class Interactable : MonoBehaviour
{
    [Header("Interactable Settings")]
    public float distancetoInteract = 1f;
    public bool autoexecute;
    bool autoexe_in_CD = false;
    float timer_cd;
    public float cd_autoexecute;
    public Transform pointToMessage;
    public FeedbackInteractBase[] feedback;
    public bool _withDelay;
    protected float currentTime;
    protected Action _executeAction;
    public float delayTime;
    protected bool updateDelay;

    public UnityEvent UE_OnEnter;
    public UnityEvent UE_OnExit;
    public UnityEvent PressDown;
    public UnityEvent PressUp;
    public Func<bool> predicate = delegate { return true; };
    public void SetPredicate(Func<bool> _pred) => predicate = _pred;

    public void Enter(WalkingEntity entity)
    {
        if (!predicate.Invoke()) { return; }
        if (!autoexecute) if (feedback.Length > 0) foreach (var fdbck in feedback) fdbck.Show();
        OnEnter(entity);
        UE_OnEnter.Invoke();
    }
    public void Exit()
    {
        if (!predicate.Invoke()) return;
        if (feedback.Length > 0) foreach (var fdbck in feedback) fdbck.Hide();
        OnExit();
        UE_OnExit.Invoke();
        PressUp.Invoke();
        currentTime = 0;
        updateDelay = false;
    }
    public void Execute(WalkingEntity entity)
    {
        if (!predicate.Invoke()) return;

        PressDown.Invoke();

        if (!_withDelay)
        {
            if (!autoexe_in_CD)
            {
                autoexe_in_CD = true;
                timer_cd = 0;
                OnExecute(entity);
            }
        }
        else
        {
            updateDelay = true;
        }
    }
    public void InterruptExecute()
    {
        if (!predicate.Invoke()) return;
        PressUp.Invoke();
        OnInterrupt();
        currentTime = 0;
        updateDelay = false;
    }

    protected virtual void Update()
    {
        if (updateDelay)
        {
            DelayExecute();
        }

        if (autoexe_in_CD)
        {
            if (timer_cd < cd_autoexecute)
            {
                timer_cd = timer_cd + 1 * Time.deltaTime;
            }
            else
            {
                timer_cd = 0;
                autoexe_in_CD = false;

            }
        }
    }
    public virtual void DelayExecute()
    {
        if (updateDelay)
        {
            if (currentTime < delayTime)
            {
                currentTime = currentTime + 1 * Time.deltaTime;
            }
            else
            {
                OnExecute(Main.instance.GetChar());
                currentTime = 0;
                updateDelay = false;
            }
        }
    }
    public abstract void OnEnter(WalkingEntity entity);
    public abstract void OnExecute(WalkingEntity collector);
    public abstract void OnExit();
    public abstract void OnInterrupt();

}
