using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeAmbient : CombatNodeAction
{
    [SerializeField] Light ambientLight = null;
    [SerializeField] Color Color1 = Color.red;
    [SerializeField] Color Color2 = Color.white;
    
    public override void OnInit() => ambientLight.color = Color1;
    public override void OnExit() => ambientLight.color = Color2;
    
    #region En desuso
    public override void OnRefresh()
    {

    }
    #endregion
}
