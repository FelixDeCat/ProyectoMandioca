using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : Interactable
{
    public GameObject enemyToSpawn;
    public Transform[] posToSpawn;

    public override void OnExecute(WalkingEntity collector)
    {
        for (int i = 0; i < posToSpawn.Length; i++)
        {
            Main.instance.SpawnItem(enemyToSpawn, posToSpawn[i]);
        }
    }

    public override void OnExit()
    {
        WorldItemInfo.instance.Hide();
    }

    public override void OnEnter(WalkingEntity entity)
    {
        WorldItemInfo.instance.Show(transform.position, "Enemy Spawn", "Spawnea enemigos", "Spawn");
    }

    private void OnDrawGizmos()
    {
        foreach (var t in posToSpawn)
        {
            Gizmos.DrawLine(this.transform.position, t.transform.position);
        }
    }
}
