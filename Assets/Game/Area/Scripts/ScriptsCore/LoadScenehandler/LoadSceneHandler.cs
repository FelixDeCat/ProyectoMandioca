using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LoadSceneHandler : MonoBehaviour
{
    Action OnEndLoad = delegate { };
    [SerializeField] GameObject model_loadBar;
    GenericBar genbar;

    public void StartLoad(Action callback_endLoad)
    {
        OnEndLoad = callback_endLoad;
        genbar = Instantiate(model_loadBar, Main.instance.gameUiController.GetRectCanvas()).GetComponent<GenericBar>();
        BeginLoad();
    }

    void BeginLoad()
    {
        Invoke("EndLoad", 5f);
    }

    void EndLoad()
    {
        Destroy(genbar.gameObject);
        genbar = null;
        OnEndLoad.Invoke();
    }
}
