using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class Inspectable : MonoBehaviour
{
    //////////////////////////////////////////////////////////////
    /// VARS
    //////////////////////////////////////////////////////////////

    #region for inspector

    [Header("Inspectable")]
    public string meessage = "* enemy detected *";
    public enum InspectableType { Object, Humanoid, Interactable }
    public InspectableType inspectable_type;
    public Transform posToInspect;
    public float time_to_inspect;

    public int level_priority = 3;

    #endregion

    #region privates

    private float timer;
    private bool alreadyinspected = false;
    private bool begininspect;
    public bool something_wrong_with_this;

    #endregion

    #region for Callbacks

    Action Endinspect;
    Action<float, float> Valueinspect;
    Action AlreadyInspected;
    Action<string> MessageEndInspect;

    #endregion

    //////////////////////////////////////////////////////////////
    /// FUNCTIONS
    //////////////////////////////////////////////////////////////

    public void Initialize(ZoneBase sadsadsa)
    {

    }

    #region Bools Inspected
    public void MarkAsInspected() => alreadyinspected = true;
    public void UnMarkAsInspected() => alreadyinspected = false;
    public bool CanInspect() => !alreadyinspected;
    public void MarkSomethingWrong() => something_wrong_with_this = true;
    public void UnMarkSomethingWrong() => something_wrong_with_this = false;

    #endregion

    #region For Investigator
    public void Configure(
        Action _endinspect,
        Action<float, float> _valueinspect,
        Action _AlreadyInspected,
        Action<string> _message)
    {
        Endinspect = _endinspect;
        Valueinspect = _valueinspect;
        AlreadyInspected = _AlreadyInspected;
        MessageEndInspect = _message;
    }

    public void Inspect()
    {
        if (alreadyinspected)
        {
            AlreadyInspected();
            OnAlreadyInspected();
        }
        else
        {
            OnBeginInspect();
            begininspect = true;
            timer = 0;
        }
    }

    public void RefreshInspect()
    {
        if (begininspect)
        {
            if (timer < time_to_inspect)
            {
                timer = timer + 1 * Time.deltaTime;
                Valueinspect(time_to_inspect, timer);
                OnRefreshInspect();
            }
            else
            {
                Endinspect();
                MessageEndInspect(meessage);
                MarkAsInspected();
                OnEndInspect();
            }
        }
    }
    #endregion

    #region Abstracts
    protected abstract void OnBeginInspect();
    protected abstract void OnAlreadyInspected();
    protected abstract void OnEndInspect();
    protected abstract void OnRefreshInspect();
    #endregion

}
