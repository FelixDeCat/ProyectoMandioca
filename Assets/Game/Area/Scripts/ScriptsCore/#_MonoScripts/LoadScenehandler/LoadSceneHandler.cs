using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class LoadSceneHandler : MonoBehaviour
{
    public static LoadSceneHandler instance;
    private void Awake() => instance = this;
    Action OnEndLoad = delegate { };
    //[SerializeField] GameObject model_master_loadBar;
    //[SerializeField] GameObject model_slave_loadBar;
    [SerializeField] GenericBar master_genbar;
    //GenericBar slave_genbar;

    public GameObject loadscreen;

    string SceneToLoad;

    public List<LoadComponent> loadCOmponents;

    public void LoadALoader(LocalLoader manager)
    {

    }

    public void LoadAScene(string scene)
    {
        SceneToLoad = scene;
        loadscreen.SetActive(true);
        Invoke("LoadTime", 1f);
    }

    public void StartLoad(Action callback_endLoad)
    {
       // OnEndLoad = callback_endLoad;
       //// master_genbar = Instantiate(model_master_loadBar, Main.instance.gameUiController.GetRectCanvas()).GetComponent<GenericBar>();
       // // slave_genbar = Instantiate(model_slave_loadBar, Main.instance.gameUiController.GetRectCanvas()).GetComponent<GenericBar>();
       // loadscreen.SetActive(true);

       // Invoke("LoadTime", 1f);
    }

    void LoadTime()
    {
        master_genbar.gameObject.SetActive(true);
        master_genbar.Configure(1f,0.01f);
        master_genbar.SetValue(0);

        StartCoroutine(Load().GetEnumerator());
    }

    IEnumerable Load()
    {
        yield return ComponentsToLoad().GetEnumerator();
        yield return LoadAsyncScene();
        loadscreen.SetActive(false);
    }

    public IEnumerable ComponentsToLoad()
    {
        foreach (var c in loadCOmponents)
        {
            yield return c.Load();
        }
    }
    IEnumerator LoadAsyncScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneToLoad);
        asyncLoad.completed += Completed;

        while (!asyncLoad.isDone)
        {
            yield return new WaitForEndOfFrame();
            MasterBar(asyncLoad.progress, 1);
            yield return null;
        }
        //Destroy(master_genbar.gameObject);
        
        OnEndLoad.Invoke();
        
    }

    void Completed(AsyncOperation async)
    {
        MasterBar(1, 1);
    }

    public void MasterBar(float value, int max)
    {
        master_genbar.Configure(max, 0.01f);
        master_genbar.SetValue(value);
    }
    public void SlaveBar(float value, int max)
    {
        //slave_genbar.Configure(max, 0.01f);
        //slave_genbar.SetValue(value);
    }
}
