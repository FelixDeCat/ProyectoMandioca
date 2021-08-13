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
    }

    void OnEnd()
    {
        PauseManager.Instance.ReturnToMenu();
    }
}
