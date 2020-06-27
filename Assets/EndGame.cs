using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : MonoBehaviour
{
    public void Start()
    {
        var aux = FindObjectsOfType<DontDestroy>();
        for (int i = 0; i < aux.Length; i++)
        {
            Destroy(aux[i].gameObject);
        }
    }

    public void BTN_Replay()
    {
        Scenes.Load_Load();
    }
    public void BTN_EndGame()
    {
        Application.Quit();
    }
}
