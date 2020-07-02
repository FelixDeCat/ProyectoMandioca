using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SkillExplodeOnDeath : SkillBase
{
    [SerializeField] ParticleSystem FX = null;
    [SerializeField] float explosionRange = 10;
    [SerializeField] int explosionDmg = 20;

    protected override void OnBeginSkill()
    {
        Main.instance.eventManager.SubscribeToEvent(GameEvents.MINION_DEAD, ReceiveExplodeOnDeath);
    }

    protected override void OnEndSkill()
    {
        Main.instance.eventManager.UnsubscribeToEvent(GameEvents.MINION_DEAD, ReceiveExplodeOnDeath);
    }

    protected override void OnUpdateSkill()
    {
    }

    public void ReceiveExplodeOnDeath(params object[] p)
    {
        //Vector3 pos = (Vector3)p[0];
        //var listOfEntities = Physics.OverlapSphere(pos, explosionRange);


        //foreach (var item in listOfEntities)
        //{
        //    EnemyBase myEnemy = item.GetComponent<EnemyBase>();
        //    if (myEnemy)
        //        myEnemy.TakeDamage(explosionDmg, Vector3.up, Damagetype.explosion);
        //}
    }
}
