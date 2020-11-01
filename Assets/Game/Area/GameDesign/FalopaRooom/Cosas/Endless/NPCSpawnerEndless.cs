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

    public void SpawnTarados()
    {
        if (taradosSpawned < taradosToWin)
        {
            for (int i = 0; i < taradosToSpawn; i++)
            {
                taradosSpawned++;
                GameObject Spawned = GameObject.Instantiate(pelotudosASpawnear);
                Spawned.transform.position = spawnPoint1.transform.position;
                Spawned.GetComponent<Jacinta>().Initialize();
                Spawned.GetComponent<Jacinta>().pos_doctor = endPoint;
                Spawned.GetComponent<Jacinta>().GoToPos_Doctor();
            }
            Invoke("SpawnTarados", cdSpawn);
        }
    }
}
