using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Villager : NPCBase
{
    [Header("Villager Variables")]
    [SerializeField] protected NPC_Anims anim;
    public Rigidbody rb;
    Action OnArriveCustom = delegate { };
    NPC_Interactable interactable;

    [SerializeField] UnityEvent OnStartMovement;
    public UnityEvent OnIHaveArrive;

    protected Action onArrivedEvent;

    protected override void StackedInitialize()
    {
        interactable = GetComponent<NPC_Interactable>();
        base.StackedInitialize();
        InitializePathFinder(rb);
        Callback_IHaveArrived(IHaveArrive);
    }

    public void NPC_Can_Interact(bool canInteract) => interactable.SetCanInteract(canInteract);
    public void NPC_Can_Interact_Auto_True() => interactable.SetCanInteract(false, true);

    public void GoTo(Vector3 pos, Action _OnArriveCustomCallback = null)
    {
        OnStartMovement.Invoke();
        OnArriveCustom = _OnArriveCustomCallback;
        canupdate = true;
        anim.StartWalk("");
        GoToPosition(pos);
    }

    public void GoToNoAnim(Vector3 pos, Action _OnArriveCustomCallback = null)
    {
        OnStartMovement.Invoke();
        OnArriveCustom = _OnArriveCustomCallback;
        canupdate = true;
        GoToPosition(pos);
    }

    public void GoToCustomAnim(Vector3 pos, string animation,Action _OnArriveCustomCallback = null)
    {
        OnStartMovement.Invoke();
        OnArriveCustom = _OnArriveCustomCallback;
        canupdate = true;
        anim.PlayAnimation(animation);
        GoToPosition(pos);
    }

    public void StopMoving()
    {
        Stop();
    }

    void IHaveArrive()
    {
        anim.StopWalk("");
        OnIHaveArrive?.Invoke();
        OnArriveCustom?.Invoke();
        if(onArrivedEvent != null) onArrivedEvent.Invoke();
    }
}
