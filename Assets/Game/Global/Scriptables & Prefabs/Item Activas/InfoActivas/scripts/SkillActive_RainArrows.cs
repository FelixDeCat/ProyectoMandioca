using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ToolsMandioca.Extensions;

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

    [SerializeField] private AudioClip _arrowSound;
    private const string _arrowsSoundName = "arrows";

    private CharacterHead _hero;

    [SerializeField] Atenea atenea;

    protected override void OnStart()
    {
        atenea.GetComponent<AnimEvent>().Add_Callback("TiraFlecha", ThrowArrows);
    }

    protected override void OnBeginSkill()
    {
        _hero = Main.instance.GetChar();
        AudioManager.instance.GetSoundPool(_arrowsSoundName, _arrowSound, true);
    }

    void ThrowArrows()
    {
        AudioManager.instance.PlaySound(_arrowsSoundName);
        dmgPerTick = dmgTotal / (duration / ticksAmount);
        SetPositionOfRainEffect();
        SetRainFeedBackParticlesPosition();
        arrowRain_ps.Play();
        FindAndDamage();
    }
    protected override void OnStartUse() 
    {
        atenea.gameObject.SetActive(true);
        atenea.GoToHero();
        atenea.Anim_Arrows();
    }
    protected override void OnStopUse()
    {
        arrowRain_ps.Stop();
        AudioManager.instance.StopAllSounds(_arrowsSoundName);
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
