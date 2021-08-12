using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TotemSpawner : Totem
{
    [SerializeField] CustomSpawner spawner = null;
    [SerializeField] protected AudioClip ac_Summon = null;

    protected override void OnInitialize()
    {
        base.OnInitialize();
        AudioManager.instance.GetSoundPool(ac_Summon.name, AudioManager.SoundDimesion.ThreeD, AudioGroups.GAME_FX, ac_Summon);
    }

    public CustomSpawner GetSpawner => spawner;

    protected override void InternalEndCast()
    {
        for (int i = 0; i < spawner.waveAmount; i++)
        {
            Vector3 pos = spawner.spot.GetSurfacePos();
            if (pos == Vector3.zero) pos = transform.position;
            feedback.StartGoToFeedback(pos, (x) => spawner.SpawnPrefab(pos, CurrentScene));
        }
        AudioManager.instance.PlaySound(ac_Summon.name, transform);
    }

    protected override bool InternalCondition()
    {
        if (spawner.ReachMaxSpawn())
        {
            StartCoroutine(CheckMaxSpawn());
            return false;
        }
        else return true;
    }

    protected override void InternalTotemExit()
    {
        base.InternalTotemExit();
        StopAllCoroutines();
    }

    IEnumerator CheckMaxSpawn()
    {
        yield return new WaitForSeconds(1);

        OnStartCast();
    }

    protected override void InternalTakeDamage()
    {
        TakeDamage();
    }
}
