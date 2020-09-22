using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaronteSkill_RagingPoolsOfFire : GOAP_Skills_Base
{
    [SerializeField] CustomSpawner fireSpawner;

    protected override void OnExecute()
    {
        
        fireSpawner.ActivateSpawner();
        owner.GetComponentInChildren<Animator>().SetTrigger("ragingPools");
        
        
    }


    protected override void OnFixedUpdate()
    {

    }

    protected override void OnInitialize()
    {
        OnFinishSkill += () => { fireSpawner.StopSpawner(); fireSpawner.ResetSpawner(); };
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

    protected override void OnUpdate()
    {

    }
}
