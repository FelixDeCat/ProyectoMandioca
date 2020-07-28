using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fase_EXAMPLE : BaseFase
{

    protected override void OnInitState() { Debug.Log("FASE EXAMPLE Init"); }

    protected override void OnExitState() { Debug.Log("FASE EXAMPLE Exit"); }
    
    protected override void OnRefreshState() { Debug.Log("FASE EXAMPLE Refresh"); }
}
