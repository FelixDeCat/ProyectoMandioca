using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;

public class CaronteSkill_FlameThrower : GOAP_Skills_Base
{
    

    protected override void OnEndSkill()
    {
        
    }

    protected override void OnExecute()
    {
        owner.GetComponentInChildren<Animator>().SetTrigger("flameThrower");
        owner.GetComponent<Ente>().OnSkillAction += ActivateFlames;
    }

    void ActivateFlames()
    {
        Debug.Log("LLAMAS A MIIII");
    }

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

    protected override void OnUpdate()
    {
        
    }

}
