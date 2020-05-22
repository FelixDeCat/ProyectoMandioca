using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class TimeToExecute : MonoBehaviour
{
    public float timer = 1;
    public string scene = "level_to_change";
    public bool stayHere;
    void Start()
    {
        Invoke("Execute", timer);
    }
    void Execute()
    {
        if(!stayHere)
         SceneManager.LoadScene(scene);
    }
}
