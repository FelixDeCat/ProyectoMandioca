using IA_Felix;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class WalkingEntity : EntityBase
{
    [SerializeField]protected RigidbodyPathFinder rig_path_finder;
    
    protected override void OnUpdate() { if (executeAStar) { rig_path_finder.Refresh(); } OnUpdateEntity(); }
    protected abstract void OnUpdateEntity();
    private bool executeAStar;
    public void InitializePathFinder(Rigidbody rb) { rig_path_finder.Initialize(rb); }
    protected virtual void Callback_IHaveArrived(Action EndArrived) { rig_path_finder.AddCallbackEnd(EndArrived); }
    public void GoToPosition(Vector3 pos) { rig_path_finder.Execute(pos); executeAStar = true; }
    
}
