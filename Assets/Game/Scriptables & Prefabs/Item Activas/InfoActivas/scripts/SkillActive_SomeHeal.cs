using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillActive_SomeHeal : SkillActivas
{

    [SerializeField] private int healAmount = 20;
    [SerializeField] private ParticleSystem healFeedback = null;

    protected override void OnOneShotExecute()
    {
        Main.instance.GetChar().Life.Heal(healAmount);
        healFeedback.Play();
    }
    protected override void OnUpdateSkill()
    {
        if (healFeedback.isPlaying)
            healFeedback.transform.position = Main.instance.GetChar().transform.position;
    }

    protected override void OnBeginSkill() { }
    protected override void OnEndSkill() { }
    protected override void OnStartUse() { }
    protected override void OnStopUse() { }
    protected override void OnUpdateUse() { }
}
