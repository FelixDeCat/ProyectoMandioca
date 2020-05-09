using UnityEngine;
public class SkillRevive : SkillBase
{
    [SerializeField] Minion model_minion = null;
    protected override void OnBeginSkill() => Main.instance.eventManager.SubscribeToEvent(GameEvents.ENEMY_DEAD, EnemyDeath);
    void EnemyDeath(params object[] param)
    {
        Vector3 pos = (Vector3)param[0];
        Minion myMinion = Instantiate(model_minion);
        myMinion.transform.position = pos;
        myMinion.owner = Main.instance.GetChar();
        myMinion.Initialize();
    }
    protected override void OnEndSkill() { }
    protected override void OnUpdateSkill() { }

}
