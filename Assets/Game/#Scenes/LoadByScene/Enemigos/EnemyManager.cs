using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance { get; private set; }

    Dictionary<string, List<EnemiesSaveStates<EnemyBase>>> scenesStates = new Dictionary<string, List<EnemiesSaveStates<EnemyBase>>>();
    Dictionary<string, ProxyEnemyBase[]> proxysPerScene = new Dictionary<string, ProxyEnemyBase[]>();
    Dictionary<string, List<EnemyBase>> enemiesPerScenes = new Dictionary<string, List<EnemyBase>>();

    List<string> scenesClear = new List<string>();
    List<string> scenesToReset = new List<string>();

    bool respawned;

    private void Awake() { Instance = this; }

    public void OnLoadEnemies(string sceneName, ProxyEnemyBase[] enemiesToCharge)
    {
        if (enemiesPerScenes.ContainsKey(sceneName) || scenesClear.Contains(sceneName)) return;
        enemiesPerScenes.Add(sceneName, new List<EnemyBase>());

        if (!scenesStates.ContainsKey(sceneName))
        {
            for (int i = 0; i < enemiesToCharge.Length; i++)
            {
                var aux = SpawnEnemy<EnemyBase>(enemiesToCharge[i].myEnemy.name, sceneName, enemiesToCharge[i].myEnemy);
                enemiesToCharge[i].SpawnEnemy(aux);
            }
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
        }

        proxysPerScene.Add(sceneName, enemiesToCharge);
    }

    public void OnSaveStateEnemies(string sceneName)
    {
        if (!scenesStates.ContainsKey(sceneName)) scenesStates.Add(sceneName, new List<EnemiesSaveStates<EnemyBase>>());
        else return;
        if (!enemiesPerScenes.ContainsKey(sceneName)) enemiesPerScenes.Add(sceneName, new List<EnemyBase>());
        StartCoroutine(SaveStates(sceneName));
    }

    bool Contains(EnemyBase enemy)
    {
        if (enemy == null) { Debug.LogError("me llegó un enemy null"); return false; }
        if (enemiesPerScenes.ContainsKey(enemy.CurrentScene))
        {
            if (enemiesPerScenes[enemy.CurrentScene].Contains(enemy)) return true;
            else return false;
        }
        else return false;
    }

    public void ChangeEnemyScene(string sceneName, EnemyBase enemy)
    {
        if (enemy.CurrentScene == null || !Contains(enemy)) return;

        if (enemy.CurrentScene != null && sceneName != enemy.CurrentScene)
        {
            enemiesPerScenes[enemy.CurrentScene].Remove(enemy);
            if (!enemiesPerScenes.ContainsKey(sceneName)) enemiesPerScenes.Add(sceneName, new List<EnemyBase>());
            enemiesPerScenes[sceneName].Add(enemy);
            enemy.CurrentScene = sceneName;
        }
    }

    public void SceneReset(string sceneName)
    {
        if (!scenesToReset.Contains(sceneName) && !scenesClear.Contains(sceneName)) scenesToReset.Add(sceneName);
    }

    public void SceneClear(string sceneName)
    {
        if (!scenesClear.Contains(sceneName)) scenesClear.Add(sceneName);

        if (scenesToReset.Contains(sceneName)) scenesToReset.Remove(sceneName);
    }

    public void RespawnsEnemies(string currentScene)
    {
        if (respawned || scenesToReset.Count == 0) return;

        StartCoroutine(ResetScenesCoroutine(currentScene));
    }

    public void DeleteEnemy(EnemyBase enemy)
    {
        if (!string.IsNullOrEmpty(enemy.CurrentScene) && enemiesPerScenes.ContainsKey(enemy.CurrentScene)) enemiesPerScenes[enemy.CurrentScene].Remove(enemy);
    }

    public void OnResetState(string sceneName)
    {
        if (scenesStates.ContainsKey(sceneName))
        {
            scenesStates[sceneName].Clear();
            scenesStates.Remove(sceneName);
            if (scenesToReset.Contains(sceneName)) scenesToReset.Remove(sceneName);
        }
    }

    public IEnumerator ExecuteSceneRebuildEnemies(string currentScene)
    {
        if (respawned || scenesToReset.Count == 0) yield break;

        respawned = true;
        for (int i = 0; i < scenesToReset.Count; i++)
        {
            if (!proxysPerScene.ContainsKey(scenesToReset[i])) continue;
            Debug.Log(scenesToReset[i]);
            for (int y = 0; y < enemiesPerScenes[scenesToReset[i]].Count; y++)
            {
                Debug.Log(enemiesPerScenes[scenesToReset[i]][y].name);
                enemiesPerScenes[scenesToReset[i]][y].ReturnToSpawner();
                yield return null;
            }

            enemiesPerScenes[scenesToReset[i]].Clear();
            for (int y = 0; y < proxysPerScene[scenesToReset[i]].Length; y++)
            {
                try
                {
                    var aux = SpawnEnemy<EnemyBase>(proxysPerScene[scenesToReset[i]][y].myEnemy.name, scenesToReset[i]);
                    proxysPerScene[scenesToReset[i]][y].SpawnEnemy(aux);
                }
                catch (NullReferenceException) { Debug.LogWarning("!!!!!!En la escena: " + scenesToReset[i] + " hay un proxy descarrilado"); }

                yield return null;
            }
        }

        scenesToReset.Clear();
        respawned = false;
        scenesToReset.Add(currentScene);
    }

    public void ResetAllStates()
    {
        scenesStates.Clear();
        enemiesPerScenes.Clear();
    }

    public T SpawnEnemy<T>(string poolName, string sceneToSpawn, T prefab = null) where T : EnemyBase
    {
        var aux = PoolManager.instance.GetObjectPool(poolName, prefab).GetPlayObject().GetComponent<T>();
        aux.CurrentScene = sceneToSpawn;
        if (!string.IsNullOrEmpty(sceneToSpawn) && enemiesPerScenes.ContainsKey(sceneToSpawn)) enemiesPerScenes[sceneToSpawn].Add(aux);
        return aux;
    }

    IEnumerator SaveStates(string sceneName)
    {
        if (!enemiesPerScenes.ContainsKey(sceneName)) enemiesPerScenes.Add(sceneName, new List<EnemyBase>());


        for (int i = 0; i < enemiesPerScenes[sceneName].Count; i++)
        {
            if (!enemiesPerScenes[sceneName][i].death)
            {
                //var aux = EnemySaveConverterAux.CreateEnemyState(enemiesPerScenes[sceneName][i]);
                //aux.SaveState(enemiesPerScenes[sceneName][i]);
                //scenesStates[sceneName].Add(aux);
            }
            enemiesPerScenes[sceneName][i].ReturnToSpawner();
            yield return null;
        }

        enemiesPerScenes.Remove(sceneName);
        if (proxysPerScene.ContainsKey(sceneName)) proxysPerScene.Remove(sceneName);
        if (scenesToReset.Contains(sceneName)) scenesToReset.Remove(sceneName); 
    }

    IEnumerator ResetScenesCoroutine(string currentScene)
    {
        respawned = true;
        for (int i = 0; i < scenesToReset.Count; i++)
        {
            if (!proxysPerScene.ContainsKey(scenesToReset[i])) continue;

            for (int y = 0; y < enemiesPerScenes[scenesToReset[i]].Count; y++)
                enemiesPerScenes[scenesToReset[i]][y].ReturnToSpawner();

            enemiesPerScenes[scenesToReset[i]].Clear();
            for (int y = 0; y < proxysPerScene[scenesToReset[i]].Length; y++)
            {
                try
                {
                    var aux = SpawnEnemy<EnemyBase>(proxysPerScene[scenesToReset[i]][y].myEnemy.name, scenesToReset[i]);
                    proxysPerScene[scenesToReset[i]][y].SpawnEnemy(aux);
                }
                catch (NullReferenceException) { Debug.LogWarning("!!!!!!En la escena: " + scenesToReset[i] + " hay un proxy descarrilado"); }

                yield return null;
            }
        }

        scenesToReset.Clear();
        respawned = false;
        scenesToReset.Add(currentScene);
    }
}

public static class EnemySaveConverterAux
{
    public static EnemiesSaveStates<T> CreateEnemyState<T>(T enemy) where T : EnemyBase
    {
        var state = new EnemiesSaveStates<T>();

        if (enemy.GetComponent<MandragoraEnemy>()) state = new MandragoraSaveState<T>();
        else if (enemy.GetComponent<CarivorousPlant>()) state = new CarnPlantSaveState<T>();
        else if (enemy.GetComponent<TotemSpawner>()) state = new TotemSpawnerState<T>();
        else if (enemy.GetComponent<EnemyWithCombatDirector>()) state = new EnemiesRangeSaveState<T>();

        return state;
    }
}
