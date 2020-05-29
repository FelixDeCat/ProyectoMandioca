using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DamageReceiver : MonoBehaviour
{
    [SerializeField] List<Damagetype> invulnerability = new List<Damagetype>();
    [SerializeField] List<Damagetype> resistances = new List<Damagetype>();
    [SerializeField] List<Damagetype> debilities = new List<Damagetype>();


    [SerializeField] int debilityAddDmg;
    [SerializeField] int resistanceRestDmg;

    bool blockEntity;
    bool parryEntity;


    Func<int, bool> OnHit;
    Action<DamageData> takeDmg;
    Action OnDead;
    Action InmuneFeedback;
    Func<bool> IsDmg;
    Transform ownerRoot;

    Func<Vector3, Vector3, Vector3, bool> IsBlock;
    Func<Vector3, Vector3, Vector3, bool> IsParry;
    Action Block;
    Action Parry;

    Rigidbody rb;

    public void Initialize(Transform _ownerRoot, Func<bool> _IsDmg, Action _OnDead, Action<DamageData> _takeDmg,
        Rigidbody _rb, Func<int, bool> _OnHit, Action _InmuneFeedback = default)
    {
        ownerRoot = _ownerRoot;
        takeDmg = _takeDmg;
        OnDead = _OnDead;
        IsDmg = _IsDmg;
        InmuneFeedback = _InmuneFeedback;
        OnHit = _OnHit;
        rb = _rb;
    }
    public DamageReceiver SetBlock(Func<Vector3, Vector3, Vector3, bool> _IsBlock, Action _Block)
    {
        blockEntity = true;
        IsBlock = _IsBlock;
        Block = _Block;
        return this;
    }

    public DamageReceiver SetParry(Func<Vector3, Vector3, Vector3, bool> _IsParry, Action _Parry)
    {
        parryEntity = true;
        IsParry = _IsParry;
        Parry = _Parry;
        return this;
    }


    public Attack_Result TakeDamage(DamageData data)
    {
        if (IsDmg()) return Attack_Result.inmune;

        if (invulnerability.Contains(data.damageType))
        {
            InmuneFeedback();
            return Attack_Result.inmune;
        }

        if (parryEntity)
        {
            if (IsParry(ownerRoot.position, data.owner_position, ownerRoot.forward))
            {
                Parry();
                return Attack_Result.blocked;
            }
        }
        if (blockEntity)
        {
            if (IsBlock(ownerRoot.position, data.owner_position, ownerRoot.forward))
            {
                Block();
                return Attack_Result.blocked;
            }
        }

        int dmg = data.damage;

        if (resistances.Contains(data.damageType)) dmg -= resistanceRestDmg;
        else if (debilities.Contains(data.damageType)) dmg += debilityAddDmg;

        Vector3 aux = (ownerRoot.position - data.owner_position).normalized;
        rb.AddForce(aux * data.knockbackForce + data.attackDir, ForceMode.Impulse);

        bool death = OnHit(dmg);
        takeDmg(data);

        return death ? Attack_Result.death : Attack_Result.sucessful;
    }

    public void AddInvulnerability(Damagetype inv) { if (!invulnerability.Contains(inv)) invulnerability.Add(inv); }

    public void RemoveInvulnerability(Damagetype inv) { if (invulnerability.Contains(inv)) invulnerability.Remove(inv); }

    public void AddResistance(Damagetype inv) { if (!resistances.Contains(inv)) resistances.Add(inv); }

    public void RemoveResistance(Damagetype inv) { if (resistances.Contains(inv)) resistances.Remove(inv); }

    public void AddDebility(Damagetype inv) { if (!debilities.Contains(inv)) debilities.Add(inv); }

    public void RemoveDebility(Damagetype inv) { if (debilities.Contains(inv)) debilities.Remove(inv); }
}
