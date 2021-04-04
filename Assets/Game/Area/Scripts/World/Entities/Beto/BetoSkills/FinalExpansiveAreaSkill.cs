﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalExpansiveAreaSkill : BossSkills
{
    [SerializeField] Sensor explosionSensor = null;
    [SerializeField, Range(1, 100)] float maxScale = 20;
    [SerializeField] float timeToMaxScale = 2;

    [SerializeField] ParticleSystem expansiveOver = null;
    [SerializeField] LayerMask obstacleMask = 1 << 0;
    [SerializeField] DamageData dmgData = null;

    [SerializeField] int damage = 4;
    [SerializeField] float knockback = 100;
    [SerializeField] Damagetype dmgType = Damagetype.Explosion;
    [SerializeField] Animator anim = null;
    [SerializeField] AnimEvent animEvent = null;
    Vector3 finalScale = Vector3.zero;
    float timerScale = 0;

    public override void Initialize()
    {
        base.Initialize();
        explosionSensor.AddCallback_OnTriggerEnter(GiveDamage);
        ParticlesManager.Instance.GetParticlePool(expansiveOver.name, expansiveOver);
        animEvent.Add_Callback("Expansive", StartAbility);
    }

    protected override void OnUseSkill()
    {
        dmgData.SetDamage(damage).SetKnockback(knockback).SetDamageInfo(DamageInfo.NonBlockAndParry).SetDamageType(dmgType);
        anim.Play("StartExpansive");
    }

    void StartAbility()
    {
        explosionSensor.gameObject.SetActive(true);
        explosionSensor.transform.localScale = Vector3.one;
        finalScale = explosionSensor.transform.localScale * maxScale;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (!explosionSensor.gameObject.activeSelf) return;

        timerScale += Time.deltaTime;

        explosionSensor.transform.localScale = Vector3.Lerp(Vector3.one, finalScale, timerScale/timeToMaxScale);

        if (timerScale >= timeToMaxScale)
            OverSkill();
    }

    protected override void OnInterruptSkill()
    {
    }

    protected override void OnOverSkill()
    {
        timerScale = 0;
        explosionSensor.transform.localScale = Vector3.one;
        explosionSensor.gameObject.SetActive(false);
        ParticlesManager.Instance.PlayParticle(expansiveOver.name, explosionSensor.transform.position);
    }

    void GiveDamage(GameObject collision)
    {
        var dmgReceiver = collision.GetComponent<DamageReceiver>();
        if (!dmgReceiver) return;

        var vectorBetween = dmgReceiver.transform.position - explosionSensor.transform.position;

        if (Physics.Raycast(explosionSensor.transform.position, vectorBetween.normalized, vectorBetween.magnitude, obstacleMask, QueryTriggerInteraction.Ignore))
            return;

        dmgReceiver.TakeDamage(dmgData.SetPositionAndDirection(explosionSensor.transform.position, vectorBetween.normalized));
    }
}
