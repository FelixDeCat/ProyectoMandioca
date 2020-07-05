using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorButton : Interactable
{
    [SerializeField] Elevator _myElevator;
    public override void OnEnter(WalkingEntity entity)
    {
        WorldItemInfo.instance.Show(transform.position, "elevator", "start/stop elevator");
    }

    public override void OnExecute(WalkingEntity collector)
    {
        _myElevator.activate(true);
    }

    public override void OnExit()
    {
        WorldItemInfo.instance.Hide();
    }

   
}
