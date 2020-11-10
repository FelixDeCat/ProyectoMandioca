using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SaveVillageManager : MonoBehaviour
{
    public int[] villagersNeededPerPhase;
    [SerializeField] int villagersPerGroup = 6;
    [SerializeField] float timeBetweenVillSamegroup = 0.3f;
    public int minEnemiesToSpawnNextWave = 3;

    [SerializeField] GameObject villagerPrefab;
    [SerializeField] LayerMask mask_hit_floor;
    int currentVillagerCount = 0;
    public int currentPhase { get; private set; }

    [SerializeField] Transform[] spawnPoints;
    [SerializeField] PointToGo endPoint;

    public VillageEventState gameState { get; private set; }
    [SerializeField] UnityEvent OnEventStarted;
    [SerializeField] UnityEvent OnPhaseChanged;
    [SerializeField] UnityEvent OnVillagerArrived;
    [SerializeField] UnityEvent OnEventCompleted;

    List<EnemyBase> currentEnemies = new List<EnemyBase>();
    List<NPCFleing> currentVillagers = new List<NPCFleing>();

    public void InitializeVillage()
    {
        Main.instance.SetVillageManager(this);
        SetCurrentState(VillageEventState.Disabled);
        OnEventCompleted.AddListener(() => GenCounter.CloseCounter());
    }

    public void AddEnemy(EnemyBase enemy)
    {
        currentEnemies.Add(enemy);
    }
    public void RemoveEnemy(EnemyBase enemy)
    {
        if (currentEnemies.Contains(enemy))
        {
            currentEnemies.Remove(enemy);
        }
    }
    public int GetCurrentEnemiesCount() => currentEnemies.Count;

    public void SetCurrentState(VillageEventState state) => gameState = state;

    public void AddPhase()
    {
        currentPhase++;
        if (currentPhase >= villagersNeededPerPhase.Length)
        {
            OnEventCompleted.Invoke();
            SetCurrentState(VillageEventState.LevelCompleted);
        }
        else
        {
            OnPhaseChanged.Invoke();
        }
    }

    public void StartEvent()
    {
        OnEventStarted.Invoke();
        GenCounter.OpenCounter();
        GenCounter.SetCounterInfo("Villagers", currentVillagerCount, villagersNeededPerPhase[villagersNeededPerPhase.Length-1], true);
        gameState = VillageEventState.Start;
        StartCoroutine(SpawnVillagers());
    }

    IEnumerator SpawnVillagers()
    {
        while (currentVillagerCount < villagersNeededPerPhase[villagersNeededPerPhase.Length - 1])
        {
            for (int i = 0; i < villagersPerGroup; i++)
            {
                GameObject Spawned = Instantiate(villagerPrefab);

                Vector3 posToSpawn = spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position + new Vector3(Random.Range(-4, 4), 0, Random.Range(-2, 2));

                RaycastHit hit;
                if (Physics.Raycast(posToSpawn + Spawned.transform.up * 10, Spawned.transform.up * -1, out hit, 30, mask_hit_floor))
                {
                    Spawned.transform.position = hit.point;
                }

                NPCFleing npc = Spawned.GetComponent<NPCFleing>();
                npc.Initialize();
                npc.On();
                AddToVillagersAlive(npc);

                npc.pos_exit_endless = endPoint;
                npc.GoToPos_RunningDesesperated();
                yield return new WaitForSeconds(timeBetweenVillSamegroup);
                ReTarget();
            }
            while (currentVillagers.Count > 0)
                yield return new WaitForSeconds(0.1f);
        }

    }

    public void ClampToFloor()
    {
        
    }

    public void AddVillager(NPCFleing npc)
    {
        currentVillagerCount++;
        if (currentVillagers.Contains(npc))
        {
            OnVillagerArrived.Invoke();
            currentVillagers.Remove(npc);
            GenCounter.SetCounterInfo("Villagers", currentVillagerCount, villagersNeededPerPhase[villagersNeededPerPhase.Length-1], true);
        }

        if (currentPhase != villagersNeededPerPhase.Length && currentVillagerCount >= villagersNeededPerPhase[currentPhase] )
        {
            AddPhase();
        }
    }

    public void AddToVillagersAlive(NPCFleing npc)
    {
        currentVillagers.Add(npc);
    }
    public void RemoveFromVillagersAlive(NPCFleing npc)
    {
        currentVillagers.Remove(npc);
    }

    public void ReTarget()
    {
        for (int i = 0; i < currentEnemies.Count; i++)
        {
            currentEnemies[i].GetComponent<CombatDirectorElement>().ChangeTarget(nearestNPC(currentEnemies[i].transform.position).transform);
        }
    }

    public WalkingEntity nearestNPC(Vector3 position)
    {

        WalkingEntity nearest = Main.instance.GetChar(); 
        if (currentVillagers.Count > 0)
        {
            for (int i = 0; i < currentVillagers.Count; i++)
            {
                if(Vector3.Distance(currentVillagers[i].transform.position, position) < Vector3.Distance(nearest.transform.position, position))
                {
                    nearest = currentVillagers[i];
                }
            }
        }
        return nearest;
    }
}



public enum VillageEventState { Disabled, Start, SpawningWave, WaveInProgress, WaitingToSpawn, LevelCompleted };
/// <EstadosDelJuego>
/// Start: inicio del juego.
/// Spawning: Durante la creacion de la wave.
/// InProgress: Una vez que la wave ya fue completamente creada -> Cuando se mueren todos los enemigos de la wave, el estado pasa a Waiting.
/// </summary>

