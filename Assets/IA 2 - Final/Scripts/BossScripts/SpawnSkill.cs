﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpawnSkill : BossSkills, ISpawner
{
    [SerializeField] SpawnerSpot spot = new SpawnerSpot();
    [SerializeField] int minSpawn = 2;
    [SerializeField] int maxSpawn = 5;
    [SerializeField] TotemFeedback totemFeedback = new TotemFeedback();
    [SerializeField] EnemyBase entAcorazed = null;
    int currentEnemies = 0;
    [SerializeField] string currentScene = "bossRoom";
    [SerializeField] Animator shieldObject = null;
    [SerializeField] Animator anim = null;
    [SerializeField] AnimEvent animEvent = null;
    [SerializeField] DamageReceiver dmgReceiver = null;

    [SerializeField] AudioClip shieldUpSound = null;
    [SerializeField] AudioClip shieldDownSound = null;
    [SerializeField] AudioClip spawnLoop = null;
    [SerializeField] AudioClip spawnOver = null;
    SoundPool pool;
    public Action OnSpawn;
    List<EnemyBase> currentSpawnedEnemies = new List<EnemyBase>();

    public override void Initialize()
    {
        base.Initialize();
        AudioManager.instance.GetSoundPool(shieldUpSound.name, AudioManager.SoundDimesion.ThreeD, AudioGroups.GAME_FX, shieldUpSound);
        AudioManager.instance.GetSoundPool(shieldDownSound.name, AudioManager.SoundDimesion.ThreeD, AudioGroups.GAME_FX, shieldDownSound);
        pool = AudioManager.instance.GetSoundPool(spawnLoop.name, AudioManager.SoundDimesion.ThreeD, AudioGroups.GAME_FX, spawnLoop, true);
        AudioManager.instance.GetSoundPool(spawnOver.name, AudioManager.SoundDimesion.ThreeD, AudioGroups.GAME_FX, spawnOver);
        totemFeedback.Initialize(StartCoroutine);
    }

    void Callback() => totemFeedback.StartChargeFeedback(SpawnEnemies);

    protected override void OnInterruptSkill()
    {
        totemFeedback.InterruptCharge();
        int enemies = currentSpawnedEnemies.Count;
        for (int i = 0; i < enemies; i++)
        {
            currentSpawnedEnemies[currentEnemies-1].GetComponent<DamageReceiver>().InstaKill();
        }
    }

    void SpawnEnemies()
    {
        int ammountToSpawn = UnityEngine.Random.Range(minSpawn, maxSpawn + 1);
        for (int i = 0; i < ammountToSpawn; i++)
        {
            Vector3 pos = spot.GetSurfacePos();
            if (pos == Vector3.zero) pos = transform.position;
            pos.y += 1;
            totemFeedback.StartGoToFeedback(pos, (x) => SpawnPrefab(x, currentScene));
        }
        shieldObject.Play("ShieldUp");
        AudioManager.instance.PlaySound(shieldUpSound.name, shieldObject.transform);
        if (source != null)
        {
            source.Stop();
            pool.ReturnToPool(source);
            source = null;
        }
        AudioManager.instance.PlaySound(spawnOver.name, shieldObject.transform);
        anim.SetBool("OnSpawn", false);
        OnSpawn?.Invoke();
        dmgReceiver.AddInvulnerability(Damagetype.All);
    }
    AudioSource source;

    protected override void OnOverSkill()
    {
        if (source != null)
        {
            source.Stop();
            pool.ReturnToPool(source);
            source = null;
        }
        if (shieldObject.GetCurrentAnimatorStateInfo(0).IsName("ShieldUp")) { shieldObject.Play("Shield Down"); AudioManager.instance.PlaySound(shieldDownSound.name, shieldObject.transform); }
        dmgReceiver.RemoveInvulnerability(Damagetype.All);
        animEvent.Remove_Callback("SpawnSkill", Callback);
    }

    protected override void OnUseSkill()
    {
        source = pool.Get();
        if (source != null)
        {
            source.transform.position = transform.position;
            source.Play();
        }
        anim.SetBool("OnSpawn", true);
        anim.Play("SpawnSkill");
        animEvent.Add_Callback("SpawnSkill",Callback);
    }

    public void SpawnPrefab(Vector3 pos, string sceneName = null)
    {
        EnemyBase enemy = spot.SpawnPrefab(pos, entAcorazed, sceneName, this).GetComponent<EnemyBase>();
        currentSpawnedEnemies.Add(enemy);

        UnityAction ReturnObjectCallback = delegate { };

        ReturnObjectCallback = () =>
        {
            currentSpawnedEnemies.Remove(enemy);
            currentEnemies -= 1;
            enemy.OnDeath.RemoveListener(ReturnObjectCallback);
            if (currentEnemies <= 0) OverSkill();
        };
        enemy.OnDeath.AddListener(ReturnObjectCallback);

        currentEnemies += 1;
    }

    public void ReturnObject(PlayObject newPrefab)
    {
        newPrefab.Spawner = null;
        newPrefab.Off();
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
