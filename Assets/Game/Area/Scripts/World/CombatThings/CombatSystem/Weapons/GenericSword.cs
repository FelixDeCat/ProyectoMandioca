using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class GenericSword : Weapon
{
    public GenericSword(float dmg, float r, string n, float angle, DamageData _data) : base(dmg, r, n, angle, _data) { }
    public GenericSword ConfigureCallback(Action<Attack_Result, Damagetype, DamageReceiver> _callback_attack_Entity) { AttackResult = _callback_attack_Entity; return this; }

    public override void Attack(Transform pos, float damage, Damagetype dmg_type)
    {
        var entities = Physics.OverlapSphere(pos.position, range)
            .Where(x => x.GetComponent<DamageReceiver>())
            .Where(x => x.GetComponent<DamageReceiver>() != _head.GetComponent<DamageReceiver>())
            .Select(x => x.GetComponent<DamageReceiver>())
            .ToList();

        for (int i = 0; i < entities.Count; i++)
        {
            Vector3 dir = entities[i].transform.position - pos.position;
            float angle = Vector3.Angle(pos.forward, dir);

            if (dir.magnitude <= range && angle < base.angle)
            {
                data.SetDamage((int)damage)
                    .SetDamageTick(false)
                    .SetKnockback(500)
                    .SetPositionAndDirection(_head.transform.position, _head.DirAttack);

                var attackResult = entities[i].TakeDamage(data);
                
                AttackResult?.Invoke(attackResult,dmg_type, entities[i]); 
            }
        }
    }
}
