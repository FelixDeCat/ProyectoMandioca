using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;

public class CaronteSkill_RagingPoolsOfFire : GOAP_Skills_Base
{
    //[SerializeField] CustomSpawner fireSpawner = null;
    [SerializeField] FirePools firePools;

    float _timer;
    [SerializeField] float duration;


    protected override void OnExecute()
    {
        //On();
        
        owner.GetComponentInChildren<Animator>().SetTrigger("ragingPools");
        owner.GetComponent<Ente>().OnSkillAction += ActivateFirepools;
        //isAvaliable = false;
        
    }

    protected override void OnEndSkill()
    {

        owner.GetComponent<Ente>().OnSkillAction -= ActivateFirepools;
    }

    void ActivateFirepools()
    {
        firePools.Activate();
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
        //_timer += Time.deltaTime;

        //if(_timer >= duration)
        //{
        //    _timer = 0;
        //    //owner.GetComponentInChildren<Animator>().SetTrigger("finishSkill");
        //    OnFinishSkill?.Invoke();
        //    Off();
        //}
    }

    protected override void OnInterruptSkill()
    {
        throw new System.NotImplementedException();
    }
}
