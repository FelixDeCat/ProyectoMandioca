using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Villager : NPCBase
{
    [SerializeField] NPC_Anims anim;
    public Rigidbody rb;

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
    }
}
