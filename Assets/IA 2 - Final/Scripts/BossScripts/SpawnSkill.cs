using System;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSkill : BossSkills, ISpawner
{
    [SerializeField] SpawnerSpot spot = new SpawnerSpot();
    [SerializeField] int minSpawn = 2;
    [SerializeField] int maxSpawn = 5;
    [SerializeField] TotemFeedback totemFeedback = new TotemFeedback();
    [SerializeField] EnemyBase entAcorazed = null;
    int currentEnemies;
    [SerializeField] string currentScene = "bossRoom";
    [SerializeField] Animator shieldObject = null;
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
        int enemies = currentSpawnedEnemies.Count;
        for (int i = 0; i < enemies; i++)
        {
            currentSpawnedEnemies[currentEnemies-1].ReturnToSpawner();
        }
    }

    void SpawnEnemies()
    {
        spot.spawnSpot.position = Main.instance.GetChar().transform.position + Vector3.up;
        int ammountToSpawn = UnityEngine.Random.Range(minSpawn, maxSpawn + 1);
        for (int i = 0; i < ammountToSpawn; i++)
        {
            Vector3 pos = spot.GetSurfacePos();
            pos.y += 1;
            totemFeedback.StartGoToFeedback(pos, (x) => SpawnPrefab(x, currentScene));
        }
        shieldObject.gameObject.SetActive(true);
        shieldObject.Play("ShieldUp");
        anim.SetBool("OnSpawn", false);
        OnSpawn?.Invoke();
        dmgReceiver.AddInvulnerability(Damagetype.All);
    }

    protected override void OnOverSkill()
    {
        shieldObject.Play("Shield Down");
        shieldObject.GetBehaviour<ANIM_SCRIPT_Base>().ConfigureCallback(() => shieldObject.gameObject.SetActive(false));
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
        if (currentEnemies <= 0) OverSkill();
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
