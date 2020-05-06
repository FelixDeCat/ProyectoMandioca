using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools.Extensions;

public class SkillActive_RainArrows : SkillActivas
{
    [Header("Rain arrows settings")]
    [SerializeField] private float duration = 6;
    [SerializeField] private float dmgTotal = 5;
    [SerializeField] private int ticksAmount = 6;
    private float dmgPerTick;
    [SerializeField] private float range = 14;
    private float tickCount;

    [SerializeField] private ParticleSystem arrowRain_ps = null;
    private Vector3 anchorPos;

    private CharacterHead _hero;

    protected override void OnBeginSkill()
    {
        _hero = Main.instance.GetChar();
    }
    protected override void OnStartUse() 
    {
        dmgPerTick = dmgTotal / (duration / ticksAmount);
        SetPositionOfRainEffect();
        SetRainFeedBackParticlesPosition();
        arrowRain_ps.Play();
        FindAndDamage();
    }
    protected override void OnStopUse()
    {
        arrowRain_ps.Stop();
    }

    protected override void OnUpdateUse() 
    {
        tickCount += Time.deltaTime;
        if (tickCount < ticksAmount)
            return;
        FindAndDamage();
        tickCount = 0;
    }

    private void SetRainFeedBackParticlesPosition()
    {
        arrowRain_ps.transform.localPosition = anchorPos + Vector3.up * 3;
    }
    private void SetPositionOfRainEffect()
    {
        anchorPos = _hero.transform.position;
    }
    private void FindAndDamage()
    {
        List<EnemyBase> enemigosAfectados = Extensions.FindInRadius<EnemyBase>(_hero.transform, range);
        DoDamageTo(enemigosAfectados);
    }
    private void DoDamageTo<T>(List<T> enemiesAffected) where T : EnemyBase
    {
        foreach (T en in enemiesAffected) 
            en.TakeDamage(Mathf.RoundToInt(dmgPerTick), Vector3.up, Damagetype.normal, _hero);
    }

    #region PARA SKILLS QUE EJECUTAN LA HABILIDAD EN UN SOLO FRAME
    protected override void OnOneShotExecute() { }
    protected override void OnEndSkill() { }
    protected override void OnUpdateSkill() { }
    #endregion

}
