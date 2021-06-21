using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SceneFire : PlayObject
{
    [SerializeField] ParticleSystem mainParticle = null;
    Dictionary<EffectReceiver, float> myTargetsTimers = new Dictionary<EffectReceiver, float>();
    Dictionary<EffectReceiver, Action> targetUpdates = new Dictionary<EffectReceiver, Action>();
    [SerializeField] float timeToTick = 4;
    [SerializeField] EffectName effectType = EffectName.OnFire;

    Action UpdateTicks = delegate { };

    private void Start()
    {
        OnInitialize();
        On();
    }

    protected override void OnInitialize()
    {
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
        myTargetsTimers = new Dictionary<EffectReceiver, float>();
    }

    protected override void OnTurnOn()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        var dmgReceiver = other.gameObject.GetComponent<EffectReceiver>();
        if (dmgReceiver != null && !myTargetsTimers.ContainsKey(dmgReceiver))
        {
            myTargetsTimers.Add(dmgReceiver, 0);
            targetUpdates.Add(dmgReceiver, () =>
            {
                myTargetsTimers[dmgReceiver] += Time.deltaTime;

                if (myTargetsTimers[dmgReceiver] >= timeToTick)
                {
                    myTargetsTimers[dmgReceiver] = 0;
                    dmgReceiver.TakeEffect(effectType);
                }
            });
            UpdateTicks += targetUpdates[dmgReceiver];
            dmgReceiver.TakeEffect(effectType);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var dmgReceiver = other.gameObject.GetComponent<EffectReceiver>();
        if (dmgReceiver != null && myTargetsTimers.ContainsKey(dmgReceiver))
        {
            UpdateTicks -= targetUpdates[dmgReceiver];
            targetUpdates.Remove(dmgReceiver);
            myTargetsTimers.Remove(dmgReceiver);
        }
    }
}
