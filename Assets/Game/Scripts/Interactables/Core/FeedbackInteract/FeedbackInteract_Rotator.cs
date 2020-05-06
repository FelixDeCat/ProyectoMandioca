using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedbackInteract_Rotator : FeedbackInteractBase
{
    public Transform torot;
    Quaternion startrot = new Quaternion();
    public bool own;
    public bool loop;
    public Vector3 cant_to_rotate = new Vector3(0,1,0);
    private void Start()
    {
        if (own) torot = this.transform;
        startrot = torot.rotation;

        if (loop) canupdate = true;
    }
    protected override void OnShow() {  }
    protected override void OnHide() { torot.rotation = startrot; }
    protected override void On_Condicional_Update() 
    {
        if(!loop) torot.Rotate(cant_to_rotate);
    }

    protected override void On_Permanent_Update()
    {
        if (loop) torot.Rotate(cant_to_rotate);
    }
}
