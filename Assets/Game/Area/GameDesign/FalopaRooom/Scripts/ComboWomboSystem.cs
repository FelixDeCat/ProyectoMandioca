using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class ComboWomboSystem
{

    Action OnComboReady;

    int hitCount;
    public int hitsNeededToCombo;

    public void AddCallback_OnComboready(Action callback) => OnComboReady += callback;
    public void RemoveCallback_OnComboready(Action callback) => OnComboReady -= callback;

    public void Initialize(int hitsNeeded)
    {
        hitsNeededToCombo = hitsNeeded;
    }

    public void AddHit()
    {
        hitCount++;

        if(ComboReady())
        {
            OnComboReady?.Invoke();

            ClearCombo();
        }
    }
    bool ComboReady() => hitCount >= hitsNeededToCombo;




    void ClearCombo()
    {
        hitCount = 0;
    }

    

}
