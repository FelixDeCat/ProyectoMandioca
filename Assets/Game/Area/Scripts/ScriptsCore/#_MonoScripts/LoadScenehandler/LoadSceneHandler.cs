using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class LoadSceneHandler : MonoBehaviour
{
    Action OnEndLoad = delegate { };
    [SerializeField] GameObject model_master_loadBar;
    [SerializeField] GameObject model_slave_loadBar;
    GenericBar master_genbar;
    GenericBar slave_genbar;

    public string firstScene;

    public List<LoadComponent> loadCOmponents;

    public void LoadALoader(LocalLoader manager)
    {

    }

    public void StartLoad(Action callback_endLoad)
    {
        OnEndLoad = callback_endLoad;
        master_genbar = Instantiate(model_master_loadBar, Main.instance.gameUiController.GetRectCanvas()).GetComponent<GenericBar>();
        // slave_genbar = Instantiate(model_slave_loadBar, Main.instance.gameUiController.GetRectCanvas()).GetComponent<GenericBar>();
        Invoke("timeload", 3f);
    }

    void timeload()
    {
        StartCoroutine(Load().GetEnumerator());
    }

    IEnumerable Load()
    {
        Debug.Log("StartComponents");
        yield return ComponentsToLoad().GetEnumerator();
        Debug.Log("StartScene");
        yield return LoadYourAsyncScene();
        //Destroy(master_genbar.gameObject);
        
    }

    public IEnumerable ComponentsToLoad()
    {
        foreach (var c in loadCOmponents)
        {
            yield return c.Load();
        }
    }
    IEnumerator LoadYourAsyncScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(firstScene);
        //asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone)
        {
            yield return new WaitForEndOfFrame();

            MasterBar(asyncLoad.progress, 1);
            yield return null;
        }
        Debug.Log("SEEJECUTAAAAAAA");
       // master_genbar = null;
        //asyncLoad.allowSceneActivation = true;
        OnEndLoad.Invoke();
        
    }

    public void MasterBar(float value, int max)
    {
        Debug.Log("VALS: " + value + " - " + max);

        master_genbar.Configure(max, 0.01f);
        master_genbar.SetValue(value);
    }
    public void SlaveBar(float value, int max)
    {
        slave_genbar.Configure(max, 0.01f);
        slave_genbar.SetValue(value);
    }
}
