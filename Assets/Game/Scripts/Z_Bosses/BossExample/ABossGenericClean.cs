using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using ToolsMandioca.StateMachine;

public class ABossGenericClean : EnemyBase
{
    [Header("Boss Options")]
    [SerializeField] GenericEnemyMove generic_move;
    [SerializeField] StateMachineHandler stateMachineHandler;
    [SerializeField] Transform root;
    [SerializeField] Rigidbody rigidBody;

    protected override void OnInitialize()
    {
        stateMachineHandler.Initialize();
        generic_move.Configure(root,rigidBody);
    }

    public override void Zone_OnPlayerEnterInThisRoom(Transform who)
    {
        stateMachineHandler.BeginBoss();
    }


    #region Desuso
    public override float ChangeSpeed(float newSpeed) { return newSpeed; }
    public override void IAInitialize(CombatDirector _director) { }
    public override Attack_Result TakeDamage(int dmg, Vector3 attack_pos, Damagetype damagetype) { return Attack_Result.sucessful; }
    public override Attack_Result TakeDamage(int dmg, Vector3 attack_pos, Damagetype damagetype, EntityBase owner_entity) { return TakeDamage(dmg, attack_pos, damagetype); }
    protected override void OnFixedUpdate() { }
    protected override void OnPause() { }
    protected override void OnResume() { }
    protected override void OnTurnOff() { }
    protected override void OnTurnOn() { }
    public override void ToAttack() => attacking = true;
    protected override void OnUpdateEntity() { }
    #endregion


}
