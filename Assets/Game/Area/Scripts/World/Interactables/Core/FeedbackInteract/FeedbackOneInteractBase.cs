using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FeedbackOneInteractBase : MonoBehaviour
{
    protected bool canupdate = false;
    public void Execute() { canupdate = true; OnExecute(); }
    private void Update() { if (canupdate) OnUpdate(); }
    protected abstract void OnExecute();
    protected abstract void OnUpdate();
}
