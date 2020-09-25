using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalToEnemyManager : MonoBehaviour
{
    public static LocalToEnemyManager instance;
    public static void ResetScenes(string[] scenes) { instance.Reset(scenes); }
    public static void OnLoadScene(string scenes) { instance.LoadScene(scenes); }

    void Reset(string[] scenes)
    {
        // <-----
    }

    void LoadScene(string scene)
    {
        // <-----
    }
}
