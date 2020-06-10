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
    
    protected override void OnBeginSkill()
    {
        if (mychar == null) mychar = Main.instance.GetChar();
        AudioManager.instance.GetSoundPool("slowmo_enter", AudioGroups.MISC, slowmoEnter);
        AudioManager.instance.GetSoundPool("slowmo_exit", AudioGroups.MISC, slowmoExit);
    }
    protected override void OnStartUse()
    {
        mychar.ActivateBuffState(damageBuff, damageResistance, speedAcceleration, timeScale, timeDuration);
        AudioManager.instance.PlaySound("slowmo_enter");
        berserkParticle.transform.position = Main.instance.GetChar().transform.position;
        berserkParticle.Play();
    }
    protected override void OnStopUse()
    {
        mychar.DesactivateBuffState(damageBuff);
        AudioManager.instance.PlaySound("slowmo_exit", OnEndSound);
    }
    public void OnEndSound() => mychar.DesactivateBuffState(damageBuff);

    #region Desuso
    protected override void OnUpdateUse() { }
    protected override void OnStart() { }
    protected override void OnOneShotExecute() { }
    protected override void OnUpdateSkill() { }
    protected override void OnEndSkill() { }
    #endregion
}
