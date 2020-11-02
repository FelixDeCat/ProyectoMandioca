using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NPCSpawnerEndless : MonoBehaviour
{
    [SerializeField] GameObject pelotudosASpawnear;
    [SerializeField] Transform spawnPoint1;
    [SerializeField] Transform spawnPoint2;
    [SerializeField] Transform spawnPoint3;
    [SerializeField] PointToGo endPoint;
    [SerializeField] float cdSpawn;
    [SerializeField] int taradosToSpawn;
    [SerializeField] int taradosToWin;
    int taradosSpawned;
    Transform pointToSpawn;
    List<GameObject> NPCAlives = new List<GameObject>();
    Dictionary<int, Transform> spawnPointsToChoose = new Dictionary<int, Transform>();



    private void Start()
    {
        pointToSpawn = spawnPoint1;
        spawnPointsToChoose.Add(1,spawnPoint1);
        spawnPointsToChoose.Add(2,spawnPoint2);
        spawnPointsToChoose.Add(3,spawnPoint3);
    }

    public void SpawnTarados()
    {
        if (taradosSpawned < taradosToWin)
        {
            for (int i = 0; i < taradosToSpawn; i++)
            {

                GameObject Spawned = GameObject.Instantiate(pelotudosASpawnear);
                NPCAlives.Add(Spawned);
                Spawned.transform.position = spawnPointsToChoose[UnityEngine.Random.Range(1,3)].transform.position + new Vector3(UnityEngine.Random.Range(-5,5),0 , UnityEngine.Random.Range(-4,4));
                Spawned.GetComponent<NPCFleing>().Initialize();
                Spawned.GetComponent<NPCFleing>().pos_exit_endless = endPoint;
                Spawned.GetComponent<NPCFleing>().GoToPos_RunningDesesperated();
            }
            Invoke("SpawnTarados", cdSpawn);
        }
    }

    public void Update()
    {
        if (taradosSpawned > taradosToWin)
        {
            KillALL();
        }
    }

    public void KillALL()
    {
        for (int i = 0; i < NPCAlives.Count; i++)
        {
            NPCAlives[i].SetActive(false);
        }
    }

    public void AddMax()
    {
        taradosSpawned++;

        if(taradosSpawned > Main.instance.GetVillageManager().villagersNeededPerPhase[Main.instance.GetVillageManager().currentPhase])
        {
            Main.instance.GetVillageManager().AddPhase();
        }
    }

}
