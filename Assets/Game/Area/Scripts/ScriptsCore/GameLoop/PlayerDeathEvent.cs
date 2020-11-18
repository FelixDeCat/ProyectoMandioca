using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathEvent : MonoBehaviour
{
    [SerializeField] float wait_time_to_start = 0.5f;

    public void UE_EVENT_PlayerDeath()
    {
        Fades_Screens.instance.Black();
        Main.instance.GetChar().transform.position = new Vector3(5000,0,5000);
        Invoke("BeginEvent", wait_time_to_start);
    }

    void BeginEvent()
    {
        LoadSceneHandler.instance.On_LoadScreen();
        Fades_Screens.instance.FadeOff(() => { });

        Main.instance.GetChar().Life.Heal_AllHealth();
        string scene_to_load = Checkpoint_Manager.instance.GetSceneToLoadFromCheckPoint();

        if (scene_to_load != "")
        {
            if (NewSceneStreamer.instance)
                NewSceneStreamer.instance.LoadScene(scene_to_load, OnSceneLoaded);
            else
                RestartGame();
        }
        else
        {
            Debug.LogWarning("Ojo que el checkpoint no tiene escena, se te va a teletransportar sin carga");
            RestartGame();
        }
    }

    void OnSceneLoaded()
    {
        if (EnemyManager.Instance)
            ThreadHandler.EnqueueProcess(
                new ThreadObject(EnemyManager.Instance.ExecuteSceneRebuildEnemies(Checkpoint_Manager.instance.GetSceneToLoadFromCheckPoint()), "Respawneando Enemigos","null", RestartGame), 
                RestartGame);
        else 
            RestartGame();
    }
    
    void RestartGame()
    {
        Debug.Log("Restart");
        Checkpoint_Manager.instance.SpawnChar();
        Main.instance.GetCombatDirector().AddNewTarget(Main.instance.GetChar().transform);
        Main.instance.GetMyCamera().InstantPosition();
        Fades_Screens.instance.Black();
        Fades_Screens.instance.FadeOff(() => { });
        LoadSceneHandler.instance.Off_LoadScreen();
    }
}
