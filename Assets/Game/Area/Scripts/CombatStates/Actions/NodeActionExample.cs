using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeActionExample : CombatNodeAction
{
    public override void OnInit() { Debug.Log("INIT ACTION"); }
    public override void OnExit() { Debug.Log("EXIT ACTION"); }
    public override void OnRefresh() { Debug.Log("REFRESH ACTION"); }
}
