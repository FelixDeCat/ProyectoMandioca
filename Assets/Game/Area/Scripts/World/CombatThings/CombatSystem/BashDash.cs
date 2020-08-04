using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BashDash : Weapon
{
    public BashDash(float dmg, float r, string n, float angle, DamageData _data) : base(dmg, r, n, angle, _data)
    {
    }

    public override void Attack(Transform pos, float damage, Damagetype dmg_type)
    {
        var entities = Physics.OverlapSphere(pos.position, range)
            .Where(x => x.GetComponent<EffectReceiver>())
            .Where(x => x.GetComponent<EffectReceiver>() != _head.GetComponent<EffectReceiver>())
            .Select(x => x.GetComponent<EffectReceiver>())
            .ToList();

        for (int i = 0; i < entities.Count; i++)
        {
            Vector3 dir = entities[i].transform.position - pos.position;
            float angle = Vector3.Angle(pos.forward, dir);

            if (dir.magnitude <= range && angle < base.angle)
                entities[i].TakeEffect(EffectName.OnPetrify, 1.5f);
        }
    }
}
