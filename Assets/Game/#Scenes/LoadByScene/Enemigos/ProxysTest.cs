using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProxysTest : MonoBehaviour
{
    [SerializeField] EnemyProxyManager[] proxyManager = new EnemyProxyManager[0];
    [SerializeField] string[] AllScenes = new string[0];

    [SerializeField] string sceneCurrent = "x80";

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) EnemyManager.Instance.OnSaveStateEnemies(sceneCurrent);

        if (Input.GetKeyDown(KeyCode.O))
        {
            for (int i = 0; i < proxyManager.Length; i++)
            {
                proxyManager[i].Test();
            }
        }

        if (Input.GetKeyDown(KeyCode.I)) EnemyManager.Instance.RespawnsEnemies(sceneCurrent);

        if (Input.GetKeyDown(KeyCode.U))
        {
            for (int i = 0; i < AllScenes.Length; i++)
            {
                EnemyManager.Instance.SceneReset(AllScenes[i]);
            }
        }
    }
}
