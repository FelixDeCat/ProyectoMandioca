﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using System.Linq;

public class LoadSceneHandler : MonoBehaviour
{
    public static LoadSceneHandler instance;
    private void Awake() => instance = this;
    public Action<int> OnEndLoad = delegate { };
    //[SerializeField] GameObject model_master_loadBar;
    [SerializeField] GenericBar master_genbar_localLoader = null;
    [SerializeField] GenericBar master_genbar_Scene = null;
    //GenericBar slave_genbar;

    public GameObject loadscreen;

    public void On_LoadScreen() => loadscreen.SetActive(true);
    public void Off_LoadScreen() => loadscreen.SetActive(false);
    public GenericBar GetMasterBar() => master_genbar_Scene;

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

    public void LoadAScene(string scene, bool loadScreen = true, LoadSceneMode mode = LoadSceneMode.Single)
    {
        SceneToLoad = scene;

        if (loadScreen)
        {
            loadscreen.SetActive(true);
            Fades_Screens.instance.Black();
            Fades_Screens.instance.FadeOff(LoadTime);
        }
        else
        {
            StartCoroutine(Load(true, mode).GetEnumerator());
        }
    }

    void LoadTime()
    {
        master_genbar_Scene?.gameObject.SetActive(true);
        master_genbar_Scene?.Configure(1f, 0.01f);
        master_genbar_Scene?.SetValue(0);
        master_genbar_localLoader?.gameObject.SetActive(true);
        master_genbar_localLoader?.Configure(1f, 0.01f);
        master_genbar_localLoader?.SetValue(0);

        StartCoroutine(Load().GetEnumerator());
    }

    IEnumerable Load(bool loadScene = true, LoadSceneMode mode = LoadSceneMode.Single)
    {
        yield return ComponentsToLoad().GetEnumerator();
        if (loadScene) if (!stayHere) 
            { 
                yield return LoadAsyncScene(mode); 
            }
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
    IEnumerator LoadAsyncScene(LoadSceneMode mode = LoadSceneMode.Single)
    {
        if (SceneManager.GetActiveScene().name != SceneToLoad)
        {
            asyncLoad = SceneManager.LoadSceneAsync(SceneToLoad, mode);
            asyncLoad.completed += Completed;
            asyncLoad.allowSceneActivation = false;
            while (!asyncLoad.isDone)
            {
                //yield return new WaitForEndOfFrame();
                MasterBarScene(asyncLoad.progress, 1);

                if (asyncLoad.progress >= 0.9f)
                {
                    yield return null;
                }
                asyncLoad.allowSceneActivation = true;
            }
            OnEndLoad.Invoke(SceneManager.GetActiveScene().buildIndex);
        }
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
        master_genbar_Scene?.Configure(max, 0.01f);
        master_genbar_Scene?.SetValue(value);
    }
    public void MasterBarScripts(float value, int max)
    {
        master_genbar_localLoader?.Configure(max, 0.01f);
        master_genbar_localLoader?.SetValue(value);
    }
    #endregion

}
