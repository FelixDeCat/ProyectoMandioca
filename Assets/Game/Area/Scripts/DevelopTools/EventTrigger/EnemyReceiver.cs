using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyReceiver : TriggerReceiver
{
    protected override void OnExecute(params object[] parameters)
    {
        var nameparent = transform.parent.gameObject.name;
        var col = (Collider)parameters[0];

        var enemy = col.gameObject.GetComponent<EnemyBase>();

        if (enemy != null)
        {
            LocalToEnemyManager.SendEnemyToScene(nameparent, enemy);
        }
    }
}
