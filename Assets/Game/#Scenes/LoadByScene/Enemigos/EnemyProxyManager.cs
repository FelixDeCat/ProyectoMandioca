using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class EnemyProxyManager : LoadComponent
{
    [SerializeField] string sceneName = "";
    [SerializeField] ProxyEnemyBase[] proxys = new ProxyEnemyBase[0];
    bool AlreadyProcessed = false;

    [Header("ExecuteInEditMode")]
    public bool ButtonRefresh = true;

    protected void Start()//por si mi parent no viene con Loader
    {
        if (!AlreadyProcessed && Application.isPlaying)
        {
            AlreadyProcessed = true;
            Debug.Log("Enter by Start");
            StartCoroutine(Process());
        }
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (ButtonRefresh)
        {
            ButtonRefresh = false;
            proxys = GetComponentsInChildren<ProxyEnemyBase>();
        }
    }
#endif

    protected override IEnumerator LoadMe()//por si mi parent me llamó a cargar
    {
        if (!AlreadyProcessed)
        {
            Debug.Log("Enter by External Loader");
            yield return Process();
        }
        else yield return null;
    }

    IEnumerator Process()
    {
        EnemyManager.Instance.OnLoadEnemies(sceneName, proxys);

        for (int i = 0; i < proxys.Length; i++)
        {
            if (proxys[i].gameObject) Destroy(proxys[i].gameObject);
            yield return null;
        }

        proxys = new ProxyEnemyBase[0];
    }
}
