using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProxysTest : MonoBehaviour
{
    [SerializeField] EnemyProxyManager proxyManager = null;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) EnemyManager.Instance.OnSaveStateEnemies("x80");

        if (Input.GetKeyDown(KeyCode.O)) proxyManager.Test();

        if (Input.GetKeyDown(KeyCode.I)) EnemyManager.Instance.RespawnsEnemies();
    }
}
