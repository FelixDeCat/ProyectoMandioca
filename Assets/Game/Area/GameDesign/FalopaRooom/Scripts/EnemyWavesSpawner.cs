using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWavesSpawner : MonoBehaviour
{
    public WaveInfo[] allWavesInfo;
    SaveVillageManager villageManager = null;
        
    public float timeBetweenEnemies = 1f;
    CustomSpawner customSpawner;

    void Start()
    {
        villageManager = Main.instance.GetVillageManager();
        customSpawner = GetComponent<CustomSpawner>();
        StartCoroutine(checkEnemy());
    }
    public void Update()
    {
        if (Main.instance.GetVillageManager().gameState == VillageEventState.Start || Main.instance.GetVillageManager().gameState == VillageEventState.WaitingToSpawn)
        {
            StartCoroutine(SpawnWave(allWavesInfo[Main.instance.GetVillageManager().currentPhase]));
        }
    }

    public IEnumerator checkEnemy()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);

            if (Main.instance.GetVillageManager().gameState == VillageEventState.WaveInProgress && Main.instance.GetVillageManager().GetCurrentEnemiesCount() <= Main.instance.GetVillageManager().minEnemiesToSpawnNextWave)
            {
                Main.instance.GetVillageManager().SetCurrentState(VillageEventState.WaitingToSpawn);
           
                yield return new WaitForSeconds(2f);              
            }
        }
    }

    IEnumerator SpawnWave(WaveInfo info)
    {
        Main.instance.GetVillageManager().SetCurrentState(VillageEventState.SpawningWave);
        for (int i = 0; i < info.enemiesPerWave.Length; i++)
        {
            for (int j = 0; j < info.enemiesPerWave[i].enemyAmmount; j++)
            {
                SpawnEnemy(info.enemiesPerWave[i].enemy);
                yield return new WaitForSeconds(timeBetweenEnemies);
            }
        }
        Main.instance.GetVillageManager().SetCurrentState(VillageEventState.WaveInProgress);
    }


    EnemyBase SpawnEnemy(EnemyBase enemyToSpawn)
    {
        //enemyToSpawn.lifesystem.AddEventOnDeath(() => Debug.Log("IM DEAD"));
        //customSpawner.ChangePool(enemyToSpawn);
        //customSpawner.SpawnPrefab(transform.position);
        EnemyBase enemy = EnemyManager.Instance.SpawnEnemy(enemyToSpawn.name, UnityEngine.SceneManagement.SceneManager.GetActiveScene().name, enemyToSpawn);
        enemy.transform.position = transform.position;
        //EnemyBase enemy = Instantiate(enemyToSpawn, transform.position, transform.rotation);
        
        enemy.Initialize();
        enemy.SpawnEnemy();
        Main.instance.GetVillageManager().AddEnemy(enemy);
        enemy.lifesystem.AddEventOnDeath(() => Main.instance.GetVillageManager().RemoveEnemy(enemy));
        enemy.lifesystem.AddEventOnDeath(() => enemy.ResetEntity());
        return enemy;
    }
}
