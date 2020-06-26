using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Tools.Extensions;

public class SkillFocusOnParry : SkillBase
{
    protected override void OnBeginSkill()
    {
        Main.instance.eventManager.SubscribeToEvent(GameEvents.ON_PLAYER_PARRY, ReceiveFocusOnParry);
    }

    public void ReceiveFocusOnParry(params object[] param)
    {
        foreach (var item in Main.instance.GetMinions())
        {
            item.SetTarget((EntityBase)param[0]);
        }
    }

    protected override void OnEndSkill()
    {
        Main.instance.eventManager.UnsubscribeToEvent(GameEvents.ON_PLAYER_PARRY, ReceiveFocusOnParry);
    }
    protected override void OnUpdateSkill() { }
}
