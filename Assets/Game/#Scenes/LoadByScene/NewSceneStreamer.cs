﻿using System.Collections;
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
    Dictionary<string, Scene> scenes = new Dictionary<string, Scene>();
    public Scene GetSceneByName(string sceneName) { return scenes[sceneName]; }

    [SerializeField] float maxLoadWaitTime = 5;

    bool IsLoaded(string sceneName) => loaded.Contains(sceneName);
    bool IsLoading(string sceneName) => loading.Contains(sceneName);

    Action OnEnd = delegate { };

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnLoaded;
        Checkpoint_Manager.instance.StopGame();
        //GCHandle.DisableGC();
        LoadScene(firstScene, true, true, EndLoad, true);
    }
    public void EndLoad()
    {
        Checkpoint_Manager.instance.StartGame();
    }

    public void LoadScene(string sceneName, bool LoadScreen = false, bool LoadNeighbor = false, Action OnEnd = null, bool waitToLoad = false)
    {
        this.OnEnd = OnEnd;
        if (string.IsNullOrEmpty(sceneName) || string.Equals(sceneName, currentScene)) return;
        
        StartCoroutine(LoadCurrentScene(sceneName, LoadScreen, LoadNeighbor,  waitToLoad));
    }
    

    IEnumerator LoadCurrentScene(string sceneName, bool LoadScreen = false, bool LoadNeighbor = false, bool waitToLoad = false)
    {
        currentScene = sceneName;
        if (!IsLoaded(currentScene)) yield return LoadAsyncAdditive(sceneName, LoadScreen, LoadNeighbor, true);

        float failsafeTime = Time.realtimeSinceStartup + maxLoadWaitTime;
        while ((loading.Count > 0) && (Time.realtimeSinceStartup < failsafeTime)) { yield return null; }

        TryToExecuteParameter(sceneName, SceneData.Detail_Parameter.full_load);

        if (LoadNeighbor)
        {
            if (waitToLoad)
            {
                yield return LoadNeighbors(currentScene, localref[currentScene].SceneData);
            }
            else
            {
                StartCoroutine(LoadNeighbors(currentScene, localref[currentScene].SceneData));
            }
            
        }

        failsafeTime = Time.realtimeSinceStartup + maxLoadWaitTime;
        while ((loading.Count > 0) && (Time.realtimeSinceStartup < failsafeTime)) { yield return null; }

        LocalToEnemyManager.OnLoadScene(sceneName);

        if (OnEnd != null)
        {
            OnEnd.Invoke();
            OnEnd = delegate { };
        }
        else
        {
            Debug.LogWarning("El OnEnd es null");
        }

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
        if (data == null) yield break;
        SceneData.SceneParameter[] parameters = data.sceneparam;

        //HashSet<string> currents_neigbors = new HashSet<string>();
        for (int i = 0; i < parameters.Length; i++)
        {
            
            if (!loaded.Contains(parameters[i].scene))
            {
                yield return LoadAsyncAdditive(parameters[i].scene, false, false, false);
            }

            TryToExecuteParameter(parameters[i].scene, parameters[i].detail);
        }
        
        HashSet<string> to_unload = new HashSet<string>(loaded);
        HashSet<string> neigbors = new HashSet<string>();
        for (int i = 0; i < parameters.Length; i++)
        {
            neigbors.Add(parameters[i].scene);
        }
        to_unload.ExceptWith(neigbors);
        to_unload.Remove(currentScene);
        
        foreach (var u in to_unload)//cambiarlo por algo mas performante
        {
            if (localref.ContainsKey(u))
            {
                yield return localref[u].ExecuteLoadParameter(SceneData.Detail_Parameter.top_to_landmark);
            }

            #region en desuso, era para descargar las que estaban lejos
            //Destroy(GameObject.Find(u));
            //loaded.Remove(u);
            //var op = SceneManager.UnloadSceneAsync(u);
            //while (op.progress < 0.9f)
            //{
            //    yield return null;
            //}
            //yield return op;
            //LocalToEnemyManager.OnUnLoadScene(u);
            #endregion


        }

    }

    IEnumerator LoadAsyncAdditive(string sceneName, bool LoadScreen = false, bool LoadNeighbor = false, bool exe = true)
    {
        if (LoadScreen)
        {
            Fades_Screens.instance.Black();
            //LoadSceneHandler.instance.On_LoadScreen();
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
            //Fades_Screens.instance.FadeOff(() => { });

            
            yield return asyncOperation;
        }

        yield return null;
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

    GameObject current;
    void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
    {
        loading.Remove(scene.name);
        loaded.Add(scene.name);

        if(!scenes.ContainsKey(scene.name)) scenes.Add(scene.name, scene);

        current = null;
        var aux = scene.GetRootGameObjects();
        for (int i = 0; i < aux.Length; i++)
        {
            if (aux[i].name == scene.name)
            {
                current = aux[i];
            }
        }

        if (current != null)
        {
            var localscript = current.GetComponent<LocalSceneHandler>();
            if (localscript != null)
            {

                RegisterLocalScene(scene.name, localscript);
                StartCoroutine(localscript.Load());
            }
            else
            {
                throw new System.Exception("recibí una escena sin LocalSceneHandler");
            }
        }
        ExecuteParameterByScene(scene.name);
    }
    void OnSceneUnLoaded(Scene scene)
    {
        UnregisterLocalScene(scene.name);
    }
    #endregion

}
