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

    [SerializeField] Atenea atenea;
    [SerializeField] float offset;

    [SerializeField] AudioClip slowmoEnter;
    [SerializeField] AudioClip slowmoExit;

    protected override void OnStart()
    {
        atenea.GetComponent<AnimEvent>().Add_Callback("Buff", Buff);
    }
    protected override void OnBeginSkill()
    {
        if (mychar == null) mychar = Main.instance.GetChar();


        AudioManager.instance.GetSoundPool("slowmo_enter", AudioGroups.MISC, slowmoEnter);
        AudioManager.instance.GetSoundPool("slowmo_exit", AudioGroups.MISC, slowmoExit);
    }

    public void Buff()
    {
        mychar.ActivateBuffState(damageBuff, damageResistance, speedAcceleration, timeScale, timeDuration);
        atenea.FarFarAway();
        AudioManager.instance.PlaySound("slowmo_enter");
    }


    protected override void OnOneShotExecute()
    {
    }
    protected override void OnUpdateSkill()
    {
    }
    
    protected override void OnEndSkill() { }
    protected override void OnStartUse()
    {
        atenea.GoToHero(offset);
        atenea.Anim_Heal();
    }
    protected override void OnStopUse()
    {
        mychar.DesactivateBuffState(damageBuff);
        atenea.FarFarAway();

        AudioManager.instance.PlaySound("slowmo_exit", OnEndSound);
    }

    public void OnEndSound()
    {
        mychar.DesactivateBuffState(damageBuff);
    }

    protected override void OnUpdateUse() { }
}
