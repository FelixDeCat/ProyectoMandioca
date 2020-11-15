using UnityEngine;

public class PlayerDeathEvent : MonoBehaviour
{
    [SerializeField] float wait_time_to_start = 0.5f;

    public void UE_EVENT_PlayerDeath()
    {
        Fades_Screens.instance.Black();
        Invoke("BeginEvent", wait_time_to_start);
    }

    void BeginEvent()
    {
        LoadSceneHandler.instance.On_LoadScreen();
        Fades_Screens.instance.FadeOff(() => { });

        Main.instance.GetChar().Life.Heal_AllHealth();
        string scene_to_load = Checkpoint_Manager.instance.GetSceneToLoadFromCheckPoint();
        if (NewSceneStreamer.instance)
        {
            NewSceneStreamer.instance.LoadScene(scene_to_load, OnSceneLoaded);
        }
        else
        {
            OnSceneLoaded();
        }
       
    }

    void OnSceneLoaded()
    {
        Checkpoint_Manager.instance.SpawnChar();
        Main.instance.GetCombatDirector().AddNewTarget(Main.instance.GetChar().transform);
        Main.instance.GetMyCamera().InstantPosition();
        Fades_Screens.instance.Black();
        Fades_Screens.instance.FadeOff(() => { });
        LoadSceneHandler.instance.Off_LoadScreen();
    }
}
