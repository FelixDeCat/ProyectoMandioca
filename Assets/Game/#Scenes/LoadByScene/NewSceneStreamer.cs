using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using System.Threading;
using UnityEngine.Scripting;

public class NewSceneStreamer : MonoBehaviour
{
    public static NewSceneStreamer instance;
    private void Awake() => instance = this;

    public HashSet<string> loaded = new HashSet<string>();
    public HashSet<string> loading = new HashSet<string>();
    public string currentScene;

    public string firstScene;

    Dictionary<string, LocalSceneHandler> localref = new Dictionary<string, LocalSceneHandler>();
    Dictionary<string, SceneData.Detail_Parameter> hotScenesParameters = new Dictionary<string, SceneData.Detail_Parameter>();

    [SerializeField] float maxLoadWaitTime = 5;

    bool IsLoaded(string sceneName) => loaded.Contains(sceneName);
    bool IsLoading(string sceneName) => loading.Contains(sceneName);

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnLoaded;
        //GCHandle.DisableGC();
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
        if (!IsLoaded(currentScene)) yield return LoadAsyncAdditive(sceneName, LoadScreen, LoadNeighbor, true, OnEnd);

        float failsafeTime = Time.realtimeSinceStartup + maxLoadWaitTime;
        while ((loading.Count > 0) && (Time.realtimeSinceStartup < failsafeTime)) { yield return null; }

        TryToExecuteParameter(sceneName, SceneData.Detail_Parameter.full_load);

        if (LoadNeighbor)
        {
            Debug.Log("LoadNeighbor");
            StartCoroutine(LoadNeighbors(currentScene, localref[currentScene].SceneData));
        }

        failsafeTime = Time.realtimeSinceStartup + maxLoadWaitTime;
        while ((loading.Count > 0) && (Time.realtimeSinceStartup < failsafeTime)) { yield return null; }

        LocalToEnemyManager.OnLoadScene(sceneName);

         yield return null;
    }

    void TryToExecuteParameter(string sceneName, SceneData.Detail_Parameter parameter)
    {
        if (localref.ContainsKey(sceneName))
        {
            StartCoroutine( localref[sceneName].ExecuteLoadParameter(parameter));
            if (hotScenesParameters.ContainsKey(sceneName)) hotScenesParameters.Remove(sceneName);
        }
        else
        {
            if (!hotScenesParameters.ContainsKey(sceneName))
            {
                hotScenesParameters.Add(sceneName, parameter);
            }
            else
            {
                hotScenesParameters[sceneName] = parameter;
            }
        }
    }
    void ExecuteParameterByScene(string sceneName)
    {
        if (!localref.ContainsKey(sceneName) || !hotScenesParameters.ContainsKey(sceneName)) return;
        StartCoroutine(localref[sceneName].ExecuteLoadParameter(hotScenesParameters[sceneName]));
        hotScenesParameters.Remove(sceneName);
    }

    IEnumerator LoadNeighbors(string currentScene, SceneData data)
    {
        SceneData.SceneParameter[] parameters = data.sceneparam;

        //HashSet<string> currents_neigbors = new HashSet<string>();
        for (int i = 0; i < parameters.Length; i++)
        {
            
            if (!loaded.Contains(parameters[i].scene))
            {
                yield return LoadAsyncAdditive(parameters[i].scene, false, false, false, null);
            }

            TryToExecuteParameter(parameters[i].scene, parameters[i].detail);
        }

        //por ahora no quiero descargar
        /*
        HashSet<string> to_unload = new HashSet<string>(loaded);
        to_unload.ExceptWith(currents_neigbors);
        to_unload.Remove(currentScene);
        foreach (var u in to_unload)//cambiarlo por algo mas performante
        {
            Destroy(GameObject.Find(u));
            loaded.Remove(u);
            var op = SceneManager.UnloadSceneAsync(u);
            while (op.progress < 0.9f)
            {
                yield return null;
            }
            yield return op;
            LocalToEnemyManager.OnUnLoadScene(u);
        }
        */
    }

    IEnumerator LoadAsyncAdditive(string sceneName, bool LoadScreen = false, bool LoadNeighbor = false, bool exe = true, Action OnEnd = null)
    {
        if (LoadScreen)
        {
            //Fades_Screens.instance.Black();
            //LoadSceneHandler.instance.On_LoadScreen();
            //Fades_Screens.instance.FadeOff(() => { });

        }

        if (!IsLoading(sceneName) && !IsLoaded(sceneName))
        {
            loading.Add(sceneName);
            
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            
            while (asyncOperation.progress < 0.9f)
            {
                yield return null;
            }
            while (!asyncOperation.isDone)
            {
                yield return null;
            }
            //yield return new WaitForEndOfFrame();
            //Fades_Screens.instance.FadeOn(() => { });
            yield return asyncOperation;

           // EndLoad(sceneName, LoadScreen, LoadNeighbor, OnEnd);
        }
        else
        {
           // EndLoad(sceneName, LoadScreen, LoadNeighbor, OnEnd);
            
        }

        yield return null;
    }
    //void EndLoad(string sceneName, bool LoadScreen = false, bool LoadNeighbor = false, Action OnEnd = null)
    //{
    //    loading.Remove(sceneName);
    //    loaded.Add(sceneName);

    //    //optimizar esto ;) con lo de "para probar"... agregarle que si no terminó la operacion no continue
    //    GameObject go = GameObject.Find(sceneName);
    //    if (go != null)
    //    {
    //        var local = go.GetComponent<LocalSceneHandler>();
    //        if (local != null) { 
    //            RegisterLocalScene(sceneName, local);
    //            StartCoroutine(local.Load());
    //        }
    //    }

    //    if (OnEnd != null) OnEnd.Invoke();
    //}


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

    void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
    {
        loading.Remove(scene.name);
        loaded.Add(scene.name);

        GameObject firstGameobject = scene.GetRootGameObjects()[0];

        var localscript = firstGameobject.GetComponent<LocalSceneHandler>();
        if (localscript != null)
        {
            RegisterLocalScene(scene.name, localscript);
            StartCoroutine(localscript.Load());
        }
        else
        {
            throw new System.Exception("recibí una escena sin LocalSceneHandler");
        }

        ExecuteParameterByScene(scene.name);
    }
    void OnSceneUnLoaded(Scene scene)
    {
        UnregisterLocalScene(scene.name);
    }
    #endregion

}
