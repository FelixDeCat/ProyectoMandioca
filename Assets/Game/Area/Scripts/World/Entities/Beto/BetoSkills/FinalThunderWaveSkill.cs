using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FinalThunderWaveSkill : BossSkills, ISpawner
{
    int currentEnemies;
    [SerializeField] string currentScene = "bossRoom";

    public Action SpawnOver;

    List<EnemyBase> currentSpawnedEnemies = new List<EnemyBase>();

    [SerializeField] LifePercent_EnemyBaseDictionary enemiesDictionary = new LifePercent_EnemyBaseDictionary();
    [SerializeField] TotemFeedback spawnModifies = new TotemFeedback();
    [SerializeField] SpawnerSpot spot = new SpawnerSpot();

    [SerializeField] GenericLifeSystem lifeSystem = null;

    [SerializeField] Animator anim = null;
    [SerializeField] AnimEvent animEvent = null;

    [SerializeField] AudioClip spawnLoop = null;
    [SerializeField] AudioClip spawnOver = null;
    SoundPool pool;
    AudioSource source;
    Transform target;

    public override void Initialize()
    {
        base.Initialize();
        pool = AudioManager.instance.GetSoundPool(spawnLoop.name, AudioManager.SoundDimesion.ThreeD, AudioGroups.GAME_FX, spawnLoop, true);
        AudioManager.instance.GetSoundPool(spawnOver.name, AudioManager.SoundDimesion.ThreeD, AudioGroups.GAME_FX, spawnOver);
        spawnModifies.Initialize(StartCoroutine);
        animEvent.Add_Callback("Spawn", Callback);
        target = Main.instance.GetChar().Root;
    }
    void Callback() => spawnModifies.StartChargeFeedback(SpawnEnemies);

    protected override void OnInterruptSkill()
    {
        spawnModifies.InterruptCharge();
        int enemies = currentSpawnedEnemies.Count;
        for (int i = 0; i < enemies; i++)
        {
            currentSpawnedEnemies[currentEnemies - 1].ReturnToSpawner();
        }
        if (source != null)
        {
            source.Stop();
            pool.ReturnToPool(source);
            source = null;
        }
    }

    void SpawnEnemies()
    {
        spot.spawnSpot.position = target.position;
        LifePercent enemies = ReturnArrays((float)lifeSystem.Life / (float)lifeSystem.LifeMax);

        for (int i = 0; i < enemiesDictionary[enemies].Length; i++)
        {
            Vector3 pos = spot.GetSurfacePos();
            pos.y += 1;
            var enemy = enemiesDictionary[enemies][i];
            spawnModifies.StartGoToFeedback(pos, (x) => SpawnPrefab(enemy, x, currentScene));
        }

        if (source != null)
        {
            source.Stop();
            pool.ReturnToPool(source);
            source = null;
        }
        AudioManager.instance.PlaySound(spawnOver.name, transform);
        SpawnOver?.Invoke();
    }

    LifePercent ReturnArrays(float percent)
    {
        foreach (var item in enemiesDictionary)
        {
            if (percent >= item.Key.minPercent && percent <= item.Key.maxPercent)
                return item.Key;
        }

        Debug.Log("hay un error acá");
        return null;
    }

    protected override void OnOverSkill()
    {
        anim.SetBool("Spawn", false);
    }

    protected override void OnUseSkill()
    {
        anim.Play("StartSpawn");
        anim.SetBool("Spawn", true);
        source = pool.Get();
        if (source != null)
        {
            source.transform.position = transform.position;
            source.Play();
        }
    }

    public void SpawnPrefab(EnemyBase enemy, Vector3 pos, string sceneName = null)
    {
        currentSpawnedEnemies.Add(spot.SpawnPrefab(pos, enemy, sceneName, this).GetComponent<EnemyBase>());
        currentEnemies += 1;
    }

    public void ReturnObject(PlayObject newPrefab)
    {
        currentEnemies -= 1;
        newPrefab.Spawner = null;
        newPrefab.Off();
        currentSpawnedEnemies.Remove(newPrefab.GetComponent<EnemyBase>());
        PoolManager.instance.ReturnObject(newPrefab);
        if (currentEnemies <= 0) OverSkill();
    }

    public override void Pause()
    {
        base.Pause();
        spawnModifies.pause = true;
    }

    public override void Resume()
    {
        base.Resume();
        spawnModifies.pause = false;
    }

    public void SpawnPrefab(Vector3 pos, string sceneName = null)
    {
    }
}
