using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalLaserSkill : BossSkills
{
    [SerializeField] ThrowData throwData = null;
    [SerializeField] Transform shootPos = null;

    [SerializeField] Throwable projectile = null;
    Transform target;

    [SerializeField] Animator anim = null;
    [SerializeField] AnimEvent animEvent = null;
    [SerializeField] AudioClip shootSound = null;

    public override void Initialize()
    {
        base.Initialize();
        ThrowablePoolsManager.instance.CreateAPool(projectile.name, projectile);
        SetTarget(Main.instance.GetChar().transform);
        animEvent.Add_Callback("Shoot", ShootEvent);
        AudioManager.instance.GetSoundPool(shootSound.name, AudioManager.SoundDimesion.ThreeD, AudioGroups.GAME_FX, shootSound);
    }

    public BossSkills SetTarget(Transform _target)
    {
        target = _target;
        return this;
    }

    protected override void OnUseSkill()
    {
        anim.Play("Shoot");
    }

    protected override void OnInterruptSkill()
    {
    }

    protected override void OnOverSkill()
    {
    }

    void ShootEvent()
    {
        AudioManager.instance.PlaySound(shootSound.name, shootPos);
        throwData.Position = shootPos.position;
        throwData.Direction = (target.position + Vector3.up - shootPos.position).normalized;
        ThrowablePoolsManager.instance.Throw(projectile.name, throwData);
        Itteration(1, 2, delegate { }, OverSkill);
    }
}
