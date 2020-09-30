using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProxyEnemyBase : MonoBehaviour
{
    public EnemyBase myEnemy;
    [SerializeField] protected bool onDrawGizmos = true;
    [SerializeField] protected Int_IntDictionary IDs = new Int_IntDictionary();

    public virtual void SpawnEnemy(EnemyBase enemy)
    {
        enemy.transform.position = transform.position;
        enemy.transform.rotation = transform.rotation;
        var itemMision = enemy.GetComponent<ExecuteItemMision>();
        if (itemMision)
        {
            itemMision.ResetID();
            foreach (var item in IDs) itemMision.AddID(item.Key, item.Value);
        }
    }

    protected virtual void OnDrawGizmos()
    {
        if (!onDrawGizmos) return;
    }
}
