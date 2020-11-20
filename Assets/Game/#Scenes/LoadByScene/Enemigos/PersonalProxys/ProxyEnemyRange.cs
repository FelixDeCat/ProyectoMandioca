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
        if (temp != null)
        {
            temp.SetRange(attackRange);
            temp.combatDistance = visionRange;
        }
        else
        {
            Debug.LogWarning("["+enemy.gameObject.name+"] no tiene EnemyWithCombatDirector " + "[SCENE=> "+ enemy.gameObject.scene.name+"]");
        }
    }
}