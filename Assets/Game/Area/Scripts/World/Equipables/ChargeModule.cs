using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Tools.EventClasses;

public class ChargeModule : MonoBehaviour
{
    Action<float, float> callback_refreshCasting = delegate { };
    Action callback_Begin = delegate { };
    Action callback_End = delegate { };
    Action<int> callbackRelease = delegate { };
    Action<bool> callback_HoldThePower = delegate { };
    public enum CastType { Normal_OnRelease, Normal_Automatic }
    public CastType cast_type;
    bool manual_success = false;

    public bool AddCharges = false;

    public Sprite contextual_charge;

    [SerializeField] float casting = 2;
    public float CastTime { get { return casting; } }
    float timer = 0;
    bool anim;
    public bool IsRunning { get { return anim; } }
    int charges;

    #region Builder
    public ChargeModule Subscribe_Feedback_Refresh(Action<float, float> _callback) { callback_refreshCasting = _callback; return this; }
    public ChargeModule Subscribe_Feedback_Begin(Action _callback) { callback_Begin = _callback; return this; }
    public ChargeModule Subscribe_Feedback_End(Action _callback) { callback_End = _callback; return this; }
    public ChargeModule Subscribe_Feedback_HoldThePower(Action<bool> _callback) { callback_HoldThePower = _callback; return this; }
    public ChargeModule Subscribe_Feedback_OnRelease(Action<int> _callback) { callbackRelease = _callback; return this; }

    #endregion

    public void BeginPress()
    {
        timer = 0;
        anim = true;
        callback_Begin.Invoke();
        ContextualBarSimple.instance.Show();
        if (contextual_charge) { ContextualBarSimple.instance.Set_Sprite_Photo(contextual_charge); }
    }
    public void StopPress()
    {

        ContextualBarSimple.instance.Hide();
        //if (cast_type == CastType.Normal_Automatic)
        //{
        //    timer = 0;
        //    anim = false;
        //    callbackRelease.Invoke(0);
        //}
        if (cast_type == CastType.Normal_OnRelease)
        {
            if (manual_success)
            {
                callbackRelease.Invoke(charges);
                callback_HoldThePower.Invoke(false);
            }
            else
            {
                callbackRelease.Invoke(0);
            }
            charges = 0;
            manual_success = false;
            timer = 0;
            anim = false;
        }
    }

    public void ForceCharges()
    {
        callback_HoldThePower.Invoke(true);
        manual_success = false;
        charges++;
    }

    public void ResetCooldown()
    {
        anim = false;
        timer = 0;
    }

    public void Pause() => anim = false;
    public void Resume() => anim = true;

    private void Update()
    {
        if (anim)
        {
            if (timer < casting)
            {
                timer = timer + 1 * Time.deltaTime;
                //callback_refreshCasting.Invoke(timer, casting);

                //aaacaaaaaaa
                if (contextual_charge)
                {
                    ContextualBarSimple.instance.Show();
                    ContextualBarSimple.instance.Set_Values_Load_Bar(casting, timer);
                }
            }
            else
            {
                //if (cast_type == CastType.Normal_Automatic)
                //{
                //    timer = 0;
                //    anim = false;
                //    callback_Sucess.Invoke();
                //    callback_End.Invoke();
                //}
                if (cast_type == CastType.Normal_OnRelease)
                {
                    if (!AddCharges) { anim = false; }
                    timer = 0;
                    manual_success = true;
                    charges++;
                    callback_End.Invoke();
                    callback_HoldThePower.Invoke(true);
                    ContextualBarSimple.instance.Hide();
                }
                
            }
        }
    }
}
