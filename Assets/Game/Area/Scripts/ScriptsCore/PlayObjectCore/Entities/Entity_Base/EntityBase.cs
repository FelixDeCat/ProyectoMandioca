using UnityEngine;
using System;
public abstract class EntityBase : PlayObject 
{
    public virtual Attack_Result TakeDamage(int dmg, Vector3 attack_pos, Damagetype damagetype) { return Attack_Result.sucessful; }
    public virtual Attack_Result TakeDamage(int dmg, Vector3 attack_pos, Damagetype damagetype, EntityBase owner_entity) { return TakeDamage(dmg, attack_pos, damagetype); }
}
