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

    [SerializeField] Transform pos_anti_bug;

    [SerializeField] SceneData original_z1 = null;

    public HashSet<string> loaded = new HashSet<string>();
    public HashSet<string> loading = new HashSet<string>();
    public string currentScene;

    public string firstScene;

    Dictionary<string, LocalSceneHandler> localref = new Dictionary<string, LocalSceneHandler>();
    //Dictionary<string, PlayObjectParentInitializer> playObjectInitializer = new Dictionary<string, PlayObjectParentInitializer>();
    //Dictionary<string, SceneData.Detail_Parameter> hotScenesParameters = new Dictionary<string, SceneData.Detail_Parameter>();
    Dictionary<string, Scene> scenes = new Dictionary<string, Scene>();
    public Scene GetSceneByName(string sceneName) { return scenes[sceneName]; }

    [SerializeField] float maxLoadWaitTime = 5;

    public static void GotToAntiBugPosition() => Main.instance.GetChar().transform.position = instance.pos_anti_bug.transform.position;

    bool IsLoaded(string sceneName) => loaded.Contains(sceneName) || loaded.Contains(sceneName.ToLower()) || loaded.Contains(sceneName.ToUpper());
    bool IsLoading(string sceneName) => loading.Contains(sceneName) || loading.Contains(sceneName.ToLower()) || loading.Contains(sceneName.ToUpper());

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        LoadScene(firstScene, OnEndLoad);
    }

    public void RemoveToSceneLoaded() => SceneManager.sceneLoaded -= OnSceneLoaded;


    // { 4 destinatarios }
    // >> El Start de Esta Clase >> es la primera carga que va a recibir
    // >> El Gameloop >> cuando me muero y hago todo el reload
    // >> El Teleport >> cuando un teleport viene parametrizado con la escena que quiere cargar
    // >> Los Triggers de las Escenas >> Al pasar por uno, me manda la escena que quiere cargar
    public void LoadScene(string sceneName, Action OnEnd = null)
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            ThreadHandler.EnqueueProcess(new ThreadObject(LoadCurrentScene(sceneName), "Chunk => " + sceneName), OnEnd, Felito_CustomCollections.Priority.high);
        }
        else Debug.LogWarning("Me llego string de escena nulo o vacio");
    }

    void OnEndLoad()
    {
        Invoke("Adjustment", 1f);
    }
    void Adjustment()
    {
        ThreadHandler.EnqueueProcess(new ThreadObject(LastConfigurations(), "Iniciando..."), null, Felito_CustomCollections.Priority.high);
    }

    IEnumerator LastConfigurations()
    {
        HashSet<string> alreadyTreated = new HashSet<string>();
        alreadyTreated.Add("z1");

        for (int i = 0; i < original_z1.sceneparam.Length; i++)
        {
            yield return localref[original_z1.sceneparam[i].scene].ExecuteLoadParameter(original_z1.sceneparam[i].detail);
            alreadyTreated.Add(original_z1.sceneparam[i].scene);
        }
        foreach (var s in loaded)
        {
            if (!alreadyTreated.Contains(s))
            {
                localref[s].ExecuteLoadParameter(SceneData.Detail_Parameter.top_to_landmark);

                TryToExecuteParameter(s, SceneData.Detail_Parameter.top_to_landmark);
            }
            yield return null;
        }

        GameLoop.instance.StartGame();
    }



    IEnumerator LoadCurrentScene(string sceneName)
    {
        currentScene = sceneName;

        if (!IsLoaded(currentScene))
        {
            StartCoroutine(LoadAsyncAdditive(sceneName));
            yield return null;
        }

        float failsafeTime = Time.realtimeSinceStartup + maxLoadWaitTime;
        while ((loading.Count > 0) && (Time.realtimeSinceStartup < failsafeTime)) { yield return null; }

        while (!localref.ContainsKey(currentScene))
        {
            yield return null;
        }

        TryToExecuteParameter(sceneName, SceneData.Detail_Parameter.full_load);

        yield return LoadNeighbors(currentScene, localref[currentScene].SceneData);

        LocalToEnemyManager.OnLoadScene(sceneName);

        yield return null;
    }

    void TryToExecuteParameter(string sceneName, SceneData.Detail_Parameter parameter)
    {
        StartCoroutine(localref[sceneName].ExecuteLoadParameter(parameter));
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
                yield return LoadAsyncAdditive(parameters[i].scene);
                yield return null;
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
                //if (playObjectInitializer.ContainsKey(u))
                //{
                //    yield return playObjectInitializer[u].UnloadPlayObject();
                //    playObjectInitializer.Remove(u);
                //}
                yield return localref[u].ExecuteLoadParameter(SceneData.Detail_Parameter.top_to_landmark);
                LocalToEnemyManager.OnUnLoadScene(u);
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

    IEnumerator LoadAsyncAdditive(string sceneName)
    {
        if (!IsLoading(sceneName) && !IsLoaded(sceneName))
        {
            Debug.Log("Cargando: " + sceneName);
            loading.Add(sceneName);
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            MsgLogData msgLogData = new MsgLogData("Loading... " + sceneName, new Color(0, 0, 0, 0), new Color(1, 1, 1, 1), 1f);
            while (asyncOperation.progress < 0.9f)
            {
                yield return new WaitForEndOfFrame();
                ThreadHandler.AuxiliarDebug("Loading subscene " + sceneName + " => %" + asyncOperation.progress * 100);
                yield return null;
            }
            while (!asyncOperation.isDone)
            {
                yield return null;
            }
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
            localref[sceneName] = sceneLocalScript;
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
        StartCoroutine(SceneLoaded(scene));
    }

    IEnumerator SceneLoaded(Scene scene)
    {
        loading.Remove(scene.name);

        if (!scenes.ContainsKey(scene.name)) scenes.Add(scene.name, scene);

        current = null;
        var aux = scene.GetRootGameObjects();
        for (int i = 0; i < aux.Length; i++)
        {
            if (aux[i].name == scene.name)
            {
                current = aux[i];
                yield return null;
            }
        }

        if (current != null)
        {
            var localscript = current.GetComponent<LocalSceneHandler>();
            if (localscript != null)
            {

                RegisterLocalScene(scene.name, localscript);
                yield return localscript.Load();
                yield return null;

            }
            else
            {
                throw new System.Exception("recibí una escena sin LocalSceneHandler");
            }
        }
        //ExecuteParameterByScene(scene.name);

        loaded.Add(scene.name);
    }

    void OnSceneUnLoaded(Scene scene)
    {
        UnregisterLocalScene(scene.name);
    }
    #endregion

}
