using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DevelopTools.UI;

public class Checkpoint_Manager : MonoBehaviour
{
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
    
    public void SpawnChar()
    {
        Main.instance.GetChar().transform.position = _activeCheckPoint.transform.position;
    }
}
