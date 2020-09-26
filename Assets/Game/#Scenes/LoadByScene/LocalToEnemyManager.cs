using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalToEnemyManager : MonoBehaviour
{
    public static LocalToEnemyManager instance; private void Awake() => instance = this;
    public static void ResetScenes(string[] scenes) { instance.Reset(scenes); }
    public static void OnLoadScene(string scene) { instance.LoadScene(scene); }
    public static void OnUnLoadScene(string scene) { instance.UnLoadScene(scene); }
    public static void SendEnemyToScene(string scene, EnemyBase enemy) => instance.SendEnemyScene(scene, enemy); 

    void Reset(string[] scenes)
    {
        // <-----
    }
    void LoadScene(string scene)
    {
        // <-----
    }
    void UnLoadScene(string scene)
    {
        // <-----
    }
    void SendEnemyScene(string scene, EnemyBase enemy)
    {
        // <-----
    }
}

