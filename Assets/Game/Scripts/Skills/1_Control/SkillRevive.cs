using UnityEngine;
public class SkillRevive : SkillBase
{
    [SerializeField] GameObject model_minion = null;
    protected override void OnBeginSkill() => Main.instance.eventManager.SubscribeToEvent(GameEvents.ENEMY_DEAD, EnemyDeath);
    void EnemyDeath(params object[] param)
    {
        Vector3 pos = (Vector3)param[0];
        var myMinion = GameObject.Instantiate(model_minion);
        myMinion.transform.position = pos;
        myMinion.GetComponent<Minion>().Initialize();
    }
    protected override void OnEndSkill() { }
    protected override void OnUpdateSkill() { }

}
