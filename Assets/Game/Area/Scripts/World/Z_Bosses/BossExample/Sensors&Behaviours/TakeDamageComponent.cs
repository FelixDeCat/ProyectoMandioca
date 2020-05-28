using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TakeDamageComponent : EntityBase
{
    Func<int, Vector3, Damagetype, Attack_Result> funcsimple;
    Func<int, Vector3, Damagetype, EntityBase, Attack_Result> funcentity;
    Func<bool> predicate = delegate { return true; };
    public void SubscribeMeTo(Func<int, Vector3, Damagetype, Attack_Result> _funcsimple) => funcsimple += _funcsimple;
    public void SubscribeMeTo(Func<int, Vector3, Damagetype, Attack_Result> _funcsimple, Func<bool> pred) { funcsimple += _funcsimple; predicate = pred; }
    public void SubscribeMeTo(Func<int, Vector3, Damagetype, EntityBase, Attack_Result> _funcentity) => funcentity += _funcentity;
    public void SubscribeMeTo(Func<int, Vector3, Damagetype, EntityBase, Attack_Result> _funcentity, Func<bool> pred) { funcentity += _funcentity; predicate = pred; }
    public override Attack_Result TakeDamage(int dmg, Vector3 attack_pos, Damagetype damagetype) => predicate() ? funcsimple(dmg, attack_pos, damagetype) : Attack_Result.inmune;
    public override Attack_Result TakeDamage(int dmg, Vector3 attack_pos, Damagetype damagetype, EntityBase owner_entity)
    {
        return funcentity(dmg, attack_pos, damagetype, owner_entity);
    }

    #region Unused
    protected override void OnFixedUpdate() { }
    protected override void OnInitialize() { }
    protected override void OnPause() { }
    protected override void OnResume() { }
    protected override void OnTurnOff() { }
    protected override void OnTurnOn() { }
    protected override void OnUpdate() { }
    #endregion
}
