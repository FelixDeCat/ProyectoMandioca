using UnityEngine;
using UnityEngine.Events;
public class GameLoop : MonoBehaviour
{
    public static GameLoop instance; private void Awake() => instance = this;
    [Header("GameLoop Events")]
    public UnityEvent UE_OnStartGame;
    public UnityEvent UE_OnStopGame;
    [Header("Transition Events")]
    public UnityEvent UE_OnTeleport;
    public void StartGame() { UE_OnStartGame.Invoke(); }
    public void BehindTeleportCheking() => UE_OnTeleport.Invoke();
    public void StopGame() => UE_OnStopGame.Invoke();


    [SerializeField] float wait_time_to_start = 0.5f;
    public void OnPlayerDeath()
    {
        Main.instance.eventManager.TriggerEvent(GameEvents.ON_PLAYER_DEATH);
        //pum! pantalla negra
        Fades_Screens.instance.Black();

        //mando lejo lejo al char para que no moleste
        Main.instance.GetChar().transform.position = new Vector3(5000, 0, 5000);

        //un tiempito super chiquito para que no haya inconvenientes
        Invoke("ReloadGameProcess", wait_time_to_start);
    }

    void ReloadGameProcess()
    {
        //Activo todo el UI de carga
        LoadSceneHandler.instance.On_LoadScreen();

        //ahora que tengo el UI puedo Fadear y quitar la pantalla negra
        Fades_Screens.instance.FadeOff(() => { });
        AudioAmbienceSwitcher.instance.StopAll();

        //capturo la ultima escena que me guardó el checkpoint
        string scene_to_load = Checkpoint_Manager.instance.GetSceneToLoadFromCheckPoint();

        //si tengo una escena guardada, la recargo
        if (scene_to_load != "")
        {
            if (NewSceneStreamer.instance)
                NewSceneStreamer.instance.LoadScene(scene_to_load, OnSceneLoaded);
            else
                RestartGame();
        }
        //si no tengo nada guardado sigo recargando cosas
        else
        {
            Debug.LogWarning("Ojo que el checkpoint no tiene escena, se te va a teletransportar sin carga");
            RestartGame();
        }
    }

    //Esto es todo lo del Spawneo de Enemigos, Esto se va a ejecutar despues de que las escenas hayan sido cargadas
    void OnSceneLoaded()
    {
        if (EnemyManager.Instance)
            ThreadHandler.EnqueueProcess(
                new ThreadObject(EnemyManager.Instance.ExecuteSceneRebuildEnemies(Checkpoint_Manager.instance.GetSceneToLoadFromCheckPoint()), "Respawneando Enemigos", "null", RestartGame),
                RestartGame, Felito_CustomCollections.Priority.high);
        else
            RestartGame();
    }

    void RestartGame()
    {
        //reseteo la vida
        Main.instance.GetChar().Life.Heal_AllHealth();

        //reposiciono al character
        Checkpoint_Manager.instance.SpawnChar();

        //esto es para que la camara se posicione directamente y no haya una transicion brusca cuando estoy prendiendo la pantalla
        Main.instance.GetMyCamera().InstantPosition();

        //mando a negro
        Fades_Screens.instance.Black();

        //Desactivo el UI de Carga
        LoadSceneHandler.instance.Off_LoadScreen();

        //Ahora que tengo la pantalla de Juego puedo fadear la pantalla negra
        Fades_Screens.instance.FadeOff(() => { });

        //vuelvo a targetear el char al Combat Director
        Main.instance.GetCombatDirector().AddNewTarget(Main.instance.GetChar().transform);
        
        //le aviso al event manager que el player Spawneo... esto se esta usando para reiniciar a los bosses
        Main.instance.eventManager.TriggerEvent(GameEvents.ON_PLAYER_RESPAWN);

        //aca se resetean los eventos Scripteados (Brazalete, presentacion de Beto, Boss de Dungeon)
        if (Main.instance.GetScriptedEventManager()) Main.instance.GetScriptedEventManager().ResetEvents();


        Debug.Log("Entro al restart");
        //Prendo la música
        AudioAmbienceSwitcher.instance.PlayAll();
    }

}
