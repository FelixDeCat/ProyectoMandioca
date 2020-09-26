using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProxyEnemyBase : MonoBehaviour
{
    public EnemyBase myEnemy;

    public virtual void SpawnEnemy(string myScene)
    {
        EnemyManager.Instance.SpawnEnemy(myEnemy.name, myScene, myEnemy);
        Destroy(this.gameObject);
    }
}
