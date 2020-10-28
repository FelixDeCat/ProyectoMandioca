using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;

public class LaserShoot_bossSkill : GOAP_Skills_Base
{
    [SerializeField] Throwable rayo_pf = null;
    [SerializeField] Transform rayoOrigin = null;

    [SerializeField] int amountLaser = 9;
    [SerializeField] int _amount = 5;
    [SerializeField] LayerMask _mask = 0;
    Ente _ent;
    Animator _anim;
    [SerializeField] PlayObject corruptVomito_pf = null;
 
    [SerializeField] float timeBwtShoots = 0.5f;

    IEnumerator ametralladora;

    protected override void OnEndSkill()
    {
        //proba frenar el animator tambien
        //owner.GetComponentInChildren<Animator>().Play("Idle");
        //Debug.Log("FRENO EL LASERSHOOOOT");
        StopCoroutine(ametralladora);
        _ent.OnSkillAction -= ShootLaser;
        _ent.OnTakeDmg -= InterruptSkill;

        _anim.SetTrigger("finishSkill");
    }

    protected override void OnExecute()
    {
        //Debug.Log("INICIO LOS TIROS");
        _amount = 0;

        _ent.OnTakeDmg += InterruptSkill;
        _ent.OnSkillAction += ShootLaser;

        ametralladora = Ametralladora();
        StartCoroutine(ametralladora);

    }  

    IEnumerator Ametralladora()
    {
        while(_amount < amountLaser)
        {
            _amount++;
            _anim.Play("StartCastOrb");
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

        Debug.Log("EL DANO QUE HACE ES " + newData.Damage);
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

        _anim = owner.GetComponentInChildren<Animator>();

        ThrowablePoolsManager.instance.CreateAPool(rayo_pf.name, rayo_pf); 
        
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
