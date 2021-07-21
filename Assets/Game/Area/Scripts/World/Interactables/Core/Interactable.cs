 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

[SerializeField]
public abstract class Interactable : MonoBehaviour
{
    [Header("--- Interactable Settings ---")]
    public float distancetoInteract = 1f;
    public bool autoexecute;
    bool autoexe_in_CD = false;
    public bool isOneShot = true;
    float timer_cd;
    public float cd_autoexecute;
    public Transform pointToMessage;
    public FeedbackInteractBase[] feedback;
    public bool _withDelay;
    protected float currentTime;
    protected Action _executeAction;
    public float delayTime;
    protected bool updateDelay;
    [SerializeField] bool drawGizmos = false;
    bool can_interact = true;
    public bool CanInteract { get { return can_interact; } }

    public bool IsUninterruptible;

    public UnityEvent UE_OnEnter;
    public UnityEvent UE_OnExit;
    public UnityEvent PressDown;
    public UnityEvent PressUp;
    public Func<bool> predicate = delegate { return true; };
    public void SetPredicate(Func<bool> _pred) => predicate = _pred;
    WalkingEntity currentCollector;

    public bool executing;

    [Header("Auxiliar Interaction for Best Feedback")]
    [SerializeField] AuxiliarInteractEvents auxiliars = null;

    public void Enter(WalkingEntity entity)
    {
        if (!can_interact || !predicate.Invoke())
        {
            auxiliars.Feedback_HOVER_I_Can_NOT_Interact.Invoke();
            return;
        }
        if (!autoexecute) if (feedback.Length > 0) foreach (var fdbck in feedback) fdbck.Show();
        auxiliars.Feedback_HOVER_I_Can_Interact.Invoke();
        OnEnter(entity);
        UE_OnEnter.Invoke();
    }
    public void Exit()
    {
        if (!can_interact) return;
        //if (!predicate.Invoke()) return;
        if (feedback.Length > 0) foreach (var fdbck in feedback) fdbck.Hide();
        OnExit(currentCollector);
        UE_OnExit.Invoke();
        
        PressUp.Invoke();
        currentTime = 0;
        updateDelay = false;
    }

    public void BUTTON_PressUp()
    {
        PressUp.Invoke();
        Debug.Log("PRESS UP");
    }
    public void BUTTON_PressDown()
    {
        PressDown.Invoke();
        Debug.Log("PRESS DOWN");
    }
    public void Execute(WalkingEntity entity)
    {
        if (!can_interact || !predicate.Invoke())
        {
            auxiliars.Feedback_EXECUTE_Fail.Invoke();
            return;
        }
        currentCollector = entity;

        auxiliars.Feedback_EXECUTE_Sucessfull.Invoke();

        if (!_withDelay)
        {
            if (!autoexe_in_CD)
            {
                autoexe_in_CD = true;
                timer_cd = 0;
                if (isOneShot)
                {
                    executing = true;
                    InteractSensor.Remove_Interactable(this);
                    if (feedback.Length > 0) foreach (var fdbck in feedback) fdbck.Hide();
                }
                OnExecute(entity);
            }
        }
        else
        {
            updateDelay = true;
        }
    }

    public void ReturnToCanExecute()
    {
        executing = false;
        InteractSensor.Add_Interactable(this);
    }
    public void SetCanInteract(bool _caninteract) => can_interact = _caninteract;

    public void SetCanInteract(bool _caninteract, bool back_with_timer = false)
    {
        can_interact = _caninteract;
        if (back_with_timer)
        {
            Invoke("CanInteractAgain", 0.1f);
        }
    }
    public void CanNotInteractAgain()
    {
        can_interact = true;
        ReturnToCanExecute();
    }
    public void CanInteractAgain() { 
        can_interact = true;
        ReturnToCanExecute();
    }

    public void CanInteractLoop()
    {
        can_interact = true;
        Invoke("CanInteractAgain", 0.1f);
    }
   
    public void InterruptExecute()
    {
        if (IsUninterruptible) return;
        if (!predicate.Invoke()) return;
        PressUp.Invoke();
        OnInterrupt();
        currentTime = 0;
        updateDelay = false;
    }

    protected virtual void Update()
    {
        if (!can_interact) return; 

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
                executing = true;
                InteractSensor.Remove_Interactable(this);
                if (feedback.Length > 0) foreach (var fdbck in feedback) fdbck.Hide();
                OnExecute(currentCollector);
                currentTime = 0;
                updateDelay = false;
            }
        }
    }

    public abstract void OnEnter(WalkingEntity entity);
    public abstract void OnExecute(WalkingEntity collector);
    public abstract void OnExit(WalkingEntity collector);
    public abstract void OnInterrupt();

    private void OnDrawGizmos()
    {
        if (drawGizmos) Gizmos.DrawWireSphere(transform.position, distancetoInteract);
    }

    public Vector3 Position => transform.position;


    [System.Serializable]
    public class AuxiliarInteractEvents
    {
        public UnityEvent Feedback_HOVER_I_Can_Interact;
        public UnityEvent Feedback_HOVER_I_Can_NOT_Interact;
        public UnityEvent Feedback_EXECUTE_Sucessfull;
        public UnityEvent Feedback_EXECUTE_Fail;
    }
}
