using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private void Awake() { instance = this; }
    public PlayableScene currentScene;

    private void Update()
    {
        
    }

    public void Pause()
    {

    }
    public void Resume()
    {

    }

    void OnApplicationFocus(bool focus)
    {
        
    }

    void OnApplicationPause(bool pause)
    {
        
    }

    void OnApplicationQuit()
    {
        
    }
}
