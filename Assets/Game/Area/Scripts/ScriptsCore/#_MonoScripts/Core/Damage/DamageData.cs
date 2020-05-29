using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageData : MonoBehaviour
{
    [HideInInspector] public int damage;
    [HideInInspector] public int initDamage;
    [HideInInspector] public int tickDamage;
    [HideInInspector] public int finalDamage;

    [HideInInspector] public Damagetype damageType;

    [HideInInspector] public Vector3 owner_position;

    [HideInInspector] public float knockbackForce;

    [HideInInspector] public bool hasDamageTick;
    [HideInInspector] public float damageTime;
    [HideInInspector] public Vector3 attackDir;
    [HideInInspector] public EntityBase owner;

    public void Initialize(EntityBase _owner)
    {
        owner = _owner;
    }

    public DamageData SetDamage(int dmg)
    {
        damage = dmg;
        return this;
    }

    public DamageData SetDamageType(Damagetype dmgType)
    {
        damageType = dmgType;
        return this;
    }

    public DamageData SetDamageTick(bool hasTick, int initDmg = 0, int tickDmg = 0, int finalDmg = 0, float dmgTime = 0)
    {
        hasDamageTick = hasTick;
        initDamage = initDmg;
        tickDamage = tickDmg;
        finalDamage = finalDmg;
        damageTime = dmgTime;
        return this;
    }

    public DamageData SetPositionAndDirection(Vector3 pos, Vector3 dir = default)
    {
        owner_position = pos;
        attackDir = dir;
        return this;
    }

    public DamageData SetKnockback(float _knockback)
    {
        knockbackForce = _knockback;
        return this;
    }
}
