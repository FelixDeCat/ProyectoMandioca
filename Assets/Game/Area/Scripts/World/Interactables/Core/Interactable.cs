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
    public Transform pointToMessage;
    public FeedbackInteractBase[] feedback;
    public bool _withDelay;
    protected float currentTime;
    protected Action _executeAction;
    public float delayTime;
    protected bool updateDelay;
    public UnityEvent UE_OnEnter;
    public UnityEvent UE_OnExit;
    public Func<bool> predicate = delegate { return true; };
    public void SetPredicate(Func<bool> _pred) => predicate = _pred;

    public void Enter(WalkingEntity entity)
    {
        if (!predicate.Invoke()) return;

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
        currentTime = 0;
        updateDelay = false;
    }
    public void Execute(WalkingEntity entity)
    {
        if (!predicate.Invoke()) return;

        if (!_withDelay)
            OnExecute(entity);
        else
            updateDelay = true;
    }
    protected virtual void Update()
    {
        if (updateDelay)
        {
            DelayExecute(delayTime);
        }
    }
    public virtual void DelayExecute(float loadTime)
    {
        currentTime += Time.deltaTime;

        if (loadTime <= currentTime)
        {
            OnExecute(Main.instance.GetChar());
        }
    }
    public abstract void OnEnter(WalkingEntity entity);
    public abstract void OnExecute(WalkingEntity collector);
    public abstract void OnExit();



}
