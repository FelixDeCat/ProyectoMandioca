using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class NewSceneStreamer : MonoBehaviour
{
    public static NewSceneStreamer instance;
    private void Awake() => instance = this;

    public HashSet<string> loaded = new HashSet<string>();
    public HashSet<string> loading = new HashSet<string>();
    public string currentScene;

    [SerializeField] float maxLoadWaitTime;

    bool IsLoaded(string sceneName) => loaded.Contains(sceneName);
    bool IsLoading(string sceneName) => loading.Contains(sceneName);

    public void LoadScene(string sceneName, bool LoadScreen = false, bool LoadNeighbor = false, Action OnEnd = null)
    {
        if (string.IsNullOrEmpty(sceneName) || string.Equals(sceneName, currentScene)) return;
        StartCoroutine(LoadCurrentScene(sceneName,LoadScreen, LoadNeighbor, OnEnd));
    }

    IEnumerator LoadCurrentScene(string sceneName, bool LoadScreen = false, bool LoadNeighbor = false, Action OnEnd = null)
    {
        currentScene = sceneName;
        if (!IsLoaded(currentScene)) yield return LoadAsyncAdditive(sceneName, LoadScreen, LoadNeighbor, OnEnd);

        float failsafeTime = Time.realtimeSinceStartup + maxLoadWaitTime;
        while ((loading.Count > 0) && (Time.realtimeSinceStartup < failsafeTime)) { yield return null; }

        yield return null;
    }


    IEnumerator LoadAsyncAdditive(string sceneName, bool LoadScreen = false, bool LoadNeighbor = false, Action OnEnd = null)
    {
        if (LoadScreen)
        {
            Fades_Screens.instance.Black();
            LoadSceneHandler.instance.On_LoadScreen();
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

    }

}
