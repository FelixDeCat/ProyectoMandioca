using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance { get; private set; }

    Dictionary<string, List<EnemiesSaveStates<EnemyBase>>> scenesStates = new Dictionary<string, List<EnemiesSaveStates<EnemyBase>>>();

    Dictionary<string, List<EnemyBase>> enemiesPerScenes = new Dictionary<string, List<EnemyBase>>();

    private void Awake() => Instance = this;

    public void OnLoadEnemies(string sceneName)
    {
        enemiesPerScenes.Add(sceneName, new List<EnemyBase>());

        if (!scenesStates.ContainsKey(sceneName))
        {

        }
        else
        {
            for (int i = 0; i < scenesStates[sceneName].Count; i++)
            {
                var aux = SpawnEnemy(scenesStates[sceneName][i].poolName, sceneName);
                scenesStates[sceneName][i].LoadState(aux);
            }

            scenesStates[sceneName].Clear();
            scenesStates.Remove(sceneName);
        }
    }

    public void OnSaveStateEnemies(string sceneName)
    {
        if (!scenesStates.ContainsKey(sceneName)) scenesStates.Add(sceneName, new List<EnemiesSaveStates<EnemyBase>>());
        else scenesStates[sceneName].Clear();

        for (int i = 0; i < enemiesPerScenes[sceneName].Count; i++)
        {
            var aux = EnemySaveConverterAux.CreateEnemyState(enemiesPerScenes[sceneName][i]);
            aux.SaveState(enemiesPerScenes[sceneName][i]);
            scenesStates[sceneName].Add(aux);
            PoolManager.instance.ReturnObject(enemiesPerScenes[sceneName][i]);
        }

        enemiesPerScenes.Remove(sceneName);
    }

    public void ChangeEnemyScene(string sceneName, EnemyBase enemy)
    {
        if(sceneName != enemy.CurrentScene)
        {
            enemiesPerScenes[enemy.CurrentScene].Remove(enemy);
            enemiesPerScenes[sceneName].Add(enemy);
            enemy.CurrentScene = sceneName;
        }
    }

    public void DeleteEnemy(EnemyBase enemy)
    {
        if(enemiesPerScenes.ContainsKey(enemy.CurrentScene)) enemiesPerScenes[enemy.CurrentScene].Remove(enemy);
    }

    public void OnResetState(string sceneName)
    {
        if (scenesStates.ContainsKey(sceneName))
        {
            scenesStates[sceneName].Clear();
            scenesStates.Remove(sceneName);
        }
    }

    public void ResetAllStates()
    {
        scenesStates.Clear();
        enemiesPerScenes.Clear();
    }

    public EnemyBase SpawnEnemy(string poolName, string sceneToSpawn, EnemyBase prefab = null)
    {
        var aux = PoolManager.instance.GetObjectPool(poolName, prefab).GetPlayObject().GetComponent<EnemyBase>();
        aux.CurrentScene = sceneToSpawn;
        enemiesPerScenes[sceneToSpawn].Add(aux);
        return aux;
    }
}

public static class EnemySaveConverterAux
{
    public static EnemiesSaveStates<T> CreateEnemyState<T>(T enemy) where T : EnemyBase
    {
        var state = new EnemiesSaveStates<T>();

        if (enemy.GetComponent<MandragoraEnemy>()) state = new MandragoraSaveState<T>();

        return state;
    }
}
