using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;

public class LaserShoot_bossSkill : GOAP_Skills_Base
{
    [SerializeField] Throwable rayo_pf;
    [SerializeField] Transform rayoOrigin;

    protected override void OnEndSkill()
    {
        owner.GetComponent<Ente>().OnSkillAction -= ShootLaser;
    }

    protected override void OnExecute()
    {
        owner.GetComponentInChildren<Animator>().Play("LaserShoot");

        owner.GetComponent<Ente>().OnSkillAction += ShootLaser;

    }

    void ShootLaser()
    {
        ThrowData newData = new ThrowData().Configure(rayoOrigin.position, ((heroRoot.position + Vector3.up) - rayoOrigin.position).normalized, 13, 10, owner);

        ThrowablePoolsManager.instance.Throw(rayo_pf.name, newData);

        EndSkill();
    }

    protected override void OnFixedUpdate()
    {
     
    }

    protected override void OnInitialize()
    {
        ThrowablePoolsManager.instance.CreateAPool(rayo_pf.name, rayo_pf);
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
