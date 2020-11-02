using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveVillageManager : MonoBehaviour
{
    public int[] villagersNeededPerPhase;
    [SerializeField] int villagersPerGroup = 6;
    public int minEnemiesToSpawnNextWave = 3;

    int currentVillagerCount = 0;
    int currentEnemiesAlive = 0;
    public int currentPhase { get; private set; }
       
    public VillageEventState gameState { get; private set; }

    List<EnemyBase> currentEnemies = new List<EnemyBase>();
    NPCSpawnerEndless endless;
    
    private void Start()
    {
        Main.instance.SetVillageManager(this);
        gameState = VillageEventState.Disabled;
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
        if (currentPhase >= villagersNeededPerPhase.Length-1)
        {
            SetCurrentState(VillageEventState.LevelCompleted);
        }
    }

    public void StartEvent() => gameState = VillageEventState.Start;
}



public enum VillageEventState {Disabled, Start, SpawningWave, WaveInProgress, WaitingToSpawn, LevelCompleted };
/// <EstadosDelJuego>
/// Start: inicio del juego.
/// Spawning: Durante la creacion de la wave.
/// InProgress: Una vez que la wave ya fue completamente creada -> Cuando se mueren todos los enemigos de la wave, el estado pasa a Waiting.
/// </summary>

