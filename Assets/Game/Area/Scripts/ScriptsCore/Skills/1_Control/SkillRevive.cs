using UnityEngine;
using Tools;
using System.Collections.Generic;

public class SkillRevive : SkillBase
{
    [SerializeField] Minion model_minion = null;
    [SerializeField, Range(1,10)] int probToRevive = 5;
    [SerializeField] int maxMinionAmmount = 5;

    Dictionary<bool, int> posibilities;

    protected override void OnBeginSkill()
    {
        Main.instance.eventManager.SubscribeToEvent(GameEvents.ENEMY_DEAD, EnemyDeath);

        posibilities = RoulletteWheel.CreateDictionary(new List<bool>() { true, false }, new List<int>() { probToRevive, 10 - probToRevive });
    }

    public void ModifyProbability(int prob)
    {
        posibilities = RoulletteWheel.CreateDictionary(new List<bool>() { true, false }, new List<int>() { prob, 10 - prob });
    }

    void EnemyDeath(params object[] param)
    {
        if(Main.instance.GetMinions().Count < maxMinionAmmount)
        {
            if (RoulletteWheel.Roullette(posibilities))
            {
                Vector3 pos = (Vector3)param[0];
                Minion myMinion = Instantiate(model_minion);
                myMinion.transform.position = pos;
                myMinion.owner = Main.instance.GetChar();
                myMinion.Initialize();
            }
        }
    }
    protected override void OnEndSkill() => Main.instance.eventManager.UnsubscribeToEvent(GameEvents.ENEMY_DEAD, EnemyDeath);
    protected override void OnUpdateSkill() { }

}
