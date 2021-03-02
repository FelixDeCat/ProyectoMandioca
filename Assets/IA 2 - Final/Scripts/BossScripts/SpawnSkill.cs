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
    [SerializeField] DamageReceiver dmgReceiver = null;
    public Action OnSpawn;
    List<PlayObject> currentSpawnedEnemies = new List<PlayObject>();

    public override void Initialize()
    {
        base.Initialize();
        totemFeedback.Initialize(StartCoroutine);
    }

    void Callback() => totemFeedback.StartChargeFeedback(SpawnEnemies);

    protected override void OnInterruptSkill()
    {
        totemFeedback.InterruptCharge();
        for (int i = 0; i < currentSpawnedEnemies.Count; i++)
        {
            if (currentSpawnedEnemies.Count == 0) break;
            Debug.Log(currentEnemies - 1);
            currentSpawnedEnemies[currentEnemies-1].ReturnToSpawner();
        }
    }

    void SpawnEnemies()
    {
        int ammountToSpawn = UnityEngine.Random.Range(minSpawn, maxSpawn);
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
        dmgReceiver.AddInvulnerability(Damagetype.All);
    }

    protected override void OnOverSkill()
    {
        shieldObject.SetActive(false);
        dmgReceiver.RemoveInvulnerability(Damagetype.All);
        animEvent.Remove_Callback("SpawnSkill", Callback);
    }

    protected override void OnUseSkill()
    {
        anim.SetBool("OnSpawn", true);
        anim.Play("SpawnSkill");
        animEvent.Add_Callback("SpawnSkill",Callback);
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
