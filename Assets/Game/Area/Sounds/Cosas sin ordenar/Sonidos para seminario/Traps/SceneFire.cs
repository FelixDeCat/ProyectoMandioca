using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SceneFire : PlayObject
{
    [SerializeField] ParticleSystem mainParticle = null;
    [SerializeField] DamageData dmgData = null;
    Dictionary<DamageReceiver, float> myTargetsTimers = new Dictionary<DamageReceiver, float>();
    Dictionary<DamageReceiver, Action> targetUpdates = new Dictionary<DamageReceiver, Action>();
    [SerializeField] float timeToTick = 4;
    [SerializeField] int damage = 3;
    [SerializeField] Damagetype dmgType = Damagetype.Fire;
    [SerializeField] EffectName effectType = EffectName.OnFire;

    Action UpdateTicks = delegate { };

    protected override void OnInitialize()
    {
        dmgData.SetDamage(damage).SetDamageInfo(DamageInfo.NonBlockAndParry).SetDamageType(dmgType).SetPositionAndDirection(transform.position, Vector3.zero);
    }

    protected override void OnFixedUpdate()
    {
    }

    protected override void OnUpdate()
    {
        if(myTargetsTimers.Count > 0)
            UpdateTicks();
    }

    protected override void OnPause()
    {
        mainParticle.Pause();
    }

    protected override void OnResume()
    {
        mainParticle.Play();
    }

    protected override void OnTurnOff()
    {
        myTargetsTimers = new Dictionary<DamageReceiver, float>();
    }

    protected override void OnTurnOn()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        var dmgReceiver = other.gameObject.GetComponent<DamageReceiver>();
        if (dmgReceiver != null && !myTargetsTimers.ContainsKey(dmgReceiver))
        {
            myTargetsTimers.Add(dmgReceiver, 0);
            targetUpdates.Add(dmgReceiver, () =>
            {
                myTargetsTimers[dmgReceiver] += Time.deltaTime;

                if (myTargetsTimers[dmgReceiver] >= timeToTick)
                {
                    myTargetsTimers[dmgReceiver] = 0;
                    dmgReceiver.TakeDamage(dmgData);
                    dmgReceiver.GetComponent<EffectReceiver>()?.TakeEffect(effectType);
                }
            });
            UpdateTicks += targetUpdates[dmgReceiver];
            dmgReceiver.TakeDamage(dmgData);
            dmgReceiver.GetComponent<EffectReceiver>()?.TakeEffect(effectType);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var dmgReceiver = other.gameObject.GetComponent<DamageReceiver>();
        if (dmgReceiver != null && myTargetsTimers.ContainsKey(dmgReceiver))
        {
            UpdateTicks -= targetUpdates[dmgReceiver];
            targetUpdates.Remove(dmgReceiver);
            myTargetsTimers.Remove(dmgReceiver);
        }
    }
}
