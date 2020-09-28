using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance { get; private set; }

    Dictionary<string, List<EnemiesSaveStates<EnemyBase>>> scenesStates = new Dictionary<string, List<EnemiesSaveStates<EnemyBase>>>();

    Dictionary<string, List<EnemyBase>> enemiesPerScenes = new Dictionary<string, List<EnemyBase>>();

    private void Awake() { Instance = this; }

    public void OnLoadEnemies(string sceneName, ProxyEnemyBase[] enemiesToCharge)
    {
        enemiesPerScenes.Add(sceneName, new List<EnemyBase>());

        if (!scenesStates.ContainsKey(sceneName))
        {
            for (int i = 0; i < enemiesToCharge.Length; i++)
            {
                var aux = SpawnEnemy<EnemyBase>(enemiesToCharge[i].myEnemy.name, sceneName, enemiesToCharge[i].myEnemy);
                enemiesToCharge[i].SpawnEnemy(aux);
                Destroy(enemiesToCharge[i].gameObject);
            }

            enemiesToCharge = new ProxyEnemyBase[0];

            Debug.Log("Cargo Nuevos");
        }
        else
        {
            for (int i = 0; i < scenesStates[sceneName].Count; i++)
            {
                var aux = SpawnEnemy<EnemyBase>(scenesStates[sceneName][i].poolName, sceneName);
                scenesStates[sceneName][i].LoadState(aux);
            }

            scenesStates[sceneName].Clear();
            scenesStates.Remove(sceneName);

            Debug.Log("Había estado guardado");
        }
    }

    public void OnSaveStateEnemies(string sceneName)
    {
        if (!scenesStates.ContainsKey(sceneName)) scenesStates.Add(sceneName, new List<EnemiesSaveStates<EnemyBase>>());
        else scenesStates[sceneName].Clear();

        if (!enemiesPerScenes.ContainsKey(sceneName)) enemiesPerScenes.Add(sceneName, new List<EnemyBase>());

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

    public T SpawnEnemy<T>(string poolName, string sceneToSpawn, T prefab = null) where T : EnemyBase
    {
        var aux = PoolManager.instance.GetObjectPool(poolName, prefab).GetPlayObject().GetComponent<T>();
        aux.On();
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
        else if (enemy.GetComponent<CarivorousPlant>()) state = new CarnPlantSaveState<T>();
        else if (enemy.GetComponent<EnemyWithCombatDirector>()) state = new EnemiesRangeSaveState<T>();

        return state;
    }
}
