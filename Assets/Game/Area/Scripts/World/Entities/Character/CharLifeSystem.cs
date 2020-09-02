﻿using System;
using DevelopTools.UI;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class CharLifeSystem: _Base_Life_System
{
    public FrontendStatBase frontendLife;

    private bool godMode = false;

    public event Action<int, int> lifechange = delegate { };

    public event Action loselife = delegate { };
    public event Action gainlife = delegate { };
    public event Action death = delegate { };
    public event Action caronte = delegate { };

    bool oneShotCaronte = false;

    CharFeedbacks feedbacks = null;

    public CharLifeSystem Configure_CharLifeSystem()
    {
        Initialize();
        lifesystem.Config(life, EVENT_OnLoseLife, EVENT_OnGainLife, EVENT_OnDeath, life);

        lifesystem.AddCallback_LifeChange(OnLifeChange);
        Debug_UI_Tools.instance.CreateToogle("GODMODE", false, ToogleDebug);
        ADD_EVENT_OnChangeValue(Main.instance.gameUiController.OnChangeLife);
        
        return this;
    }

    string ToogleDebug(bool active) { godMode = active; ; return active ? "debug activado" : "debug desactivado"; }

    //////////////////////////////////////////////////////////////////////////////////
    /// EVENTS Subscribers
    //////////////////////////////////////////////////////////////////////////////////
    public CharLifeSystem ADD_EVENT_OnChangeValue(Action<int, int> _callback) { lifechange += _callback; return this; }
    public CharLifeSystem REMOVE_EVENT_OnChangeValue(Action<int, int> _callback) { lifechange -= _callback; return this; }
    public CharLifeSystem ADD_EVENT_OnLoseLife(Action _callback) { loselife += _callback; return this; }
    public CharLifeSystem REMOVE_EVENT_OnLoseLife(Action _callback) { loselife -= _callback; return this; }
    public CharLifeSystem ADD_EVENT_OnGainLife(Action _callback) { gainlife += _callback; return this; }
    public CharLifeSystem REMOVE_EVENT_OnGainLife(Action _callback) { gainlife -= _callback; return this; }
    public CharLifeSystem ADD_EVENT_Death(Action _callback) { death += _callback; return this; }
    public CharLifeSystem REMOVE_EVENT_Death(Action _callback) { death -= _callback; return this; }
    public CharLifeSystem ADD_EVENT_OnCaronteDeathEvent(Action _callback) { caronte += _callback; return this; }
    public CharLifeSystem REMOVE_EVENT_OnCaronteDeathEvent(Action _callback) { caronte -= _callback; return this; }

    //////////////////////////////////////////////////////////////////////////////////
    /// CALLBACKS
    //////////////////////////////////////////////////////////////////////////////////
    void OnLifeChange(int current, int max)
    {
        lifechange.Invoke(current, max);
    }
    void EVENT_OnLoseLife() { loselife.Invoke();}


    void EVENT_OnGainLife()
    {
        if(feedbacks != null) feedbacks.sounds.Play_TakeHeal();
        gainlife.Invoke();
    }

    void EVENT_OnDeath()
    {
        if (!oneShotCaronte)
        {
            oneShotCaronte = true;
            caronte?.Invoke();
            return;
        }

        
        if (!godMode)
        {
            oneShotCaronte = false;
            death.Invoke();
        }
            
    }

    //////////////////////////////////////////////////////////////////////////////////
    /// PUBLIC METHODS
    //////////////////////////////////////////////////////////////////////////////////
    public void Heal(int _val) => lifesystem.AddHealth(_val);

    public bool CanHeal() { return lifesystem.CanHealth(); }

    public void Heal_AllHealth()
    {
        Main.instance.gameUiController.ResetYellowHeart();
        lifesystem.ResetLife();
        
    }

    public void AddHealth(int _val) => lifesystem.IncreaseLife(_val);
    public int GetLife() => (int)lifesystem.Life;
    public int GetMax() => lifesystem.GetMax();


}
