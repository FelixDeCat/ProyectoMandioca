using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;

public class PoisonAttack : GOAP_Skills_Base
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
    [SerializeField] float _countTime = 5;
    [SerializeField] AudioClip _chargeShot;
    [SerializeField] AudioClip _shootShot;
    Coroutine ametralladora;
    Coroutine waitToShoot;

    [SerializeField] ParticleSystem FeedBack = null;

    protected override void OnEndSkill()
    {

        Off();
        _ent.OnSkillAction -= ShootLaser;
    }

    protected override void OnExecute()
    {

        _ent.OnTakeDmg += InterruptSkill;
        _ent.OnSkillAction += ShootLaser;

        AudioManager.instance.PlaySound(_chargeShot.name, transform);
        _amount = 0;
        _countTime = 0;
        _anim.Play("StartCastOrb");
    }

    void ShootLaser()
    {
        On();

        _amount++;

        ThrowData newData;

        FeedBack.Play();

        if (_amount == amountLaser)
        {
            newData = new ThrowData().Configure(rayoOrigin.position + _ent.Root().forward * .5f, ((heroRoot.position + Vector3.up + heroRoot.forward * 4) - rayoOrigin.position).normalized, 13, 10, owner, SummonSlowGoo);
        }
        else
        {
            newData = new ThrowData().Configure(rayoOrigin.position + _ent.Root().forward * .5f, ((heroRoot.position + Vector3.up) - rayoOrigin.position).normalized, 13, 10, owner);
        }

        ThrowablePoolsManager.instance.Throw(rayo_pf.name, newData);

        if (_amount >= amountLaser)
        {
            _anim.SetTrigger("finishSkill");
            AudioManager.instance.PlaySound(_chargeShot.name, transform);

        }


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

        AudioManager.instance.GetSoundPool(_chargeShot.name, AudioGroups.GAME_FX, _chargeShot);
        AudioManager.instance.GetSoundPool(_shootShot.name, AudioGroups.GAME_FX, _shootShot);
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
        _countTime += Time.deltaTime;

        if (_countTime >= timeBwtShoots)
        {
            _countTime = 0;
            if (_amount >= amountLaser)
            {

                EndSkill();
            }
            else
            {
                Off();
                _anim.SetTrigger("loop");
                //_anim.Play("StartCastOrb");
            }
        }
    }

    protected override void OnInterruptSkill()
    {
        Off();
        _ent.OnSkillAction -= ShootLaser;
        _ent.OnTakeDmg -= InterruptSkill;
        _anim.ResetTrigger("finishSkill");
    }
}