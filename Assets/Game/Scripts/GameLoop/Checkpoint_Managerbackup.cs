using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

//los checkpoint podemos hacer que sean locales
//o tal vez como habias dicho un transform que adentro tenga todo... cosa que lo busco por escena
public class Checkpoint_Managerbackup : MonoBehaviour
{
    public Checkpoint_Spot _activeCheckPoint;
    public List<Checkpoint_Spot> allCheckpoints = new List<Checkpoint_Spot>();

    private void Start()
    {
        RegisterAllCheckPoints();
        //GameLoop.instance.SubscribeCheckpoint(this);
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
