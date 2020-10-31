using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunningFromVillage_npcEvent : Villager
{
    public Transform point;

    protected override void OnFixedUpdate()
    {

    }

    protected override void OnInitialize()
    {

    }

    protected override void OnPause()
    {

    }

    protected override void OnResume()
    {

    }

    protected override void OnTurnOff()
    {

    }

    protected override void OnTurnOn()
    {

    }

    protected override void OnUpdateEntity()
    {

    }

    private void Start()
    {
        
    }

    

    public void GoTo_Spot() { GoTo(point.position); Debug.Log("asdasdsa");}


}
