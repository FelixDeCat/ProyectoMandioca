using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProxyTotemSpawner : ProxyEnemyBase
{
    [SerializeField] Transform spawnSpot = null;
    [SerializeField] float radious = 7;
    [SerializeField] BoxCollider trigger = null;
    [SerializeField] int maxSpawns = 10;
    [SerializeField] int wavesAmmount = 3;
    [SerializeField] PlayObject prefab = null;
    [SerializeField] float castingTime = 3;

    public override void SpawnEnemy(EnemyBase enemy)
    {
        base.SpawnEnemy(enemy);
        var temp = enemy.GetComponent<CustomSpawner>();

        if (temp.spot.spawnSpot != null) temp.spot.spawnSpot.position = spawnSpot.position;
        else { Debug.Log("-> " + enemy.gameObject.name + " no tiene spawnSpot"); }

        temp.spot.radious = radious;
        temp.maxSpawn = maxSpawns;
        temp.waveAmount = wavesAmmount;
        temp.ChangePool(prefab);
        temp.currentSpawn = 0;
        var aux = temp.GetComponentInChildren<BoxCollider>();
        aux.transform.position = trigger.transform.position;
        aux.size = trigger.size;
        aux.center = trigger.center;
        var aux2 = temp.GetComponentInChildren<CastingBar>();
        aux2.castingTime = castingTime;
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        if (spawnSpot) Gizmos.DrawWireSphere(spawnSpot.position, radious);
    }
}
