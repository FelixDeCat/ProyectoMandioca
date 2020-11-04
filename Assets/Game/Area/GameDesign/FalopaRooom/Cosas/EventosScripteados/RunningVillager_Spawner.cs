using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunningVillager_Spawner : MonoBehaviour
{
    [SerializeField] public List<NPCFleing> npcs;
    [SerializeField] public List<NPCFleing> npcsDisponibles;
    [SerializeField] public TriggerNpcs triggerNpcArrived;

    public PointToGo finalDest;
    public Transform startingPos;

    bool isActive;

    private void Start()
    {
        foreach (var item in npcsDisponibles)
        {
            item.GetComponent<RigidbodyPathFinder>().Initialize(item.GetComponent<Rigidbody>());
        }

        triggerNpcArrived.onArrived += StartNowRunning;
        triggerNpcArrived.onArrived_returnObject += ReturnVillager;

    }

    public void ActiveSpawner()
    {
        isActive = true;
        StartNowRunning();
    }

    public void DeactivateSpawner()
    {
        isActive = false;
    }

    public void StartNowRunning()
    {
        if (!isActive) return;

        if (npcsDisponibles.Count <= 0) return;

        NPCFleing chosen = npcsDisponibles[Random.Range(0, npcsDisponibles.Count)];
        npcsDisponibles.Remove(chosen);
        npcs.Add(chosen);

        chosen.gameObject.SetActive(true);
        chosen.GoToPos_RunningDesesperated();

        
    }

    IEnumerator WaitAndSpawn()
    {
        yield return new WaitForSeconds(3);

        StartNowRunning();
    }

    public void ReturnVillager(NPCFleing po)
    {
        po.gameObject.SetActive(false);
        po.transform.position = startingPos.position;
        npcs.Remove(po);
        npcsDisponibles.Add(po);

        

        //StartCoroutine(WaitAndSpawn());
    }
}
