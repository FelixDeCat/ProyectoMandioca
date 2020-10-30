using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Villager : NPCBase
{
    [SerializeField] NPC_Anims anim;
    public Rigidbody rb;

    [SerializeField] UnityEvent OnIHaveArrive;

    protected override void StackedInitialize()
    {
        base.StackedInitialize();
        InitializePathFinder(rb);
        Callback_IHaveArrived(IHaveArrive);
    }

    public void GoTo(Vector3 pos)
    {
        canupdate = true;
        anim.StartWalk("");
        GoToPosition(pos);
    }

    void IHaveArrive()
    {
        anim.StopWalk("");
        OnIHaveArrive?.Invoke();
    }
}
