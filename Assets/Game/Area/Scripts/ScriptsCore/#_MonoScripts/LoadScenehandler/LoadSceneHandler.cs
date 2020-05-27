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
    [SerializeField] GenericBar master_genbar_localLoader;
    [SerializeField] GenericBar master_genbar_Scene;
    //GenericBar slave_genbar;

    public GameObject loadscreen;

    string SceneToLoad;

    public List<LoadComponent> loadCOmponents;

    public void LoadALoader(LocalLoader localloader)
    {
        foreach (var loader in localloader.GetLoaders())
            loadCOmponents.Add(loader);
    }

    public void LoadAScene(string scene)
    {
        SceneToLoad = scene;
        loadscreen.SetActive(true);
        Invoke("LoadTime", 0.1f);
    }

    void LoadTime()
    {
        master_genbar_Scene.gameObject.SetActive(true);
        master_genbar_Scene.Configure(1f,0.01f);
        master_genbar_Scene.SetValue(0);
        loadscreen.SetActive(false);
        StartCoroutine(Load().GetEnumerator());
    }

    IEnumerable Load()
    {
        yield return LoadAsyncScene();
        yield return ComponentsToLoad().GetEnumerator();
    }

    #region LocalLoaders
    public IEnumerable ComponentsToLoad()
    {
        foreach (var c in loadCOmponents) 
            yield return c.Load();
        loadCOmponents.Clear();
    }
    #endregion

    #region Scene
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
        OnEndLoad.Invoke();
    }
    void Completed(AsyncOperation async) { MasterBar(1, 1); }
    #endregion

    #region Bar
    public void MasterBar(float value, int max)
    {
        master_genbar_Scene.Configure(max, 0.01f);
        master_genbar_Scene.SetValue(value);
    }
    #endregion

}
