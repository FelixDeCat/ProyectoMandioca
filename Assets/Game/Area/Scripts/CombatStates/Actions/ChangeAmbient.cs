using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeAmbient : CombatNodeAction
{
    [SerializeField] Light light;
    [SerializeField] Color Color1;
    [SerializeField] Color Color2;
    
    public override void OnInit() => light.color = Color1;
    public override void OnExit() => light.color = Color2;
    
    #region En desuso
    public override void OnRefresh()
    {

    }
    #endregion
}
