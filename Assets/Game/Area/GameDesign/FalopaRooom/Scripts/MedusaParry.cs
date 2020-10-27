using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedusaParry : MonoBehaviour
{
    [SerializeField] private float duration = 3;

    public void OnEquip()
    {
        Main.instance.eventManager.SubscribeToEvent(GameEvents.ON_PLAYER_PARRY, PetrifyEnemies);
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
        var enemies = Physics.OverlapSphere(Main.instance.GetChar().transform.position, 5);
        foreach (var item in enemies)
        {
            EffectReceiver effect = item.GetComponent<EffectReceiver>();
            if (effect != null)
            {
                effect.ExtendEffectDuration(EffectName.OnPetrify);
            }
        }
    }

    public void OnUnequip()
    {
        Main.instance.eventManager.UnsubscribeToEvent(GameEvents.ON_PLAYER_PARRY, PetrifyEnemies);
    }
}
