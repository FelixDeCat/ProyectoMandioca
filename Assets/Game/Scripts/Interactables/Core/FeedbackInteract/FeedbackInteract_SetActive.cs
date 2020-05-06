using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedbackInteract_SetActive : FeedbackInteractBase
{
    public bool active_on_feedback = true;
    public GameObject to_active;

    private void Start() { to_active.SetActive( ! active_on_feedback); }
    protected override void OnShow() => to_active.SetActive(active_on_feedback);
    protected override void OnHide() => to_active.SetActive( ! active_on_feedback);
    protected override void On_Condicional_Update() { }
    protected override void On_Permanent_Update() { }
}
