using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class EffectBase : PlayObject
{
    [SerializeField] EffectName effectType = EffectName.OnFire;
    [SerializeField] float cdBase = 5.0f;
    [SerializeField] float cdMultiplier = 1.0f;

    bool onEffect;
    float timer;
    float currentCD;

    Action StartEffect;
    Action EndEffect;

    protected override void OnInitialize()
    {
        if (GetComponent<EffectReceiver>()) GetComponent<EffectReceiver>().AddEffect(effectType, this); else Debug.LogError("No hay un EffectReceiver");

        StartEffect += OnEffect;
        EndEffect += OffEffect;
    }

    public void AddStartCallback(Action _StartEffect) => StartEffect += _StartEffect;
    public void AddEndCallback(Action _EndCallback) => StartEffect += _EndCallback;

    protected override void OnUpdate()
    {
        if (onEffect)
        {
            timer += Time.deltaTime;

            if (timer == currentCD)
                OnEndEffect();
        }
    }

    public void OnStartEffect(float cd)
    {
        if (!onEffect)
        {
            onEffect = true;
            currentCD = cd == -1 ? cdBase : cd;
            StartEffect?.Invoke();
        }
    }

    protected abstract void OnEffect();

    public void OnEndEffect()
    {
        if (onEffect)
        {
            onEffect = false;
            timer = 0;
            EndEffect?.Invoke();
        }
    }

    protected abstract void OffEffect();

    #region En Desuso
    protected override void OnFixedUpdate() { }
    protected override void OnPause() { }
    protected override void OnResume() { }
    protected override void OnTurnOff() { }
    protected override void OnTurnOn() { }
    #endregion
}
