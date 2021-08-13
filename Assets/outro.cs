using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class outro : MonoBehaviour
{
    public void PlayOutro()
    {
        VideoCamera.Play("outro", OnEnd);
        AudioAmbienceSwitcher.instance.StopAll();
        Main.instance.GetChar().getInput.inMenu = true;
        StartCoroutine(FuncionFeisima());
    }
    bool oneshot;
    void OnEnd()
    {
        if (oneshot) return;
        oneshot = true;
        PauseManager.Instance.ReturnToMenu();
    }
    IEnumerator FuncionFeisima()
    {
        yield return new WaitForSeconds(123.4f);
        VideoCamera.instance.SkipVideo();
    }
}
