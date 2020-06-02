using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillActive_SomeHeal : SkillActivas
{

    [SerializeField] float damageBuff = 5;
    [SerializeField] float  damageResistance = 5;
    [SerializeField] float speedAcceleration = 3;
    [SerializeField] float timeScale = 0.5f;
    [SerializeField] float timeDuration = 5f;

    [SerializeField] private ParticleSystem healFeedback = null;

    CharacterHead mychar;
    Animator anim;

    [SerializeField] AudioClip slowmoEnter;
    [SerializeField] AudioClip slowmoExit;


    protected override void OnBeginSkill()
    {
        if (mychar == null) mychar = Main.instance.GetChar();


        AudioManager.instance.GetSoundPool("slowmo_enter", AudioGroups.MISC, slowmoEnter);
        AudioManager.instance.GetSoundPool("slowmo_exit", AudioGroups.MISC, slowmoExit);
    }


    protected override void OnOneShotExecute()
    {
    }
    protected override void OnUpdateSkill()
    {
        if (healFeedback.isPlaying)
            healFeedback.transform.position = Main.instance.GetChar().transform.position;
    }
    
    protected override void OnEndSkill() { }
    protected override void OnStartUse()
    {
        mychar.ActivateBuffState(damageBuff, damageResistance, speedAcceleration, timeScale, timeDuration);
        AudioManager.instance.PlaySound("slowmo_enter");
    }
    protected override void OnStopUse()
    {
        
        AudioManager.instance.PlaySound("slowmo_exit", OnEndSound);
    }

    public void OnEndSound()
    {
        mychar.DesactivateBuffState(damageBuff);
    }

    protected override void OnUpdateUse() { }
}
