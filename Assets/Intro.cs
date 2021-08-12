using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intro : MonoBehaviour
{

    void Start()
    {
        VideoCamera.Play("intro", OnEndVideo);
    }

    void OnEndVideo()
    {
        LoadSceneHandler.instance.On_LoadScreen();
        LoadSceneHandler.instance.LoadAScene("MainScene", false);
    }

}
