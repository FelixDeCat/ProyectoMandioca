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

    public Checkpoint current;
    public Transform spawnpoint;
    public List<Checkpoint> AllCheckPoint = new List<Checkpoint>();

    public void SubscribeSpawnPoint(Transform spawnpoint) => this.spawnpoint = spawnpoint;
    public void ConfigureCheckPoint(Checkpoint checkpoint, ref Action<Checkpoint> callback)
    {
        AllCheckPoint.Add(checkpoint);
        if (current == null) current = checkpoint;
        callback = x => current = x;
    }

    public void StopGame()
    {
        var chr = Main.instance.GetChar();
        chr.StopMovement();
    }

    public void StartGame()
    {
        current = AllCheckPoint[0];
        SpawnChar();
    }

    public void SpawnChar()
    {
        var chr = Main.instance.GetChar();
        chr.StopMovement();
        chr.GetBackControl();

        if (current != null)
        {
            if (!string.IsNullOrEmpty(current.sceneName))
            {
                Debug.Log("Un checkpoint con escena para cargar");
                NewSceneStreamer.instance.LoadScene(current.sceneName, true, true, EndLoadScene);
            }
            else
            {
                Debug.Log("Estoy recibiendo una escena nula, solo voy a posicionar al char");
                PositionateChar(current.Mytranform);
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

        Main.instance.GetCombatDirector().AddNewTarget(Main.instance.GetChar().transform);
        Main.instance.GetMyCamera().InstantPosition();
        //Invoke("Wait", 1f);
    }

    void EndLoadScene()
    {
        PositionateChar(current.Mytranform);
    }

    void PositionateChar(Transform tr = null)
    {
        var chr = Main.instance.GetChar();
        chr.transform.position = tr != null ? tr.position : Vector3.zero;
        chr.Root.eulerAngles = tr != null ? tr.eulerAngles : Vector3.zero;
        chr.GetComponentInChildren<CameraRotate>().CameraStartPosition();
        Main.instance.GetMyCamera().InstantPosition();
    }
    public void Wait()
    {
       // Fades_Screens.instance.FadeOff(() => { });
    }
}

