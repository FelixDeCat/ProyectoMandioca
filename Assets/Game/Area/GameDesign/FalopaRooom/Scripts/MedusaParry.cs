using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedusaParry : MonoBehaviour
{
    [SerializeField] private float duration = 3;
    [SerializeField] float areaToChainPetrify = 8;

    public void OnEquip()
    {
        Main.instance.eventManager.SubscribeToEvent(GameEvents.ON_PLAYER_PARRY, PetrifyEnemies);
        Main.instance.eventManager.SubscribeToEvent(GameEvents.ON_PLAYER_PARRY, ExtendPetrifyDuration);
    }

    void PetrifyEnemies(params object[] param)
    {
        var entity = (EntityBase)param[0];
        if (entity != null && entity.GetComponent<EffectReceiver>())
        {
            entity.GetComponent<EffectReceiver>().TakeEffect(EffectName.OnPetrify, duration);
        }
    }

    void ExtendPetrifyDuration()
    {
        var enemies = Main.instance.GetListOfComponentInRadiusByCondition(Main.instance.GetChar().transform.position, areaToChainPetrify, (x) => x.GetComponent<EffectReceiver>());
        
        foreach (var item in enemies)
        {
            item.GetComponent<EffectReceiver>().ExtendEffectDuration(EffectName.OnPetrify);            
        }
    }

    public void OnUnequip()
    {
        Main.instance.eventManager.UnsubscribeToEvent(GameEvents.ON_PLAYER_PARRY, PetrifyEnemies);
        Main.instance.eventManager.UnsubscribeToEvent(GameEvents.ON_PLAYER_PARRY, ExtendPetrifyDuration);
    }
}
