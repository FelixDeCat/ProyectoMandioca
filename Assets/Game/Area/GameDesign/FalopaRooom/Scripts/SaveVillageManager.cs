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
    int currentVillagerCount = 0;
    int currentEnemiesAlive = 0;
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
   
    private void Start()
    {
        Main.instance.SetVillageManager(this);
        SetCurrentState(VillageEventState.Disabled);
    }

    public void AddEnemy(EnemyBase enemy)
    {
        currentEnemies.Add(enemy);
        currentEnemiesAlive++;
    }
    public void RemoveEnemy(EnemyBase enemy)
    {
        currentEnemiesAlive--;
        if (currentEnemies.Contains(enemy))
            currentEnemies.Remove(enemy);
    }
    public int GetCurrentEnemiesCount() => currentEnemiesAlive;

    public void SetCurrentState(VillageEventState state) => gameState = state;

    public void AddPhase()
    {
        currentPhase++;
        if (currentPhase >= villagersNeededPerPhase.Length - 1)
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
                Spawned.transform.position = spawnPoints[Random.Range(0,spawnPoints.Length)].transform.position + new Vector3(Random.Range(-4, 4), 0, Random.Range(-2, 2));
                NPCFleing npc = Spawned.GetComponent<NPCFleing>();
                npc.Initialize();
                currentVillagers.Add(npc);
                npc.pos_exit_endless = endPoint;
                npc.GoToPos_RunningDesesperated();
                yield return new WaitForSeconds(timeBetweenVillSamegroup);
            }

            while (currentVillagers.Count > 0)
                yield return new WaitForSeconds(0.1f);
        }

    }

    public void AddVillager(NPCFleing npc)
    {
        currentVillagerCount++;
        if (currentVillagers.Contains(npc))
        {
            OnVillagerArrived.Invoke();
            currentVillagers.Remove(npc);
        }

        if (currentPhase != villagersNeededPerPhase.Length && currentVillagerCount >= villagersNeededPerPhase[currentPhase] )
        {
            AddPhase();
        }
    }
}



public enum VillageEventState { Disabled, Start, SpawningWave, WaveInProgress, WaitingToSpawn, LevelCompleted };
/// <EstadosDelJuego>
/// Start: inicio del juego.
/// Spawning: Durante la creacion de la wave.
/// InProgress: Una vez que la wave ya fue completamente creada -> Cuando se mueren todos los enemigos de la wave, el estado pasa a Waiting.
/// </summary>

