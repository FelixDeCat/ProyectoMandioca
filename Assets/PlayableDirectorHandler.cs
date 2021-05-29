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
        LoadSceneHandler.instance.Off_LoadScreen();
        Fades_Screens.instance.Black();
        Fades_Screens.instance.FadeOff(OnFinishSceneLoad);
    }

    public void OnFinishSceneLoad()
    {
        director.Play();
        director.stopped += OnFinishCinematic;
    }

    void OnFinishCinematic(PlayableDirector d)
    {
        Fades_Screens.instance.FadeOff(EndFade);
    }
    void EndFade()
    {
        LoadSceneHandler.instance.On_LoadScreen();
        CharacterInput inputs = Main.instance.GetChar().getInput;
        LoadSceneHandler.instance.LoadAScene(Scene_To_Load_OnFinish, false);
    }
}
