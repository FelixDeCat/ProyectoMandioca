using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanBoss : EnemyBase
{
    protected override void OnInitialize()
    {
        base.OnInitialize();


    }


    #region En desuso
    protected override void Die(Vector3 dir) { }
    protected override bool IsDamage() { return true; }
    protected override void OnFixedUpdate() { }
    protected override void OnReset() { }
    protected override void OnTurnOff() { }
    protected override void OnTurnOn() { }
    protected override void OnUpdateEntity() { }
    protected override void TakeDamageFeedback(DamageData data) 
    {

    }
    #endregion
}
