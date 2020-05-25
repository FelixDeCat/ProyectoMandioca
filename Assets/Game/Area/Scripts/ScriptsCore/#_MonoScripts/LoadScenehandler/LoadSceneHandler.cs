using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LoadSceneHandler : MonoBehaviour
{
    Action OnEndLoad = delegate { };
    [SerializeField] GameObject model_master_loadBar;
    [SerializeField] GameObject model_slave_loadBar;
    GenericBar master_genbar;
    GenericBar slave_genbar;

    public List<LoadComponent> loadCOmponents;

    public void LoadALoader(LocalLoader manager)
    {

    }

    public void StartLoad(Action callback_endLoad)
    {
        OnEndLoad = callback_endLoad;
        master_genbar = Instantiate(model_master_loadBar, Main.instance.gameUiController.GetRectCanvas()).GetComponent<GenericBar>();
        slave_genbar = Instantiate(model_slave_loadBar, Main.instance.gameUiController.GetRectCanvas()).GetComponent<GenericBar>();
        BeginLoad();
    }

    void BeginLoad()
    {
        Invoke("EndLoad", 3f);

    }

    void EndLoad()
    {
        Destroy(master_genbar.gameObject);
        master_genbar = null;
        OnEndLoad.Invoke();

        StartCoroutine(ComponentsToLoad().GetEnumerator());
    }

    public IEnumerable ComponentsToLoad()
    {
        foreach (var c in loadCOmponents)
        {
            Debug.Log("Name Object: " + c.gameObject.name);
            yield return c.Load();
        }
    }

    public void MasterBar(int value, int max)
    {
        master_genbar.Configure(max, 0.01f);
        master_genbar.SetValue(value);
    }
    public void SlaveBar(int value, int max)
    {

    }
}
