using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class LoadAdditiveAsyncScene : MonoBehaviour
{
    public UnityEvent UE_OnEndLoad;
    public GenericBar_Sprites bar;
    public string scenename;
    bool oneshot = false;

    public void LoadLevel()
    {
        if (oneshot) return;
        oneshot = true;
        StartCoroutine(Load());
    }

    IEnumerator Load()
    {
        var op = SceneManager.LoadSceneAsync(scenename,LoadSceneMode.Additive);
        op.completed += OnEndLoad;

        bar.Configure(1, 0.01f);

        while (op.progress < 0.9f)
        {
            bar.SetValue(op.progress);
            yield return null;
        }
        yield return op;
    }

    void OnEndLoad(AsyncOperation op)
    {
        op.completed -= OnEndLoad;
        UE_OnEndLoad.Invoke();
    }
}
