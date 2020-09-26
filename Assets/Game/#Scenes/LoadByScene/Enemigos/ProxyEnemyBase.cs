using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProxyEnemyBase : MonoBehaviour
{
    public EnemyBase myEnemy;

    public virtual void SpawnEnemy(EnemyBase enemy)
    {
        enemy.transform.position = transform.position;
        enemy.transform.rotation = transform.rotation;
    }
}
