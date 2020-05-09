using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using ToolsMandioca.Extensions;

public class SkillFocusOnParry : SkillBase
{
    protected override void OnBeginSkill()
    {

    }

    public void ReceiveFocusOnParry(EntityBase entity)
    {
        foreach (var item in Main.instance.GetMinions())
        {
            item.SetTarget(entity);
        }
    }

    protected override void OnEndSkill() { }
    protected override void OnUpdateSkill() { }
}
