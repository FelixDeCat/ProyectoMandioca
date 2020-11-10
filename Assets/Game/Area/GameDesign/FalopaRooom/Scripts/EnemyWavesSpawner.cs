using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWavesSpawner : MonoBehaviour
{
    public WaveInfo[] allWavesInfo;
    SaveVillageManager village = null;
        
    public float timeBetweenEnemies = 1f;
    CustomSpawner customSpawner;

    public void InitializeEnemyWavesSpawner()
    {
        village = Main.instance.GetVillageManager();
        customSpawner = GetComponent<CustomSpawner>();
        StartCoroutine(checkEnemy());
    }
    public void Update()
    {
        if (village == null) return;

        if (village.gameState == VillageEventState.Start || village.gameState == VillageEventState.WaitingToSpawn)
        {
            StartCoroutine(SpawnWave(allWavesInfo[village.currentPhase]));
        }
    }

    public IEnumerator checkEnemy()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);

            if (village.gameState == VillageEventState.WaveInProgress && village.GetCurrentEnemiesCount() <= village.minEnemiesToSpawnNextWave)
            {
                village.SetCurrentState(VillageEventState.WaitingToSpawn);
           
                yield return new WaitForSeconds(2f);              
            }
        }
    }

    IEnumerator SpawnWave(WaveInfo info)
    {
        village.SetCurrentState(VillageEventState.SpawningWave);
        for (int i = 0; i < info.enemiesPerWave.Length; i++)
        {
            for (int j = 0; j < info.enemiesPerWave[i].enemyAmmount; j++)
            {
                SpawnEnemy(info.enemiesPerWave[i].enemy);
                yield return new WaitForSeconds(timeBetweenEnemies);
            }
        }
        village.SetCurrentState(VillageEventState.WaveInProgress);
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
        enemy.lifesystem.Initialize(enemy.lifesystem.life, ()=> { }, () => { }, () => village.RemoveEnemy(enemy));
        enemy.SpawnEnemy();
        village.AddEnemy(enemy);
        enemy.lifesystem.AddEventOnDeath(() => enemy.ResetEntity());
        enemy.lifesystem.AddEventOnDeath(() => EnemyManager.Instance.DeleteEnemy(enemy));

        enemy.GetComponent<DamageReceiver>().AddTakeDamage((DamageData data) => enemy.GetComponent<CombatDirectorElement>().ChangeTarget(Main.instance.GetChar().transform));
        ChangeFocus(enemy);
        return enemy;
    }

    public void ChangeFocus(EnemyBase enemy)
    {
        WalkingEntity nearest = village.nearestNPC(transform.position);
        if (nearest != null)
        {
            enemy.GetComponent<CombatDirectorElement>().ChangeTarget(village.nearestNPC(enemy.transform.position).transform);
            enemy.GetComponent<CombatDirectorElement>().SetBool(true);
        }
    }

}
