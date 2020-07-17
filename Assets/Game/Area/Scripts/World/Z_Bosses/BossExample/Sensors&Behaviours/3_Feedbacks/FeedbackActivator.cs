using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedbackActivator : FeedbackBase
{
    [SerializeField] GameObject to_active_gameObject = null; 
    protected override void OnPlayFeedback() { }

    public void Activate(bool val)
    {
        to_active_gameObject.SetActive(val);
    }
}
