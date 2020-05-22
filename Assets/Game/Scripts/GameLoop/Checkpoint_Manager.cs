using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Checkpoint_Manager : MonoBehaviour
{
    public Checkpoint_Spot _activeCheckPoint;
    public List<Checkpoint_Spot> allCheckpoints = new List<Checkpoint_Spot>();
    private CharacterHead _hero;

    private void Start()
    {
        _hero = Main.instance.GetChar();
        _hero.Life.ADD_EVENT_Death(SpawnChar);
        RegisterAllCheckPoints();
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
    
    public void SpawnChar()
    {
        _hero.transform.position = _activeCheckPoint.transform.position;
        Debug.Log("Respawn");
    }
}
