using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System;
using TMPro;

namespace PixelCrushers.SceneStreamer
{
    [AddComponentMenu("Scene Streamer/Scene Streamer")]
    public class SceneStreamer : MonoBehaviour
    {
        [Tooltip("Max number of neighbors to load out from the current scene.")]
        public int maxNeighborDistance = 1;
        [Tooltip("(Failsafe) If scene doesn't load after this many seconds, stop waiting.")]
        public float maxLoadWaitTime = 10f;

        [System.Serializable] public class StringEvent : UnityEvent<string> { }
        [SerializeField] StringEvent onLoaded = new StringEvent();
        [System.Serializable] public class StringAsyncEvent : UnityEvent<string, AsyncOperation> { }
        [SerializeField] StringAsyncEvent onLoading = new StringAsyncEvent();
        // private delegate void InternalLoadedHandler(string sceneName, int distance);

        [Tooltip("Tick to log debug info to the Console window.")]
        public bool debug = false;
        public bool logDebugInfo { get { return debug && Debug.isDebugBuild; } }
        private string m_currentSceneName = null;

        private HashSet<string> m_loaded = new HashSet<string>();
        private HashSet<string> m_loading = new HashSet<string>();
        private HashSet<string> m_near = new HashSet<string>();
        private HashSet<string> begined = new HashSet<string>();
        private HashSet<string> myCurrentVecinos = new HashSet<string>();

        #region instance things
        private static object s_lock = new object();
        private static SceneStreamer s_instance = null;
        private static SceneStreamer instance
        {
            get
            {
                lock (s_lock)
                {
                    if (s_instance == null)
                    {
                        s_instance = FindObjectOfType(typeof(SceneStreamer)) as SceneStreamer;
                        if (s_instance == null) s_instance = new GameObject("Scene Loader", typeof(SceneStreamer)).GetComponent<SceneStreamer>();
                    }
                    return s_instance;
                }
            }
            set { s_instance = value; }
        }
        public void Awake()
        {
            if (s_instance) Destroy(this);
            else { s_instance = this; UnityEngine.Object.DontDestroyOnLoad(this.gameObject); }
        }
        #endregion
        public void SetCurrent(string sceneName)
        {
            if (string.IsNullOrEmpty(sceneName) || string.Equals(sceneName, m_currentSceneName)) return;
            if (logDebugInfo) Debug.Log("Scene Streamer: Setting current scene to " + sceneName + ".");
            StartCoroutine(LoadCurrentScene(sceneName));
        }
        IEnumerator LoadCurrentScene(string sceneName)
        {
            Debug.Log("Inicio carga");
            m_currentSceneName = sceneName;
            if (!IsLoaded(m_currentSceneName)) Load(sceneName);

            //bloque de checkeo de conjunto y de tiempo para continuar
            float failsafeTime = Time.realtimeSinceStartup + maxLoadWaitTime;
            while ((m_loading.Count > 0) && (Time.realtimeSinceStartup < failsafeTime)) { yield return null; }
            if (Time.realtimeSinceStartup >= failsafeTime && Debug.isDebugBuild) Debug.LogWarning("Scene Streamer: Timed out waiting to load " + sceneName + ".");

            if (logDebugInfo) Debug.Log("Scene Streamer: Loading " + maxNeighborDistance + " closest neighbors of " + sceneName + ".");
            m_near.Clear();
            LoadNeighbors(sceneName, 0);

            //bloque de checkeo de conjunto y de tiempo para continuar
            failsafeTime = Time.realtimeSinceStartup + maxLoadWaitTime;
            while ((m_loading.Count > 0) && (Time.realtimeSinceStartup < failsafeTime)) yield return null;
            if (Time.realtimeSinceStartup >= failsafeTime && Debug.isDebugBuild) Debug.LogWarning("Scene Streamer: Timed out waiting to load neighbors of " + sceneName + ".");

            UnloadFarScenes();// Finally unload any scenes not in the near list:
            Debug.Log("Termino Carga");
        }
        void LoadNeighbors(string sceneName, int distance)
        {
            //checkeos
            if (m_near.Contains(sceneName)) return;
            m_near.Add(sceneName);
            if (distance >= maxNeighborDistance) return;

            //busco la escena y el componente de vecindario
            GameObject scene = GameObject.Find(sceneName);
            NeighboringScenes neighboringScenes = (scene) ? scene.GetComponent<NeighboringScenes>() : null;



            //por si acaso no tiene el componente se lo asigna al GO que tenga el nombre de la escena
            //y si tiene edges adentro los va asignando de acuerdo a las refes de los vecinos
            if (!neighboringScenes) neighboringScenes = CreateNeighboringScenesList(scene);
            if (!neighboringScenes) return;

            //agarro todos mis vecinos y los cargo
            //le sumo +1 asi cuando llega al maximo esta misma funcion corta
            for (int i = 0; i < neighboringScenes.sceneNames.Length; i++)
            {
                Load(neighboringScenes.sceneNames[i], LoadNeighbors, distance + 1);
            }
        }
        HashSet<string> neighbors;
        NeighboringScenes CreateNeighboringScenesList(GameObject scene)
        {
            if (!scene) return null;
            NeighboringScenes neighboringScenes = scene.AddComponent<NeighboringScenes>();
            neighbors = new HashSet<string>();
            var sceneEdges = scene.GetComponentsInChildren<SceneEdge>();
            for (int i = 0; i < sceneEdges.Length; i++) neighbors.Add(sceneEdges[i].nextSceneName);
            neighboringScenes.sceneNames = new string[neighbors.Count];
            neighbors.CopyTo(neighboringScenes.sceneNames);
            return neighboringScenes;
        }
        bool IsLoaded(string sceneName) => m_loaded.Contains(sceneName);
        bool IsLoading(string sceneName) => m_loading.Contains(sceneName);
        bool IsBegined(string sceneName) => begined.Contains(sceneName);
        void Load(string sceneName) => Load(m_currentSceneName, null, 0);
        void Load(string sceneName, Action<string, int> loadedHandler, int distance)
        {
            if (IsLoaded(sceneName))
            {
                //si ya esta cargado ejecuto el action y corto la ejecucion acá nomás
                if (loadedHandler != null) loadedHandler(sceneName, distance);
                return;
            }

            StartCoroutine(LoadAdditiveAsync(sceneName, loadedHandler, distance));
        }
        IEnumerator LoadAdditiveAsync(string sceneName, Action<string, int> loadedHandler, int distance)
        {
            if (!IsLoading(sceneName))
            {
                m_loading.Add(sceneName);

                AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

                // asyncOperation.allowSceneActivation = false;
                while (asyncOperation.progress < 0.9f)
                {
                    //onLoading.Invoke(sceneName, asyncOperation);
                    yield return null;
                }

                yield return asyncOperation;
                FinishLoad(sceneName, loadedHandler, distance);
            }
            else
            {
                FinishLoad(sceneName, loadedHandler, distance);
            }
        }

