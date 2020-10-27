using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProxyMandragora : ProxyEnemyRange
{
    [SerializeField] EnemyBase_IntDictionary enemySpawn = new EnemyBase_IntDictionary();
    [SerializeField] bool isTrap = true;
    [SerializeField] Transform spawnSpot = null;
    [SerializeField] float radious = 7;
    [SerializeField] BoxCollider trigger = null;

    public override void SpawnEnemy(EnemyBase enemy)
    {
        base.SpawnEnemy(enemy);
        var temp = enemy.GetComponent<MandragoraEnemy>();
        temp.mandragoraIsTrap = isTrap;
        temp.spawnerSpot.spawnSpot.position = spawnSpot.position;
        temp.spawnerSpot.radious = radious;
        temp.enemiesToSpawnDic = new EnemyBase_IntDictionary();
        foreach (var item in enemySpawn)
            temp.enemiesToSpawnDic.Add(item.Key, item.Value);
        var aux = temp.GetComponentInChildren<TriggerDispatcher>();
        aux.transform.position = trigger.transform.position;
        aux.transform.localScale = trigger.transform.localScale;
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        if(spawnSpot) Gizmos.DrawWireSphere(spawnSpot.position, radious);
    }
}
