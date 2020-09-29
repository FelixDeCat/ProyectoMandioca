using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProxyManager : LoadComponent
{
    [SerializeField] string sceneName = "";
    [SerializeField] ProxyEnemyBase[] proxys = new ProxyEnemyBase[0];

    protected void Start()
    {
        EnemyManager.Instance.OnLoadEnemies(sceneName, proxys);

        for (int i = 0; i < proxys.Length; i++) Destroy(proxys[i].gameObject);

        proxys = new ProxyEnemyBase[0];
    }

    protected override IEnumerator LoadMe()
    {
        yield return null;
        Debug.Log("Me cargo bien chetardo");
    }
}
