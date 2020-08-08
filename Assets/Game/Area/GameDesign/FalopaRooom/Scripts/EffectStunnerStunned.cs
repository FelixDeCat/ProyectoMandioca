using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EffectStunnerStunned : EffectBase
{
    public UnityEvent startStun;
    public UnityEvent endStun;

    protected override void OffEffect()
    {
        endStun.Invoke();
    }

    protected override void OnEffect()
    {
        startStun.Invoke();
    }

    protected override void OnTickEffect(float cdPercent)
    {

    }
}
