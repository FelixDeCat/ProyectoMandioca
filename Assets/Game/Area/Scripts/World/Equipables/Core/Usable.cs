using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Usable : Equipable
{
    public void BeginUse() => OnBeginUse();
    public void EndUse() => OnEndUse();
    public void UpdateUse() => OnUpdateUse();
    protected abstract void OnBeginUse();
    protected abstract void OnEndUse();
    protected abstract void OnUpdateUse();
}
