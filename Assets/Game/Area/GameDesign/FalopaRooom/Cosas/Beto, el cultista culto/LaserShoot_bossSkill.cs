using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;

public class LaserShoot_bossSkill : GOAP_Skills_Base
{
    [SerializeField] Throwable rayo_pf;
    [SerializeField] Transform rayoOrigin;

    [SerializeField] int amountLaser;
    [SerializeField] int _amount;
    [SerializeField] LayerMask _mask;
    Ente _ent;

    [SerializeField] PlayObject corruptVomito_pf;
 
    [SerializeField] float timeBwtShoots;

    IEnumerator ametralladora;

    protected override void OnEndSkill()
    {
        //proba frenar el animator tambien
        //owner.GetComponentInChildren<Animator>().Play("Idle");
        StopCoroutine(ametralladora);
        owner.GetComponent<Ente>().OnSkillAction -= ShootLaser;
        
    }

    protected override void OnExecute()
    {
        _amount = 0;


        owner.GetComponent<Ente>().OnSkillAction += ShootLaser;


        StartCoroutine(ametralladora);
    }

    IEnumerator Ametralladora()
    {
        while(_amount < amountLaser)
        {
            _amount++;
            owner.GetComponentInChildren<Animator>().Play("LaserShoot");
            yield return new WaitForSeconds(timeBwtShoots);
        }

        EndSkill();
    }

    void ShootLaser()
    {
        ThrowData newData;

        if (_amount == amountLaser)
        {
            newData = new ThrowData().Configure(rayoOrigin.position + _ent.Root().forward * .5f, ((heroRoot.position + Vector3.up + heroRoot.forward * 4) - rayoOrigin.position).normalized, 13, 10, owner, SummonSlowGoo);
        }
        else
        {
            newData = new ThrowData().Configure(rayoOrigin.position + _ent.Root().forward * .5f, ((heroRoot.position + Vector3.up) - rayoOrigin.position).normalized, 13, 10, owner);
        }
        
        ThrowablePoolsManager.instance.Throw(rayo_pf.name, newData);
        
    }

    void SummonSlowGoo(Vector3 summonPos)
    {
        var pool = PoolManager.instance.GetObjectPool(corruptVomito_pf.name);
        var trap = pool.GetPlayObject(3);
        trap.transform.position = GetSurfacePos(summonPos, _mask);
    }

    protected override void OnFixedUpdate()
    {
     
    }

    public Vector3 GetSurfacePos(Vector3 pos, LayerMask mask)
    {
        RaycastHit hit;

        if (Physics.Raycast(pos, Vector3.down, out hit, Mathf.Infinity, mask, QueryTriggerInteraction.Ignore))
            pos = hit.point;

        return pos;
    }

    protected override void OnInitialize()
    {
        _ent = owner.GetComponent<Ente>();

        ametralladora = Ametralladora();
        ThrowablePoolsManager.instance.CreateAPool(rayo_pf.name, rayo_pf);
        owner.GetComponent<Ente>().OnTakeDmg += InterruptSkill;
        
        PoolManager.instance.GetObjectPool(corruptVomito_pf.name, corruptVomito_pf);
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

    protected override void OnInterruptSkill()
    {
        //StopCoroutine(Ametralladora());
    }
}
