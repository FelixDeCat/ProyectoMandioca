using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Tools.Extensions;

public class SkillFocusOnParry : SkillBase
{

    List<FocusOnParryComponent> focusOnParryComponents = new List<FocusOnParryComponent>();


    protected override void OnBeginSkill()
    {
        Main.instance.eventManager.SubscribeToEvent(GameEvents.ENEMY_SPAWN, OnNewEnemySpawned);
        Main.instance.eventManager.SubscribeToEvent(GameEvents.MINION_SPAWN, OnNewMinionSpawned);
        //lo mismo pero desubscribir al minion cuando muere
    }

    void OnNewMinionSpawned(params object[] param)
    {
        var focusedOnParry = ((Minion)param[0]).GetComponent<FocusOnParryComponent>();
        focusedOnParry.Configure(ReceiveFocusOnParry);
        focusedOnParry.OnBegin();
        focusOnParryComponents.Add(focusedOnParry);
    }

    void OnNewEnemySpawned(params object[] param)
    {
        var enemybase = (EnemyBase)param[0];
        enemybase.OnParried += enemybase.GetFocusedOnParry;
    }

    public void ReceiveFocusOnParry(Vector3 pos, FocusOnParryComponent component)
    {
        foreach (var item in focusOnParryComponents)
        {
            Minion myMinion = item.GetComponent<Minion>();

            var enemies = Main.instance.GetEnemiesByPointInRadius(myMinion.transform.position, 10).Select(x => (TrueDummyEnemy)x);

            //Debug.Log(enemies.StringConcatCollection());

            foreach (var enemy in enemies)
            {
                if(enemy.minionTarget)
                {
                    myMinion.ChangeToAttackState(enemy.transform);
                }
            }
        }
    }

    protected override void OnEndSkill() { }
    protected override void OnUpdateSkill() { }
}
