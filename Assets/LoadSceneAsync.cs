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
        Main.instance.IgnoreCheckPoints();
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
            
            asyncLoad = SceneManager.LoadSceneAsync(SceneToLoad, LoadSceneMode.Single);
            yield return new WaitForEndOfFrame();

            while (!asyncLoad.isDone)
            {
                yield return new WaitForEndOfFrame();
                if (asyncLoad.progress >= 0.9f)
                {
                    asyncLoad.allowSceneActivation = true;
                }
            }

            //yield return new WaitUntil(() => asyncLoad.isDone);

            //asyncLoad.allowSceneActivation = true;

            yield return null;
        }
    }
}
