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
        if ( villageManager.gameState == VillageEventState.Start || villageManager.gameState == VillageEventState.WaitingToSpawn)
        {
            StartCoroutine(SpawnWave(allWavesInfo[villageManager.currentPhase]));
        }
    }

    public IEnumerator checkEnemy()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);

            if (villageManager.gameState == VillageEventState.WaveInProgress && Main.instance.GetVillageManager().GetCurrentEnemiesCount() <= villageManager.minEnemiesToSpawnNextWave)
            {
                villageManager.SetCurrentState(VillageEventState.WaitingToSpawn);
           
                yield return new WaitForSeconds(2f);              
            }
        }
    }

    IEnumerator SpawnWave(WaveInfo info)
    {
        villageManager.SetCurrentState(VillageEventState.SpawningWave);
        for (int i = 0; i < info.enemiesPerWave.Length; i++)
        {
            for (int j = 0; j < info.enemiesPerWave[i].enemyAmmount; j++)
            {
                SpawnEnemy(info.enemiesPerWave[i].enemy);
                yield return new WaitForSeconds(timeBetweenEnemies);
            }
        }
        villageManager.SetCurrentState(VillageEventState.WaveInProgress);
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
        villageManager.AddEnemy(enemy);
        enemy.lifesystem.AddEventOnDeath(() => villageManager.RemoveEnemy(enemy));
        enemy.lifesystem.AddEventOnDeath(() => enemy.ResetEntity());
        return enemy;
    }
}
