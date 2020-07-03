using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FireParry_skill : SkillBase
{
    [SerializeField] private Damagetype dmgType;
    [SerializeField] private float doTDuration;
    [SerializeField] private float timePerTick;
    [SerializeField] private int dmgPerTick;

    private List<EnemyBase> _enemies;
    
    protected override void OnBeginSkill()
    {
        Main.instance.eventManager.SubscribeToEvent(GameEvents.ON_PLAYER_PARRY, Fire);
    }

    void Fire(params object[] param)
    {
        var entity = (EntityBase)param[0];

        Debug.Log("Tiró evento");
        if (entity.GetComponent<EffectReceiver>())
            entity.GetComponent<EffectReceiver>().TakeEffect(EffectName.OnFire);
    }

    protected override void OnEndSkill()
    {
        Main.instance.eventManager.UnsubscribeToEvent(GameEvents.ON_PLAYER_PARRY, Fire);
    }

    protected override void OnUpdateSkill()
    {
        
    }
    
}
