using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private void Awake() { instance = this; }

    public void StartGame()
    {
        Debug.LogWarning("STARTGAME");
    }

    public void Pause()
    {

    }
    public void Resume()
    {

    }


    private void Update()
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
