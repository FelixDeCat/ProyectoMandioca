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
    [SerializeField] UnityEvent OnIHaveArrive;

    protected override void StackedInitialize()
    {
        interactable = GetComponent<NPC_Interactable>();
        base.StackedInitialize();
        InitializePathFinder(rb);
        Callback_IHaveArrived(IHaveArrive);
    }

    public void NPC_Can_Interact(bool canInteract) => interactable.SetCanInteract(canInteract);

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

    void IHaveArrive()
    {
        anim.StopWalk("");
        OnIHaveArrive?.Invoke();
        OnArriveCustom?.Invoke();
    }
}