        void FinishLoad(string sceneName, Action<string, int> loadedHandler, int distance)
        {
            GameObject scene = GameObject.Find(sceneName);
            m_loading.Remove(sceneName);
            m_loaded.Add(sceneName);
            onLoaded.Invoke(sceneName);
            if (loadedHandler != null) loadedHandler(sceneName, distance);
        }
        void UnloadFarScenes()
        {
            Debug.Log("INicio: " + m_loaded.Count);
            HashSet<string> hash_far = new HashSet<string>(m_loaded);
            Debug.Log("fin: " + m_loaded.Count);
            hash_far.ExceptWith(m_near);
            hash_far.ExceptWith(neighbors);

            foreach (var sceneName in hash_far)
            {
                Unload(sceneName);
            }
        }
        void Unload(string sceneName)
        {
            
            Destroy(GameObject.Find(sceneName));
            m_loaded.Remove(sceneName);
            SceneManager.UnloadSceneAsync(sceneName);
        }
        public static void SetCurrentScene(string sceneName) => instance.SetCurrent(sceneName);
        public static bool IsSceneLoaded(string sceneName) => instance.IsLoaded(sceneName);
        public static void LoadScene(string sceneName) => instance.Load(sceneName);
        public static void UnloadScene(string sceneName) => instance.Unload(sceneName);
        bool AlreadyExist(string scene)
        {
            bool exist = false;
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                if (SceneManager.GetSceneAt(i).name == scene) exist = true;
            }
            return exist;
        }

        //private IEnumerator LoadAdditive(string sceneName, InternalLoadedHandler loadedHandler, int distance) {
        //    if (AlreadyExist(sceneName)) yield break;
        //    SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        //    onLoading.Invoke(sceneName, null);
        //    yield return new WaitForEndOfFrame();
        //    yield return new WaitForEndOfFrame();
        //    FinishLoad(sceneName, loadedHandler, distance);
        //}
    }
}