using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeCondition_CharacterLowHealth : CombatNodeCondition
{
    CharLifeSystem charlifesystem;
    // current*100/max = 50

    public int percert_to_condition = 25;

    bool Lowhealth
    {
        get
        {
            if (charlifesystem == null) return false;
            else return percert_to_condition >= (charlifesystem.GetLife() * 100 / charlifesystem.GetMax());
        }
    }

    protected override void OnInit()
    {
        charlifesystem = Main.instance.GetChar().Life;
    }

    public override bool RefreshPredicate()
    {
        if (Lowhealth)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
