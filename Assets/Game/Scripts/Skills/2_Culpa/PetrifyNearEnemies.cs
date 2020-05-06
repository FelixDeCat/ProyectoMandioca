using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DevelopTools;

public class PetrifyNearEnemies : SkillBase
{
    [SerializeField] float buffRange = 20;

    [SerializeField] ParticleSystem feedbackParticle = null;
    
    protected override void OnBeginSkill()
    {
        Main.instance.eventManager.SubscribeToEvent(GameEvents.ENEMY_DEAD, PetrifyWhenDie);
    }

    protected override void OnEndSkill()
    {
        Main.instance.eventManager.UnsubscribeToEvent(GameEvents.ENEMY_DEAD, PetrifyWhenDie);
        feedbackParticle.Stop();
    }

    protected override void OnUpdateSkill()
    {

    }

    void PetrifyWhenDie(params object[] param)
    {
        Vector3 pos = (Vector3)param[0];
        bool isPetrified = (bool)param[1];

        if (isPetrified)
        {
            feedbackParticle.Stop();
            feedbackParticle.transform.position = pos;
            feedbackParticle.Play();

            var overlap = Physics.OverlapSphere(pos, buffRange);

            foreach (var item in overlap)
                if (item.GetComponent<EnemyBase>())
                    item.GetComponent<EnemyBase>().OnPetrified();
        }
    }
}
