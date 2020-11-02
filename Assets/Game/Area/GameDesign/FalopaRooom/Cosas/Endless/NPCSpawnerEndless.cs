using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSpawnerEndless : MonoBehaviour
{
    [SerializeField] GameObject pelotudosASpawnear;
    [SerializeField] Transform spawnPoint1;
    [SerializeField] PointToGo endPoint;
    [SerializeField] float cdSpawn;
    [SerializeField] int taradosToSpawn;
    [SerializeField] int taradosToWin;
    int taradosSpawned;
    List<GameObject> NPCAlives = new List<GameObject>();

    public void SpawnTarados()
    {
        if (taradosSpawned < taradosToWin)
        {
            for (int i = 0; i < taradosToSpawn; i++)
            {
                GameObject Spawned = GameObject.Instantiate(pelotudosASpawnear);
                NPCAlives.Add(Spawned);
                Spawned.transform.position = spawnPoint1.transform.position + new Vector3(Random.Range(-4,4),0,Random.Range(-2,2));
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
    }

}
