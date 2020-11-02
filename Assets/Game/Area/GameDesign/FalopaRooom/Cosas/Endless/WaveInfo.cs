using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "WaveInfo")]
public class WaveInfo : ScriptableObject
{
    public int waveNumber;

    public EnemyInfo[] enemiesPerWave;
}

[System.Serializable]
public struct EnemyInfo
{
    public EnemyBase enemy;
    public int enemyAmmount;
}
