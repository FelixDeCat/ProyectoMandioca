using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProxyEnemyRange : ProxyEnemyBase
{
    [SerializeField] float rangeToCombat = 20;
    [SerializeField] float rangeToAttack = 1.5f;

    public override void SpawnEnemy(EnemyBase enemy)
    {
        base.SpawnEnemy(enemy);
        var temp = enemy.GetComponent<EnemyWithCombatDirector>();
        temp.SetRange(rangeToAttack);
        temp.combatDistance = rangeToCombat;
    }
}