using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSkillsActiva : ItemInterceptor
{
    public SkillInfo info;

    public void EV_Collect()
    {
        Main.instance.GetActivesManager().ReplaceFor(info, myitemworld);
    }

    protected override bool OnCollect()
    {
        return true;
    }
}
