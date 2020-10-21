using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricBullet : Waves
{
    [SerializeField] float stunTime;

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        if (other.gameObject.GetComponent<EnemyBase>())
        {
            other.GetComponent<EffectReceiver>().TakeEffect(EffectName.OnFreeze, stunTime);
        }

        if (other.gameObject.GetComponent<ElectricOrb>())
        {
            other.gameObject.GetComponent<ElectricOrb>().Explode();
            Destroy(gameObject);
        }
    }
}
