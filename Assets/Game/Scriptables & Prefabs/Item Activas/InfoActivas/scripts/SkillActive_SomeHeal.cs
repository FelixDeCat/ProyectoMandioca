using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillActive_SomeHeal : SkillActivas
{

    [SerializeField] private int healAmount = 20;
    [SerializeField] private ParticleSystem healFeedback = null;

    [SerializeField] Atenea atenea;

    protected override void OnBeginSkill() 
    {
        atenea.GetComponent<AnimEvent>().Add_Callback("Heal", AteneaHeal);
    }

    void AteneaHeal()
    {
        Main.instance.GetChar().Life.Heal(healAmount);
        healFeedback.Play();
    }

    protected override void OnOneShotExecute()
    {
        atenea.gameObject.SetActive(true);
        atenea.GoToHero();
        atenea.Anim_Heal();
    }
    protected override void OnUpdateSkill()
    {
        if (healFeedback.isPlaying)
            healFeedback.transform.position = Main.instance.GetChar().transform.position;
    }
    
    protected override void OnEndSkill() { }
    protected override void OnStartUse() { }
    protected override void OnStopUse() { }
    protected override void OnUpdateUse() { }
}
