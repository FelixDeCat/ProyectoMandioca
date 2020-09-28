using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProxyCarnPlant : ProxyEnemyBase
{
    [SerializeField] int dmg = 6;
    [SerializeField] float force = 200;

    public override void SpawnEnemy(EnemyBase enemy)
    {
        base.SpawnEnemy(enemy);
        enemy.transform.localScale = transform.localScale;
        var temp = enemy.GetComponent<CarivorousPlant>();
        temp.attractionForce = force;
        temp.dmg = dmg;
    }
}
