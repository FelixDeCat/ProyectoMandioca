using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class GenericSword : Weapon
{
    public GenericSword(float dmg, float r, string n, float angle) : base(dmg, r, n, angle) { }
    public GenericSword ConfigureCallback(Action<Attack_Result, Damagetype, EntityBase> _callback_attack_Entity) { AttackResult = _callback_attack_Entity; return this; }

    public override void Attack(Transform pos, float damage, Damagetype dmg_type)
    {
        var entities = Physics.OverlapSphere(pos.position, range)
            .Where(x => x.GetComponent<EntityBase>())
            .Where(x => x.GetComponent<EntityBase>() != Main.instance.GetChar())
            .ToList();


        for (int i = 0; i < entities.Count; i++)
        {
            Debug.Log(entities[i].gameObject.name);

            Vector3 dir = entities[i].transform.position - pos.position;
            float angle = Vector3.Angle(pos.forward, dir);

            var current = entities[i].GetComponent<EntityBase>();

            if (dir.magnitude <= range && angle < base.angle)
            {
                var attackResult = current.TakeDamage(
                        (int) damage, 
                        Main.instance.GetChar().transform.position, 
                        Damagetype.parriable, 
                        _head);

                AttackResult?.Invoke(attackResult,dmg_type, current); 
            }
        }
    }
}
