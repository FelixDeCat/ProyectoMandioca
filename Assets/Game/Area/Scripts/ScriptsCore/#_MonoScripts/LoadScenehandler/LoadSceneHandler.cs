using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using System.Linq;

public class LoadSceneHandler : MonoBehaviour
{
    public static LoadSceneHandler instance;
    private void Awake() => instance = this;
    Action OnEndLoad = delegate { };
    //[SerializeField] GameObject model_master_loadBar;
    [SerializeField] GenericBar master_genbar_localLoader = null;
    [SerializeField] GenericBar master_genbar_Scene = null;
    //GenericBar slave_genbar;

    public GameObject loadscreen;

    string SceneToLoad;

    public List<LoadComponent> loadCOmponents;

    public bool stayHere;
    public void Start()
    {
        if (stayHere)
        {
            LoadAScene("asdasda");
        }
    }

    public void LoadAScene(string scene)
    {
        SceneToLoad = scene;
        loadscreen.SetActive(true);
        Fades_Screens.instance.Black();
        Fades_Screens.instance.FadeOff(LoadTime);
    }

    void LoadTime()
    {
        master_genbar_Scene.gameObject.SetActive(true);
        master_genbar_Scene.Configure(1f, 0.01f);
        master_genbar_Scene.SetValue(0);
        master_genbar_localLoader.gameObject.SetActive(true);
        master_genbar_localLoader.Configure(1f, 0.01f);
        master_genbar_localLoader.SetValue(0);

        StartCoroutine(Load().GetEnumerator());
    }

    IEnumerable Load(bool loadScene = true)
    {
        yield return ComponentsToLoad().GetEnumerator();
        if(loadScene) if (!stayHere) yield return LoadAsyncScene();
        loadscreen.SetActive(false);
    }

    public IEnumerable LoadComponents(LoadComponent[] newLoadComponents, Action endCoroutine)
    {
        loadCOmponents.Clear();
        loadCOmponents = newLoadComponents.ToList();
        yield return StartCoroutine(Load(false).GetEnumerator());
        endCoroutine.Invoke();
    }

    #region LocalLoaders
    public IEnumerable ComponentsToLoad()
    {
        for (int i = 0; i < loadCOmponents.Count; i++)
        {
            MasterBarScripts(i, loadCOmponents.Count);
            yield return loadCOmponents[i].Load();
        }
        loadCOmponents.Clear();
    }
    #endregion

    #region Scene
    AsyncOperation asyncLoad;
    IEnumerator LoadAsyncScene()
    {
        asyncLoad = SceneManager.LoadSceneAsync(SceneToLoad);
        asyncLoad.completed += Completed;
        asyncLoad.allowSceneActivation = false;
        while (!asyncLoad.isDone)
        {
            yield return new WaitForEndOfFrame();
            MasterBarScene(asyncLoad.progress, 1);

            if (asyncLoad.progress >= 0.9f)
            {
                asyncLoad.allowSceneActivation = true;
            }
            yield return null;
        }
        OnEndLoad.Invoke();
    }
    void Completed(AsyncOperation async) 
    { 
        MasterBarScene(1, 1);
        Fades_Screens.instance.Black();
        Fades_Screens.instance.FadeOff(PLAY);
    }
    void PLAY()
    {
    }
    #endregion

    #region Bar
    public void MasterBarScene(float value, int max)
    {
        master_genbar_Scene.Configure(max, 0.01f);
        master_genbar_Scene.SetValue(value);
    }
    public void MasterBarScripts(float value, int max)
    {
        master_genbar_localLoader.Configure(max, 0.01f);
        master_genbar_localLoader.SetValue(value);
    }
    #endregion

}
