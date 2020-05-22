using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedbackOneInteract_ScaleByCurve : FeedbackOneInteractBase
{
    public AnimationCurve curve;
    float timer = 0;
    bool anim;

    Vector3 firstScale;

    [Range(0,3)][SerializeField] float speedMultiplier = 1;
    Keyframe lastkey;

    Vector3 scalerAux;

    private void Awake()
    {
        firstScale = this.transform.localScale;
    }

    protected override void OnExecute()
    {
        lastkey = curve[curve.length - 1];
        anim = true;
        timer = 0;

        Debug.Log("on execute");
    }

    protected override void OnUpdate()
    {
        if (anim)
        {
            
            if (timer < lastkey.time)
            {
                timer = timer + speedMultiplier * Time.deltaTime;
                scalerAux.x = curve.Evaluate(timer);
                scalerAux.y = curve.Evaluate(timer);
                scalerAux.z = curve.Evaluate(timer);
                transform.localScale = scalerAux;
            }
            else
            {
                anim = false;
                timer = 0;
                transform.localScale = firstScale;
            }
        }
    }
}
