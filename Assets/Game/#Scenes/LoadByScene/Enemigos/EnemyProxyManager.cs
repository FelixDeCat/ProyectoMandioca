using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProxyManager : LoadComponent
{
    [SerializeField] string sceneName = "";
    [SerializeField] ProxyEnemyBase[] proxys = new ProxyEnemyBase[0];
    bool AlreadyProcessed = false;

    protected void Start()
    {
        if(!AlreadyProcessed) StartCoroutine(Process());
    }

    protected override IEnumerator LoadMe()
    {
        AlreadyProcessed = true;
        yield return Process();
    }

    IEnumerator Process()
    {
        EnemyManager.Instance.OnLoadEnemies(sceneName, proxys);

        for (int i = 0; i < proxys.Length; i++) 
        { 
            Destroy(proxys[i].gameObject);
            yield return null;
        }

        proxys = new ProxyEnemyBase[0];
    }
}
