using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneAsync : MonoBehaviour
{
    string SceneToLoad;
    public void BeginLoad(string scene)
    {
        SceneToLoad = scene;
        StartCoroutine(LoadAsyncScene());
    }

    AsyncOperation asyncLoad;
    IEnumerator LoadAsyncScene()
    {
        bool canLoad = true;

        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            if (SceneToLoad == SceneManager.GetSceneAt(i).name)
            {
                canLoad = false;
            }
        }

        if (!canLoad) yield break;

        if (SceneManager.GetActiveScene().name != SceneToLoad)
        {
            
            asyncLoad = SceneManager.LoadSceneAsync(SceneToLoad, LoadSceneMode.Additive);

            asyncLoad.allowSceneActivation = true;

            yield return null;
        }
    }
}
