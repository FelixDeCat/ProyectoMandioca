using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyReceiver : TriggerReceiver
{
    protected override void OnExecute(Collider col)
    {
        var nameparent = transform.parent.gameObject.name;

        var enemy = col.gameObject.GetComponent<EnemyBase>();

        if (enemy != null)
        {
            LocalToEnemyManager.SendEnemyToScene(nameparent, enemy);
        }
    }
}
