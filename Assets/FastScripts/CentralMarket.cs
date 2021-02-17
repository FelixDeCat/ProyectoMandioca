using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CentralMarket : MonoBehaviour
{
    [SerializeField] SaveVillageManager saveVillageManger = null;
    public EnemyDieEvent[] Catchers;
    public EnemyWavesSpawner[] enemySpawners;

    public void TRIGGER_BeginCentralMarketEvent()
    {
        saveVillageManger.InitializeVillage();
        for (int i = 0; i < Catchers.Length; i++)
        {
            Catchers[i].Initialize();
        }
        for (int i = 0; i < enemySpawners.Length; i++)
        {
            enemySpawners[i].InitializeEnemyWavesSpawner();
        }
    }
}
