using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaronteSkill_RagingPoolsOfFire : GOAP_Skills_Base
{
    [SerializeField] CustomSpawner fireSpawner = null;
    [SerializeField] FirePools firePools;

    float _count;
    [SerializeField] float duration;


    protected override void OnExecute()
    {
        On();
        firePools.Activate();
        owner.GetComponentInChildren<Animator>().SetTrigger("ragingPools");
    }


    protected override void OnFixedUpdate()
    {

    }

    protected override void OnInitialize()
    {
        firePools = FindObjectOfType<FirePools>();
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
        _count += Time.deltaTime;

        if(_count >= duration)
        {
            _count = 0;
            owner.GetComponentInChildren<Animator>().SetTrigger("finishSkill");
            OnFinishSkill?.Invoke();
            Off();
        }
    }
}
