using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class EnemyProxyManager : LoadComponent
{
    public string sceneName = "";
    ProxyEnemyBase[] proxys = new ProxyEnemyBase[0];

    bool AlreadyProcessed = false;

    protected void Start() // por si mi parent no viene con Loader
    {
        if (!AlreadyProcessed && Application.isPlaying)
        {
            AlreadyProcessed = true;
            StartCoroutine(Process());
        }
    }

    protected override IEnumerator LoadMe()//por si mi parent me llamó a cargar
    {
        if (!AlreadyProcessed)
        {
            yield return Process();
        }
        else yield return null;
    }

    IEnumerator Process()
    {
        proxys = GetComponentsInChildren<ProxyEnemyBase>();

        EnemyManager.Instance.OnLoadEnemies(sceneName, proxys);

        for (int i = 0; i < proxys.Length; i++)
        {
            if (proxys[i]) proxys[i].gameObject.SetActive(false);
            yield return null;
        }

        proxys = new ProxyEnemyBase[0];
    }

    protected override IEnumerator UnLoadMe()
    {
        AlreadyProcessed = false;
        EnemyManager.Instance.OnSaveStateEnemies(sceneName);
        return base.UnLoadMe();
    }
}
