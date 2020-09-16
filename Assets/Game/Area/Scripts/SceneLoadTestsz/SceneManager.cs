using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

namespace SM {

    public class SceneManager : MonoBehaviour {
        public SceneVault [] sceneFiles;
        public UnityEvent<string, AsyncOperation> onLoading;
        public static SceneManager _sceneManager;
        
        public void Awake() {
            if ( _sceneManager ) {
                Destroy(this);
            } else {
                _sceneManager = this;
                DontDestroyOnLoad(this.gameObject);
            }

        }

        public void LoadScene(int id ) {
            StartCoroutine(StartLoadingScene(id));
            
        }
        IEnumerator StartLoadingScene(int id ) {

            AsyncOperation asyncOperation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneFiles[id].sceneName, LoadSceneMode.Additive);
            onLoading.Invoke(sceneFiles [ id ].sceneName, asyncOperation);
            yield return asyncOperation;
            /*
            FinishLoad(sceneName, loadedHandler, distance);

            Debug.Log("terrain");
            //itemtoloadList?
            if ( sceneFiles [ id ].terrain )
                Instantiate(sceneFiles [ id ].terrain);
            yield return new WaitForFixedUpdate();
            Debug.Log("rocks");
            if ( sceneFiles [ id ].rocks )
                Instantiate(sceneFiles [ id ].rocks);
            yield return new WaitForFixedUpdate();
            Debug.Log("trees");
            if ( sceneFiles [ id ].trees )
                Instantiate(sceneFiles [ id ].trees);
                */
            yield return new WaitForFixedUpdate();

        }
        public void FinishLoad(string sceneName) {
            GameObject scene = GameObject.Find(sceneName);
            if ( scene == null && Debug.isDebugBuild ) Debug.LogWarning("Scene Streamer: Can't find loaded scene named '" + sceneName + "'.");

        }

    }

}
