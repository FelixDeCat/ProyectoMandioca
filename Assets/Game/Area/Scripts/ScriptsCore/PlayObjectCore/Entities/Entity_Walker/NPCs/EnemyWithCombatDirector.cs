using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyWithCombatDirector : EnemyBase
{
    [SerializeField] protected CombatDirectorElement combatElement = null;
    [SerializeField, Range(0.5f, 30)] protected float distancePos = 1.5f;
    public float combatDistance = 20;
    protected CombatDirector director;

    protected override void OnInitialize()
    {
        base.OnInitialize();
        director = Main.instance.GetCombatDirector();
        combatElement.Initialize(distancePos, director);
    }

    public void SetRange(float distance) => combatElement.distancePos = distance;
    public float GetRange() => combatElement.distancePos;

    public abstract void IAInitialize(CombatDirector director);

    public override void ResetEntity()
    {
        base.ResetEntity();
        combatElement.ResetCombat();
    }
}
