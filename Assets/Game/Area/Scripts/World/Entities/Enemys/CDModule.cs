using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CDModule
{
    Action OnUpdateCD = delegate { };
    Dictionary<string, Action> EndCDCallbacks = new Dictionary<string, Action>();

    public void UpdateCD() => OnUpdateCD();

    public void AddCD(string _cdName, Action _EndCallback, float _cdTime)
    {
        if (EndCDCallbacks.ContainsKey(_cdName)) return;

        float timer = 0;
        Action EndCallback = _EndCallback;
        Action Updater = () =>
        {
            timer += Time.deltaTime;
            if (timer >= _cdTime)
                EndCDWithExecute(_cdName);
        };
        OnUpdateCD += Updater;

        EndCallback += () => OnUpdateCD -= Updater;

        EndCDCallbacks.Add(_cdName, EndCallback);
    }

    public void EndCDWithExecute(string _cdName)
    {
        if (!EndCDCallbacks.ContainsKey(_cdName)) return;

        Action x = EndCDCallbacks[_cdName];
        EndCDCallbacks.Remove(_cdName);
        x();
    }

    public void EndCDWithoutExecute(string _cdName)
    {
        if (!EndCDCallbacks.ContainsKey(_cdName)) return;

        EndCDCallbacks.Remove(_cdName);
    }

    public void ResetAll()
    {
        foreach (var item in EndCDCallbacks)
        {
            item.Value();
        }

        EndCDCallbacks.Clear();
    }
}
