using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SkillPetrify : SkillBase
{
    [SerializeField] float petrifyRange = 10;

    protected override void OnBeginSkill()
    {
        Main.instance.eventManager.SubscribeToEvent(GameEvents.MINION_DEAD, ReceivePetrifyOnDeathMinion);
    }

    protected override void OnEndSkill()
    {
        Main.instance.eventManager.UnsubscribeToEvent(GameEvents.MINION_DEAD, ReceivePetrifyOnDeathMinion);
    }

    protected override void OnUpdateSkill()
    {
    }

    public void ReceivePetrifyOnDeathMinion(params object[] p)
    {
        Vector3 pos = (Vector3)p[0];

        var listOfEntities = Physics.OverlapSphere(pos, petrifyRange );

        foreach (var item in listOfEntities)
        {
            EnemyBase myEnemy = item.GetComponent<EnemyBase>();
            if (myEnemy)
            {
                //myEnemy.OnPetrified();
            }
        }
    }
}
