using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EXE_ChangeFase : ExecutableBase
{
    public int ID_Fase;
    public int index_fase;

    Func<bool> predicate = delegate { return true; };
    protected override bool OnCanExecute() {  return predicate.Invoke(); }
    protected override void OnExecute() { ManagerGlobalFases.instance.ModifyFase(ID_Fase, index_fase); }
}
