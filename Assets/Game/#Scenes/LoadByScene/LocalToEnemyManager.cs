using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// esto lo tiré en gamecore, borrá esto cuando lo leas
/// </summary>

public class LocalToEnemyManager : MonoBehaviour
{
    public static LocalToEnemyManager instance; private void Awake() => instance = this;
    public static void ResetScenes(string[] scenes) { instance.Reset(scenes); }
    public static void OnLoadScene(string scene) { instance.LoadScene(scene); }
    public static void OnUnLoadScene(string scene) { instance.UnLoadScene(scene); }
    public static void SendEnemyToScene(string scene, EnemyBase enemy) => instance.SendEnemyScene(scene, enemy); 

    void Reset(string[] scenes)
    {
        if (scenes == null) return;
        for (int i = 0; i < scenes.Length; i++) EnemyManager.Instance?.OnResetState(scenes[i]);
        // <-----
    }
    void LoadScene(string scene)
    {
        // <-----
        Main.instance.eventManager.TriggerEvent(GameEvents.INTERACTABLES_INITIALIZE);
    }
    void UnLoadScene(string scene)
    {
        //EnemyManager.Instance.OnSaveStateEnemies(scene);
        // <-----
    }
    void SendEnemyScene(string scene, EnemyBase enemy)
    {
        EnemyManager.Instance.ChangeEnemyScene(scene, enemy);
    }
}

