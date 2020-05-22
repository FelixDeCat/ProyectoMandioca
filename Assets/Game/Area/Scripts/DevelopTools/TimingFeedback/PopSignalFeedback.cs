using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PopSignalFeedback
{
    protected Action<EntityBase> callback;
    Action endTimer;

    [Header("CombatComponentFeedback")]
    GameObject obj;
    float timerfeedback;
    float time_to_pop;
    bool can_refresh;

    public PopSignalFeedback(float _time_to_pop, GameObject _obj, Action _endTimer)
    {
        obj = _obj;
        time_to_pop = _time_to_pop;
        endTimer = _endTimer;
    }
    public PopSignalFeedback(float _time_to_pop, GameObject _obj)
    {
        obj = _obj;
        time_to_pop = _time_to_pop;
        endTimer = delegate { };
    }

    public void Show()
    {
        obj.SetActive(true);
        can_refresh = true;
        timerfeedback = 0;
    }


    public void Refresh()
    {
        if (can_refresh)
        {
            if (timerfeedback < time_to_pop)
            {
                timerfeedback = timerfeedback + 1 * Time.deltaTime;
            }
            else
            {
                can_refresh = false;
                timerfeedback = 0;
                obj.SetActive(false);
                endTimer.Invoke();
            }
        }
    }
}
