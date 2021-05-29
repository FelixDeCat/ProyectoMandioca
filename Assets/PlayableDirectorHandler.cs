using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;


public class PlayableDirectorHandler : MonoBehaviour
{
    [SerializeField] PlayableDirector director;

    public string Scene_To_Load_OnFinish = "MainScene";

    private void Start()
    {
        if (LoadSceneHandler.instance)
        {
            LoadSceneHandler.instance.Off_LoadScreen();
            Fades_Screens.instance.Black();
            Fades_Screens.instance.FadeOff(OnFinishSceneLoad);
        }
        else
        {
            director.Play();
            director.stopped += OnFinishCinematic;
        }
    }

    public void OnFinishSceneLoad()
    {
        director.Play();
        director.stopped += OnFinishCinematic;
    }

    void OnFinishCinematic(PlayableDirector d)
    {
        if (Fades_Screens.instance)
        {
            Fades_Screens.instance.FadeOff(EndFade);
        }
        else
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(this.gameObject.scene.name);
        }
        
    }
    void EndFade()
    {
        LoadSceneHandler.instance.On_LoadScreen();
        CharacterInput inputs = Main.instance.GetChar().getInput;
        LoadSceneHandler.instance.LoadAScene(Scene_To_Load_OnFinish, false);
    }
}
