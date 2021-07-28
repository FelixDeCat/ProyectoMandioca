using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SceneFire : PlayObject
{
    [SerializeField] ParticleSystem mainParticle = null;
    [SerializeField] float timeToTick = 4;
    [SerializeField] EffectName effectType = EffectName.OnFire;

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
    }

    protected override void OnTurnOn()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        var dmgReceiver = other.gameObject.GetComponent<EffectReceiver>();
        if(dmgReceiver == null) dmgReceiver = other.gameObject.GetComponentInParent<EffectReceiver>();
        if (dmgReceiver != null)
        {
            dmgReceiver.TakeEffect(effectType);
        }
    }
}
