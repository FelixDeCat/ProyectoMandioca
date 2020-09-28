using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProxyEnemyBase : MonoBehaviour
{
    public EnemyBase myEnemy;
    [SerializeField] protected bool onDrawGizmos = true;

    public virtual void SpawnEnemy(EnemyBase enemy)
    {
        enemy.transform.position = transform.position;
        enemy.transform.rotation = transform.rotation;
    }

    protected virtual void OnDrawGizmos()
    {
        if (!onDrawGizmos) return;
    }
}
