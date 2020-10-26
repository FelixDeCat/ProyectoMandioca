using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillActive_Buff : SkillActivas
{
    //[SerializeField] float damageBuff = 5;
    //[SerializeField] float  damageResistance = 5;
    //[SerializeField] float speedAcceleration = 3;
    //[SerializeField] float timeScale = 0.5f;
    //[SerializeField] float timeDuration = 5f;
    
    CharacterHead mychar;
    [SerializeField] AudioClip slowmoEnter = null;
    [SerializeField] AudioClip slowmoExit = null;
    [SerializeField] ParticleSystem berserkParticle = null;
    [SerializeField] ParticleSystem brightParticle = null;
    [SerializeField] float smallLightVerticalOffset = 1;
    [SerializeField] ParticleSystem smallLightParticle = null;
    [SerializeField] GameObject berserkWings = null;

    [SerializeField] private AudioClip pickUp_skill = null;
    private const string _pickupSkill = "pickUp_skill";
    private const string _slowMoEnter = "slowmo_enter";
    private const string _slowMoExit = "slowmo_exit";

    Animator animWings;

    protected override void OnBeginSkill()
    {
        if (mychar == null) mychar = Main.instance.GetChar();
        AudioManager.instance.GetSoundPool(_slowMoEnter, AudioGroups.MISC, slowmoEnter);
        AudioManager.instance.GetSoundPool(_slowMoExit, AudioGroups.MISC, slowmoExit);
        AudioManager.instance.GetSoundPool(_pickupSkill, AudioGroups.GAME_FX,pickUp_skill);
        
        AudioManager.instance.PlaySound(_pickupSkill);
    
        mychar = Main.instance.GetChar();
        animWings = berserkWings.GetComponent<Animator>();
    }
    protected override void OnStartUse()
    {
        //mychar.ActivateBuffState(damageBuff, damageResistance, speedAcceleration, timeScale, timeDuration);
        AudioManager.instance.PlaySound("slowmo_enter");
        berserkParticle.transform.position = mychar.transform.position;
        berserkParticle.Play();
        brightParticle.Play();
        smallLightParticle.Play();
        animWings.Play("openWings");
    }
    protected override void OnStopUse()
    {
        //mychar.DesactivateBuffState(damageBuff);
        //AudioManager.instance.PlaySound("slowmo_exit", OnEndSound);
        //brightParticle.Stop();
    }
    //public void OnEndSound() => mychar.DesactivateBuffState(damageBuff);

    protected override void OnUpdateUse()
    {
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
