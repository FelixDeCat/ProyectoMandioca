using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProxyEnemyRange : ProxyEnemyBase
{
    [SerializeField] float visionRange = 20;
    [SerializeField] float attackRange = 1.5f;

    public override void SpawnEnemy(EnemyBase enemy)
    {
        base.SpawnEnemy(enemy);
        var temp = enemy.GetComponent<EnemyWithCombatDirector>();
        temp.SetRange(attackRange);
        temp.combatDistance = visionRange;
    }
}