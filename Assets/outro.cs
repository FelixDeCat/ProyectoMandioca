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
            
    }

    void OnEnd()
    {
        PauseManager.Instance.ReturnToMenu();
        var myGameCores = FindObjectsOfType<DontDestroy>().Where(x => x.transform != transform.parent).ToArray();
        NewSceneStreamer.instance?.RemoveToSceneLoaded();

        for (int i = 0; i < myGameCores.Length; i++)
            Destroy(myGameCores[i].gameObject);
        SceneManager.LoadScene(0);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Destroy(transform.parent.gameObject);
    }
}
