using System;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint_Manager : MonoBehaviour
{
    public static Checkpoint_Manager instance;
    private void Awake() { if (instance == null) instance = this; else Destroy(this.gameObject); }

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

    public string GetSceneToLoadFromCheckPoint() { return current != null ? current.sceneName : ""; }

    public void SpawnChar()
    {
        var chr = Main.instance.GetChar();
        chr.StopMovement();
        chr.GetBackControl();
        chr.transform.position = current.Mytranform.position;
        chr.Root.localEulerAngles = current.Mytranform.localEulerAngles;
        chr.GetComponentInChildren<CameraRotate>().CameraStartPosition();
        Main.instance.GetMyCamera().InstantPosition();
    }
}

