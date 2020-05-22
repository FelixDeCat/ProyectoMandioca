using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon
{
    public float baseDamage
    {
        get;
        private set;
    }
    protected float range;
    protected float originalRange;
    public string weaponName
    {
        get;
        private set;
    }
    protected float angle;
    protected float originalAngle;

    protected CharacterHead _head;

    public Action<Attack_Result, Damagetype, EntityBase> AttackResult;

    public Weapon(float dmg, float r, string n, float angle)
    {
        baseDamage = dmg;
        range = r;
        weaponName = n;
        this.angle = angle;
        originalAngle = angle;
        originalRange = range;
        _head = Main.instance.GetChar();
    }

    #region Range Handler
    /// está por parametro opcional, si se envia parametro usa ese valor, si no se manada setea -1 y usa el rango original
    bool overriderange = false;
    public float ModifyAttackrange(float changedValue = -1, float multiplier = 1f) 
    {
        if (overriderange) return range;
        range = (changedValue == -1) ? range = originalRange : range = changedValue;
        range *= multiplier;
        return range;
    }
    public float BeginOverrideRange(float changedValue = -1, float multiplier = 1f)
    {
        overriderange = true;
        range = (changedValue == -1) ? range = originalRange : range = changedValue;
        range *= multiplier;
        return range;
    }
    public void EndOverrideRange()
    {
        overriderange = false;
        range = originalRange;
    }
    #endregion

    #region Angle Handler
    bool overrideAngle = false;
    public float ModifyAttackAngle(float changedValue = -1, float multiplier = 1f)
    {
        if (overrideAngle) return angle;
        angle = (changedValue == -1) ? angle = originalAngle : angle = changedValue;
        angle *= multiplier;
        return angle;
    }
    public float BeginOverrideAngle(float changedValue = -1, float multiplier = 1f)
    {
        overrideAngle = true;
        angle = (changedValue == -1) ? angle = originalAngle : angle = changedValue;
        angle *= multiplier;
        return angle;
    }
    public void EndOverrideAngle()
    {
        overrideAngle = false;
        angle = originalAngle;
    }
    #endregion







    public float GetWpnRange()
    {
        return range;
    }
    public float GetWpnOriginalRange()
    {
        return originalRange;
    }
    public abstract void Attack(Transform pos, float damage, Damagetype dmg_type);
}
