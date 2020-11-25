using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CDModule
{
    Action OnUpdateCD = delegate { };
    Dictionary<string, Action[]> EndCDCallbacks = new Dictionary<string, Action[]>();

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
        Action[] temp = new Action[2];
        EndCallback += () => OnUpdateCD -= Updater;
        temp[0] = EndCallback;
        temp[1] = () => OnUpdateCD -= Updater;
        EndCDCallbacks.Add(_cdName, temp);
    }

    public void EndCDWithExecute(string _cdName)
    {
        if (!EndCDCallbacks.ContainsKey(_cdName)) return;

        Action x = EndCDCallbacks[_cdName][0];
        EndCDCallbacks.Remove(_cdName);
        x();
    }

    public void EndCDWithoutExecute(string _cdName)
    {
        if (!EndCDCallbacks.ContainsKey(_cdName)) return;

        EndCDCallbacks[_cdName][1]();
        EndCDCallbacks.Remove(_cdName);
    }

    public void ResetAll()
    {
        foreach (var item in EndCDCallbacks)
        {
            item.Value[0]();
        }

        EndCDCallbacks.Clear();
    }

    public void ResetAllWithoutExecute()
    {
        foreach (var item in EndCDCallbacks)
        {
            if (!EndCDCallbacks.ContainsKey(item.Key)) return;

            EndCDCallbacks[item.Key][1]();
            //EndCDCallbacks.Remove(item.Key);
        }

        EndCDCallbacks.Clear();
    }
}
