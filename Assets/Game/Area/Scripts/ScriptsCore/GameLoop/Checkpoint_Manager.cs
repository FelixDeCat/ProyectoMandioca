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
            currentImportant = checkpoint; 
        }
    }


    public void StartGame()
    {
        Fades_Screens.instance.Black();
        Fades_Screens.instance.FadeOff(() => { });

        currentNormal = AllCheckPoint[0];
        SpawnChar();
    }

    public void SpawnChar(bool important = false)
    {
        var chr = Main.instance.GetChar();

        Fades_Screens.instance.Black();
        chr.StopMovement();

        var togo = important ? currentImportant : currentNormal;

        if (togo != null)
        {
            Debug.Log("tengo checkpoint");
            chr.transform.position = togo.Mytranform.position;
            chr.transform.eulerAngles = togo.Mytranform.eulerAngles;
        }
        else
        {
            if (spawnpoint)
            {
                Debug.Log("tengo spawpoint");
                chr.transform.position = spawnpoint.position;
                chr.transform.eulerAngles = spawnpoint.eulerAngles;
            }
            else
            {
                Debug.Log("no tengo nada");
                chr.transform.position = Vector3.zero;
                chr.transform.eulerAngles = Vector3.zero;
            }
        }

        Main.instance.GetCombatDirector().AddNewTarget(Main.instance.GetChar());
        Main.instance.GetMyCamera().InstantPosition();
        Invoke("Wait", 0.75f);
    }
    public void Wait()
    {
        Fades_Screens.instance.FadeOff(() => { });
    }
}

