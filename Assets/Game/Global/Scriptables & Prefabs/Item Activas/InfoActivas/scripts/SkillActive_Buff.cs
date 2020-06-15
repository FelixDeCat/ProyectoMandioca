using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillActive_Buff : SkillActivas
{
    [SerializeField] float damageBuff = 5;
    [SerializeField] float  damageResistance = 5;
    [SerializeField] float speedAcceleration = 3;
    [SerializeField] float timeScale = 0.5f;
    [SerializeField] float timeDuration = 5f;
    
    CharacterHead mychar;
    [SerializeField] AudioClip slowmoEnter;
    [SerializeField] AudioClip slowmoExit;
    [SerializeField] ParticleSystem berserkParticle;
    [SerializeField] ParticleSystem brightParticle;
    [SerializeField] float smallLightVerticalOffset;
    [SerializeField] ParticleSystem smallLightParticle;
    [SerializeField] GameObject berserkWings;

    protected override void OnBeginSkill()
    {
        if (mychar == null) mychar = Main.instance.GetChar();
        AudioManager.instance.GetSoundPool("slowmo_enter", AudioGroups.MISC, slowmoEnter);
        AudioManager.instance.GetSoundPool("slowmo_exit", AudioGroups.MISC, slowmoExit);
        mychar = Main.instance.GetChar();

    }
    protected override void OnStartUse()
    {
        mychar.ActivateBuffState(damageBuff, damageResistance, speedAcceleration, timeScale, timeDuration);
        AudioManager.instance.PlaySound("slowmo_enter");
        berserkParticle.transform.position = mychar.transform.position;
        berserkParticle.Play();
        brightParticle.Play();
        smallLightParticle.Play();
    }
    protected override void OnStopUse()
    {
        mychar.DesactivateBuffState(damageBuff);
        AudioManager.instance.PlaySound("slowmo_exit", OnEndSound);
        brightParticle.Stop();
    }
    public void OnEndSound() => mychar.DesactivateBuffState(damageBuff);

    protected override void OnUpdateUse()
    {
        berserkWings.transform.position = mychar.transform.position;
        brightParticle.transform.position = mychar.transform.position + Vector3.up;
        smallLightParticle.transform.position = mychar.transform.position + Vector3.up * smallLightVerticalOffset;
    }

    #region Desuso
    protected override void OnStart() { }
    protected override void OnOneShotExecute() { }
    protected override void OnUpdateSkill() { }
    protected override void OnEndSkill() { }
    #endregion
}
