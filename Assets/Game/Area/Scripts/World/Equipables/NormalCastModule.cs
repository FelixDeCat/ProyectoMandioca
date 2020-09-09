using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Tools.EventClasses;

public class NormalCastModule : MonoBehaviour
{
    Action<float, float> callback_refreshCasting = delegate { };
    Action callback_Begin = delegate { };
    Action callback_End = delegate { };
    Action callback_fail = delegate { };
    Action callback_Sucess = delegate { };
    Action<bool> callback_HoldThePower = delegate { };
    public enum CastType { Normal_OnRelease, Normal_Automatic }
    public CastType cast_type;
    bool manual_success = false;


    [SerializeField] float casting;
    public float CastTime { get { return casting; } }
    float timer = 0;
    bool anim;
    public bool IsRunning { get { return anim; } }

    #region Builder
    public NormalCastModule Subscribe_Feedback_Refresh(Action<float, float> _callback) { callback_refreshCasting = _callback; return this; }
    public NormalCastModule Subscribe_Feedback_Begin(Action _callback) { callback_Begin = _callback; return this; }
    public NormalCastModule Subscribe_Feedback_End(Action _callback) { callback_End = _callback; return this; }
    public NormalCastModule Subscribe_Feedback_HoldThePower(Action<bool> _callback) { callback_HoldThePower = _callback; return this; }
    public NormalCastModule Subscribe_Feedback_CastingFail(Action _callback) { callback_fail = _callback; return this; }
    public NormalCastModule Subscribe_Sucess(Action _callback) { callback_Sucess = _callback; return this; }

    #endregion

    public void StartCast()
    {
        timer = 0;
        anim = true;
        callback_Begin.Invoke();
    }
    public void StopCast()
    {
        if (cast_type == CastType.Normal_Automatic)
        {
            timer = 0;
            anim = false;
            callback_fail.Invoke();
        }
        if (cast_type == CastType.Normal_OnRelease)
        {
            if (manual_success)
            {
                callback_Sucess.Invoke();
                callback_HoldThePower.Invoke(false);
            }
            else
            {
                callback_fail.Invoke();
            }
            manual_success = false;
            timer = 0;
            anim = false;
        }
        
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
                callback_refreshCasting.Invoke(timer, casting);
            }
            else
            {
                if (cast_type == CastType.Normal_Automatic)
                {
                    timer = 0;
                    anim = false;
                    callback_Sucess.Invoke();
                    callback_End.Invoke();
                }
                if (cast_type == CastType.Normal_OnRelease)
                {
                    timer = 0;
                    anim = false;
                    manual_success = true;
                    callback_End.Invoke();
                    callback_HoldThePower.Invoke(true);
                }
                
            }
        }
    }
}
