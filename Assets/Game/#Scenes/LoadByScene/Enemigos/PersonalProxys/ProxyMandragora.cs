using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProxyMandragora : ProxyEnemyRange
{
    [SerializeField] bool isTrap = true;
    [SerializeField] Transform spawnSpot = null;
    [SerializeField] float radious = 7;
    [SerializeField] BoxCollider trigger = null;
    [SerializeField] int enemiesToSpawn = 3;

    public override void SpawnEnemy(EnemyBase enemy)
    {
        base.SpawnEnemy(enemy);
        var temp = enemy.GetComponent<MandragoraEnemy>();
        temp.mandragoraIsTrap = isTrap;
        temp.spawnerSpot.spawnSpot.position = spawnSpot.position;
        temp.spawnerSpot.radious = radious;
        temp.enemiesToSpawn = enemiesToSpawn;
        var aux = temp.GetComponentInChildren<BoxCollider>();
        aux.transform.position = trigger.transform.position;
        //aux.center = trigger.center;
        //aux.size = trigger.size;
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        if(spawnSpot) Gizmos.DrawWireSphere(spawnSpot.position, radious);
    }
}
