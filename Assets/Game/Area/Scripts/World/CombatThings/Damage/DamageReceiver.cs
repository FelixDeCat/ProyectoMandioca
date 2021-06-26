using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DamageReceiver : MonoBehaviour
{
    [SerializeField] List<Damagetype> invulnerability = new List<Damagetype>();
    [SerializeField] List<Damagetype> onlyVulnerablyTo = new List<Damagetype>();
    [SerializeField] DmgType_FloatDictionary resistances = new DmgType_FloatDictionary();
    [SerializeField] DmgType_FloatDictionary debilities = new DmgType_FloatDictionary();

    [SerializeField] float knockbackMultiplier = 1;

    bool blockEntity;

    bool parryEntity;

    Action<DamageData> takeDmg;
    Action<Vector3> OnDead;
    Action<DamageData> InmuneFeedback;
    Action<Vector3> OwnKnockback;
    Func<bool> IsDmg;
    Transform ownerRoot;

    Func<bool> DontApplyKnockback =delegate { return false; };

    Func<Vector3, Vector3, Vector3, bool> IsBlock;
    Func<Vector3, Vector3, Vector3, bool> IsParry;
    Action<EntityBase> Block;
    Action<EntityBase> Parry;
    _Base_Life_System _LifeSystem;

    Rigidbody rb;

    #region Builder
    public void Initialize(Transform _ownerRoot, Rigidbody _rb,_Base_Life_System lifeSystem)
    {
        if (_ownerRoot != null) ownerRoot = _ownerRoot;
        if (_rb != null) rb = _rb;
        if (lifeSystem != null) _LifeSystem = lifeSystem;
    }

    public void ChangeKnockback(Action<Vector3> _OwnKnockback, Func<bool> _DontApplyKnockback)
    {
        rb = null;
        OwnKnockback = _OwnKnockback;
        DontApplyKnockback = _DontApplyKnockback;
    }

    public DamageReceiver AddDead(Action<Vector3> _OnDead)
    {
        OnDead += _OnDead;
        return this;
    }

    public DamageReceiver AddTakeDamage(Action<DamageData> _takeDmg)
    {
        takeDmg += _takeDmg;
        return this;
    }

    public DamageReceiver RestTakeDamage(Action<DamageData> _takeDmg)
    {
        takeDmg -= _takeDmg;
        return this;
    }

    public DamageReceiver SetIsDamage(Func<bool> _IsDmg)
    {
        IsDmg = _IsDmg;
        return this;
    }

    public DamageReceiver AddInmuneFeedback(Action<DamageData> _InmuneFeedback)
    {
        InmuneFeedback += _InmuneFeedback;
        return this;
    }

    public DamageReceiver RestInmuneFeedback(Action<DamageData> _InmuneFeedback)
    {
        InmuneFeedback -= _InmuneFeedback;
        return this;
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
    #endregion

    public Attack_Result TakeDamage(DamageData data)
    {
        if(_LifeSystem == null) { Debug.Log("WASD " + name); return Attack_Result.inmune; }
        if (IsDmg != null && IsDmg()) return Attack_Result.inmune;

        if (_LifeSystem != null && _LifeSystem.Life <= 0) return Attack_Result.inmune;

        if (onlyVulnerablyTo.Count != 0 && !onlyVulnerablyTo.Contains(data.damageType))
        {
            InmuneFeedback?.Invoke(data);
            return Attack_Result.inmune;
        }

        if (invulnerability.Contains(Damagetype.All) || invulnerability.Contains(data.damageType))
        {
            InmuneFeedback?.Invoke(data);
            return Attack_Result.inmune;
        }

        if (data.damageInfo != DamageInfo.NonBlockAndParry)
        {
            if (parryEntity && data.damageInfo != DamageInfo.NonParry)
            {
                if (IsParry(ownerRoot.position, data.owner_position, ownerRoot.forward))
                {
                    Parry(data.owner);
                    return Attack_Result.parried;
                }
            }
            if (blockEntity && data.damageInfo != DamageInfo.NonBlock)
            {
                if (IsBlock(ownerRoot.position, data.owner_position, ownerRoot.forward))
                {
                    Block(data.owner);
                    return Attack_Result.blocked;
                }
            }
        }

        float tempDmg = data.damage;

        if (resistances.ContainsKey(data.damageType)) tempDmg *= resistances[data.damageType];
        else if (debilities.ContainsKey(data.damageType)) tempDmg *= debilities[data.damageType];

        int dmg = (int)tempDmg;

        Vector3 aux = (ownerRoot.position - data.owner_position).normalized;

        Vector3 knockbackForce = aux * data.knockbackForce + data.attackDir;

        knockbackForce.y = 0;
        if(knockbackForce != Vector3.zero)
        {
            if (rb)
                rb.AddForce(Vector3.up + knockbackForce * knockbackMultiplier, ForceMode.Impulse);
            else
            {
                if (!DontApplyKnockback())
                {
                    OwnKnockback?.Invoke(Vector3.up + knockbackForce * knockbackMultiplier);
                }
            }
        }

        bool death = _LifeSystem.Hit(dmg);
            takeDmg?.Invoke(data);

        if (death) OnDead?.Invoke(data.attackDir);

        return death ? Attack_Result.death : Attack_Result.sucessful;
    }

    public void DamageTick(int damage, Damagetype dmgType)
    {
        if (_LifeSystem != null && _LifeSystem.Life <= 0) return;

        if (onlyVulnerablyTo.Count != 0 && !onlyVulnerablyTo.Contains(dmgType)) return;

        if (invulnerability.Contains(Damagetype.All) || invulnerability.Contains(dmgType)) return;

        float tempDmg = damage;

        if (resistances.ContainsKey(dmgType)) tempDmg *= resistances[dmgType];
        else if (debilities.ContainsKey(dmgType)) tempDmg *= debilities[dmgType];

        int dmg = (int)tempDmg;

        bool death = _LifeSystem.Hit(dmg);

        if (death) OnDead?.Invoke(Vector3.zero);
    }

    public void InstaKill()
    {
        if (_LifeSystem == null || _LifeSystem.Life <= 0) return;

        int dmg = _LifeSystem.Life;

        bool death = _LifeSystem.Hit(dmg);

        if (death) OnDead?.Invoke(Vector3.zero);
    }

    public void AddInvulnerability(Damagetype inv) { if (!invulnerability.Contains(inv)) invulnerability.Add(inv); }

    public void RemoveInvulnerability(Damagetype inv) { if (invulnerability.Contains(inv)) invulnerability.Remove(inv); }

    public void AddResistance(Damagetype inv, float resistanceMultiplier) { if (!resistances.ContainsKey(inv)) resistances.Add(inv, resistanceMultiplier); }

    public void RemoveResistance(Damagetype inv) { if (resistances.ContainsKey(inv)) resistances.Remove(inv); }

    public void AddDebility(Damagetype inv, float debilityMultiplier) { if (!debilities.ContainsKey(inv)) debilities.Add(inv, debilityMultiplier); }

    public void RemoveDebility(Damagetype inv) { if (debilities.ContainsKey(inv)) debilities.Remove(inv); }
 }
