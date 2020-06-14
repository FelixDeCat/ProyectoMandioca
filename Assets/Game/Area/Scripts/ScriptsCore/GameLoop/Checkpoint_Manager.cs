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
    public static Checkpoint_Manager instance; private void Awake() => instance = this;

    public Checkpoint_Spot _activeCheckPoint;
    public List<Checkpoint_Spot> allCheckpoints = new List<Checkpoint_Spot>();

    private void Start()
    {
        GameLoop.instance.SubscribeCheckpoint(this);
        RegisterAllCheckPoints();
        _activeCheckPoint = allCheckpoints[0];
    }

    void RegisterAllCheckPoints()
    {
        allCheckpoints = transform.GetComponentsInChildren<Checkpoint_Spot>().ToList();
        foreach (var cp in allCheckpoints)
        {
            cp.OnCheckPointActivated += UpdateCurrentCheckpoint;
        }
    }

    void UpdateCurrentCheckpoint(Checkpoint_Spot cp)
    {
        _activeCheckPoint = cp;
    }

    private void Update()
    {
        if (Main.instance.GetChar().transform.position.y < -10)
        {
           SpawnChar();
        }
    }

    public void SpawnChar()
    {
        Fades_Screens.instance.Black();
        Main.instance.GetChar().StopMovement();
        Main.instance.GetChar().transform.position = _activeCheckPoint.GetPosition;
        Main.instance.GetCombatDirector().AddNewTarget(Main.instance.GetChar());
        Main.instance.GetMyCamera().InstantPosition();
        Invoke("Wait", 0.75f);
    }
    public void Wait()
    {
        Fades_Screens.instance.FadeOff(()=> { });
    }
}

