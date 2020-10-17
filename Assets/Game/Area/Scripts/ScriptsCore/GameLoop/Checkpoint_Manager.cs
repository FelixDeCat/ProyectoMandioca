using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


/// <summary>
/// MODO DE USO
/// 
/// Tienen que tener en escena el CheckPoint manager.
/// Tiren por donde quieran los checkpoint_spots(los prefabs)
/// Cuando el personaje toque uno de esos checkpoints, se actualiza como el "current"
/// Al morir, aparece en ese lugar
///
///
/// TODO: agregarle un feedback de muerte (seguro sea una animacion de muerte o algo asi)
/// </summary>



public class Checkpoint_Manager : MonoBehaviour
{
    public static Checkpoint_Manager instance;
    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this.gameObject);
    }

    public Checkpoint currentNormal;
    public Checkpoint currentImportant;
    public Transform spawnpoint;
    public List<Checkpoint> AllCheckPoint = new List<Checkpoint>();

    public void SubscribeSpawnPoint(Transform spawnpoint) => this.spawnpoint = spawnpoint;
    public void ConfigureCheckPoint(Checkpoint checkpoint, ref Action<Checkpoint> callback)
    {
        AllCheckPoint.Add(checkpoint);
        if (currentNormal == null && !checkpoint.IsImportant) currentNormal = checkpoint;
        if (currentImportant == null && checkpoint.IsImportant) currentImportant = checkpoint;
        callback = SetSpawn;
    }

    void SetSpawn(Checkpoint checkpoint)
    {
        if (!checkpoint.IsImportant) 
        {
            currentNormal = checkpoint; 
        }
        else
        {
            currentNormal = checkpoint;
            currentImportant = checkpoint; 
        }
    }

    public void StopGame()
    {
        Fades_Screens.instance.Black(); Fades_Screens.instance.FadeOff(() => { });
        LoadSceneHandler.instance.On_LoadScreen();
        var chr = Main.instance.GetChar();
        chr.StopMovement();
        //chr.Pause();
    }

    public void StartGame()
    {
        Fades_Screens.instance.Black(); Fades_Screens.instance.FadeOff(() => { });
        LoadSceneHandler.instance.Off_LoadScreen();
        currentNormal = AllCheckPoint[0];
        SpawnChar();
        Debug.Log("Starte champion");
    }

    public void SpawnChar(bool important = false)
    {
        var chr = Main.instance.GetChar();

        chr.StopMovement();
        //chr.Resume();
        chr.GetBackControl();

        var togo = important ? currentImportant : currentNormal;

        if (togo != null)
        {
            if (togo.sceneName != "" && string.IsNullOrEmpty(togo.sceneName))
            {
                NewSceneStreamer.instance.LoadScene(togo.sceneName, true, true);
            }
            else
            {
                PositionateChar(togo.Mytranform);
            }
        }
        else
        {
            if (spawnpoint)
            {
                PositionateChar(spawnpoint);
            }
            else
            {
                PositionateChar();
            }
        }

        Main.instance.GetCombatDirector().AddNewTarget(Main.instance.GetChar());
        Main.instance.GetMyCamera().InstantPosition();
        Invoke("Wait", 1f);
    }

    void PositionateChar(Transform tr = null)
    {
        var chr = Main.instance.GetChar();
        chr.transform.position = tr != null ? tr.position : Vector3.zero;
        chr.transform.eulerAngles = tr != null ? tr.eulerAngles : Vector3.zero;
    }
    public void Wait()
    {
        Fades_Screens.instance.FadeOff(() => { });
    }
}

