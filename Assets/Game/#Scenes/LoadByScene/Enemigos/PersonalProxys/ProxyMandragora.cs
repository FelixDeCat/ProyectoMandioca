using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProxyMandragora : ProxyEnemyRange
{
    [SerializeField] bool isTrap = true;
    [SerializeField] Transform spawnSpot = null;
    [SerializeField] float radious = 7;

    public override void SpawnEnemy(EnemyBase enemy)
    {
        base.SpawnEnemy(enemy);
        var temp = enemy.GetComponent<MandragoraEnemy>();
        temp.mandragoraIsTrap = isTrap;
        temp.spawnerSpot.spawnSpot.position = spawnSpot.position;
        temp.spawnerSpot.radious = radious;
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawWireSphere(spawnSpot.position, radious);
    }
}
