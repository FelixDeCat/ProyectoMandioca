using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedbackInteract_Scaler : FeedbackInteractBase
{
    public bool ownTransform;
    public Transform toscale;
    Vector3 startscale = Vector3.zero;
    Vector3 finalscale = Vector3.zero;
    public Vector3 cant_to_scale = new Vector3(2,2,2);
    private void Start()
    {
        if (ownTransform) toscale = this.transform;
        startscale = toscale.transform.localScale;
        finalscale = new Vector3(
            startscale.x + cant_to_scale.x, 
            startscale.y + cant_to_scale.y, 
            startscale.z + cant_to_scale.z);
    }
    protected override void OnShow() => toscale.transform.localScale = finalscale;
    protected override void OnHide() => toscale.transform.localScale = startscale;
    protected override void On_Condicional_Update() { }
    protected override void On_Permanent_Update() { }
}
