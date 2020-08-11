using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DamageReceiver : MonoBehaviour
{
    [SerializeField] List<Damagetype> invulnerability = new List<Damagetype>();
    [SerializeField] List<Damagetype> resistances = new List<Damagetype>();
    [SerializeField] List<Damagetype> debilities = new List<Damagetype>();


    [SerializeField] int debilityAddDmg = 5;
    [SerializeField] int resistanceRestDmg = 3;
    [SerializeField] float knockbackMultiplier = 1;

    bool blockEntity;
    bool parryEntity;


    Action<DamageData> takeDmg;
    Action<Vector3> OnDead;
    Action InmuneFeedback;
    Action IndestructibleFeedback;
    Func<bool> IsDmg;
    bool IsNotDestructible = false;
    Transform ownerRoot;

    Func<Vector3, Vector3, Vector3, bool> IsBlock;
    Func<Vector3, Vector3, Vector3, bool> IsParry;
    Action<EntityBase> Block;
    Action<EntityBase> Parry;
    _Base_Life_System _LifeSystem;

    Rigidbody rb;

    public void Initialize(Transform _ownerRoot, Func<bool> _IsDmg, Action<Vector3> _OnDead, Action<DamageData> _takeDmg,
        Rigidbody _rb,_Base_Life_System lifeSystem, Action _InmuneFeedback = default)
    {
        ownerRoot = _ownerRoot;
        takeDmg = _takeDmg;
        OnDead = _OnDead;
        IsDmg = _IsDmg;
        InmuneFeedback = _InmuneFeedback;
        rb = _rb;
        _LifeSystem = lifeSystem;
    }
    public DamageReceiver SetBlock(Func<Vector3, Vector3, Vector3, bool> _IsBlock, Action<EntityBase> _Block)
    {
        blockEntity = true;
        IsBlock = _IsBlock;
        Block = _Block;
        return this;
    }

    public DamageReceiver SetParry(Func<Vector3, Vector3, Vector3, bool> _IsParry, Action<EntityBase> _Parry)
    {
        parryEntity = true;
        IsParry = _IsParry;
        Parry = _Parry;
        return this;
    }


    public Attack_Result TakeDamage(DamageData data)
    {
        Debug.Log("1");
        if (IsDmg()) return Attack_Result.inmune;

        if (IsNotDestructible)
        {
            InmuneFeedback();
            return Attack_Result.inmune;
        }

        if (invulnerability.Contains(data.damageType))
        {
            InmuneFeedback();
            return Attack_Result.inmune;
        }
            
        Debug.Log("2");
        if (data.damageType != Damagetype.NonBlockAndParry)
        {
            if (parryEntity && data.damageType != Damagetype.NonParry)
            {
                if (IsParry(ownerRoot.position, data.owner_position, ownerRoot.forward))
                {
                    Parry(data.owner);
                    return Attack_Result.parried;
                }
            }
            if (blockEntity && data.damageType != Damagetype.NonBlock)
            {
                if (IsBlock(ownerRoot.position, data.owner_position, ownerRoot.forward))
                {
                    Block(data.owner);
                    return Attack_Result.blocked;
                }
            }
        }

        Debug.Log(data.damage);

        int dmg = data.damage;

        if (resistances.Contains(data.damageType)) dmg -= resistanceRestDmg;
        else if (debilities.Contains(data.damageType)) dmg += debilityAddDmg;

        Vector3 aux = (ownerRoot.position - data.owner_position).normalized;

        if(rb)
        {
            Vector3 knockbackForce = aux * data.knockbackForce + data.attackDir;
            rb.AddForce(knockbackForce * knockbackMultiplier, ForceMode.Impulse);
        }

        
        bool death = _LifeSystem.Hit(dmg);
       
        if (death) OnDead(data.attackDir);

       takeDmg(data);

        return death ? Attack_Result.death : Attack_Result.sucessful;
    }

    public void AddInvulnerability(Damagetype inv) { if (!invulnerability.Contains(inv)) invulnerability.Add(inv); }

    public void RemoveInvulnerability(Damagetype inv) { if (invulnerability.Contains(inv)) invulnerability.Remove(inv); }

    public void AddResistance(Damagetype inv) { if (!resistances.Contains(inv)) resistances.Add(inv); }

    public void RemoveResistance(Damagetype inv) { if (resistances.Contains(inv)) resistances.Remove(inv); }

    public void AddDebility(Damagetype inv) { if (!debilities.Contains(inv)) debilities.Add(inv); }

    public void RemoveDebility(Damagetype inv) { if (debilities.Contains(inv)) debilities.Remove(inv); }

    public void ChangeIndestructibility(bool isIndestructible) { IsNotDestructible = isIndestructible; }
 }
