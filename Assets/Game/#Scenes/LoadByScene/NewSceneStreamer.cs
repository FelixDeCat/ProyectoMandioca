using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using System.Threading;

public class NewSceneStreamer : MonoBehaviour
{
    public static NewSceneStreamer instance;
    private void Awake() => instance = this;

    public HashSet<string> loaded = new HashSet<string>();
    public HashSet<string> loading = new HashSet<string>();
    public string currentScene;

    public string firstScene;

    Dictionary<string, LocalSceneHandler> localref = new Dictionary<string, LocalSceneHandler>();

    [SerializeField] float maxLoadWaitTime;

    bool IsLoaded(string sceneName) => loaded.Contains(sceneName);
    bool IsLoading(string sceneName) => loading.Contains(sceneName);

    private void Start()
    {
        LoadScene(firstScene, false, true);
    }

    public void LoadScene(string sceneName, bool LoadScreen = false, bool LoadNeighbor = false, Action OnEnd = null)
    {
        if (string.IsNullOrEmpty(sceneName) || string.Equals(sceneName, currentScene)) return;
        StartCoroutine(LoadCurrentScene(sceneName, LoadScreen, LoadNeighbor, OnEnd));
    }

    IEnumerator LoadCurrentScene(string sceneName, bool LoadScreen = false, bool LoadNeighbor = false, Action OnEnd = null)
    {
        currentScene = sceneName;
        if (!IsLoaded(currentScene)) yield return LoadAsyncAdditive(sceneName, LoadScreen, LoadNeighbor, OnEnd);

        float failsafeTime = Time.realtimeSinceStartup + maxLoadWaitTime;
        while ((loading.Count > 0) && (Time.realtimeSinceStartup < failsafeTime)) { yield return null; }

        if (LoadNeighbor)
        {
            Debug.Log("LoadNeighbor");
            StartCoroutine(LoadNeighbors(localref[currentScene].SceneData.scenes_to_view));
        }

        failsafeTime = Time.realtimeSinceStartup + maxLoadWaitTime;
        while ((loading.Count > 0) && (Time.realtimeSinceStartup < failsafeTime)) { yield return null; }

        yield return null;
    }

    IEnumerator LoadNeighbors(string[] neighbors)
    {
        Debug.Log("LoadNeighbor coroutine");
        for (int i = 0; i < neighbors.Length; i++)
        {
            yield return LoadAsyncAdditive(neighbors[i], false, false, null);
        }
    }

    IEnumerator LoadAsyncAdditive(string sceneName, bool LoadScreen = false, bool LoadNeighbor = false, Action OnEnd = null)
    {
        if (LoadScreen)
        {
            //Fades_Screens.instance.Black();
            //LoadSceneHandler.instance.On_LoadScreen();
            //Fades_Screens.instance.FadeOff(() => { });

        }

        if (!IsLoading(sceneName))
        {
            loading.Add(sceneName);
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            while (asyncOperation.progress < 0.9f)
            {
                if (LoadScreen)
                {
                    //bla bla bla
                }
                yield return null;
            }
            //yield return new WaitForEndOfFrame();
            //Fades_Screens.instance.FadeOn(() => { });
            yield return asyncOperation;


            EndLoad(sceneName, LoadScreen, LoadNeighbor, OnEnd);
        }
        else
        {
            EndLoad(sceneName, LoadScreen, LoadNeighbor, OnEnd);
        }

        yield return null;
    }

    void EndLoad(string sceneName, bool LoadScreen = false, bool LoadNeighbor = false, Action OnEnd = null)
    {
        Debug.Log("EndLoad");
        loading.Remove(sceneName);
        loaded.Add(sceneName);
        Debug.Log("hashset");

        GameObject go = GameObject.Find(sceneName);
        if (go != null)
        {
            Debug.Log("hay go con este nombre");
            var local = go.GetComponent<LocalSceneHandler>();
            if (local != null) RegisterLocalScene(sceneName, local);
        }

        if (OnEnd != null) OnEnd.Invoke();
    }


    #region Registry Zone
    public void RegisterLocalScene(string sceneName, LocalSceneHandler sceneLocalScript)
    {
        if (!localref.ContainsKey(sceneName))
        {
            localref.Add(sceneName, sceneLocalScript);
        }
        else
        {
            if (localref[sceneName] == null)
            {
                localref[sceneName] = sceneLocalScript;
            }
        }
    }
    public void UnregisterLocalScene(string sceneName)
    {
        localref.Remove(sceneName);
    }
    #endregion


    #region para probar despues
    //private void Start()
    //{
    //    SceneManager.sceneLoaded += OnSceneLoaded;
    //    SceneManager.sceneUnloaded += OnSceneUnLoaded;
    //}

    //void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
    //{
    //    GameObject firstGameobject = scene.GetRootGameObjects()[0];
    //    var localscript = firstGameobject.GetComponent<LocalSceneHandler>();
    //    if (localscript != null)
    //    {
    //        RegisterLocalScene(scene.name, localscript);
    //        StartCoroutine(localscript.Load());
    //    }
    //    else
    //    {
    //        throw new System.Exception("recibí una escena sin LocalSceneHandler");
    //    }
    //}
    //void OnSceneUnLoaded(Scene scene)
    //{
    //    UnregisterLocalScene(scene.name);
    //}
    #endregion

}
