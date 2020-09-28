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
    }

    protected override IEnumerator LoadMe()
    {
        yield return null;
    }
}
