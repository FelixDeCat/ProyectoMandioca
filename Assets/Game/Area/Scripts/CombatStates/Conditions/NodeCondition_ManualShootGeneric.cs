using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeCondition_ManualShootGeneric : CombatNodeCondition
{
    #region En Desuso, porque al ser shoot manual, usamos las funciones de mi parent class
    public override bool RefreshPredicate() { return false; }
    protected override void OnInit() { }
    #endregion
}
