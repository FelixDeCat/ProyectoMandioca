using System;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSkill : BossSkills, ISpawner
{
    [SerializeField] SpawnerSpot spot = new SpawnerSpot();
    [SerializeField] int minSpawn = 2;
    [SerializeField] int maxSpawn = 5;
    [SerializeField] TotemFeedback totemFeedback = new TotemFeedback();
    [SerializeField] EnemyBase entAcorazed;
    int currentEnemies;
    [SerializeField] string currentScene = "bossRoom";
    [SerializeField] GameObject shieldObject = null;
    [SerializeField] Animator anim = null;
    [SerializeField] AnimEvent animEvent = null;
    public Action OnSpawn;
    List<PlayObject> currentSpawnedEnemies = new List<PlayObject>();

    public override void Initialize()
    {
        base.Initialize();
        animEvent.Add_Callback("SpawnSkill", () => totemFeedback.StartChargeFeedback(SpawnEnemies));
        totemFeedback.Initialize(StartCoroutine);
    }

    protected override void OnInterruptSkill()
    {
        totemFeedback.InterruptCharge();
        for (int i = 0; i < 1; i++)
        {
            if (currentSpawnedEnemies.Count == 0) break;
            currentSpawnedEnemies[i].ReturnToSpawner();
            i -= 1;
        }
    }

    void SpawnEnemies()
    {
        int ammountToSpawn = UnityEngine.Random.Range(minSpawn, maxSpawn + 1);
        spot.spawnSpot.position = Main.instance.GetChar().transform.position;

        for (int i = 0; i < ammountToSpawn; i++)
        {
            Vector3 pos = spot.GetSurfacePos();
            pos.y += 1;
            totemFeedback.StartGoToFeedback(pos, (x) => SpawnPrefab(x, currentScene));
        }
        shieldObject.SetActive(true);
        anim.SetBool("OnSpawn", false);
        OnSpawn?.Invoke();
    }

    protected override void OnOverSkill()
    {
        shieldObject.SetActive(false);
    }

    protected override void OnUseSkill()
    {
        anim.SetBool("OnSpawn", true);
        anim.Play("SpawnSkill");
    }

    public void SpawnPrefab(Vector3 pos, string sceneName = null)
    {
        currentSpawnedEnemies.Add(spot.SpawnPrefab(pos, entAcorazed, sceneName, this));
        currentEnemies += 1;
    }

    public void ReturnObject(PlayObject newPrefab)
    {
        currentEnemies -= 1;
        newPrefab.Spawner = null;
        newPrefab.Off();
        currentSpawnedEnemies.Remove(newPrefab);
        PoolManager.instance.ReturnObject(newPrefab);
        if (currentEnemies == 0) OverSkill();
    }

    public override void Pause()
    {
        base.Pause();
        totemFeedback.pause = true;
    }

    public override void Resume()
    {
        base.Resume();
        totemFeedback.pause = false;
    }
}
