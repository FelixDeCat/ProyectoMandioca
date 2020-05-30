using System;
using DevelopTools.UI;
using UnityEngine;

[System.Serializable]
public class CharLifeSystem
{
    LifeSystemBase lifesystem;

    public FrontendStatBase frontendLife;

    public int life = 100;

    private bool godMode = false;

    public event Action<int, int> lifechange = delegate { };

    public event Action loselife = delegate { };
    public event Action gainlife = delegate { };
    public event Action death = delegate { };

    public CharLifeSystem Configure_CharLifeSystem()
    {
        lifesystem = new LifeSystemBase();
        lifesystem.Config(life, EVENT_OnLoseLife, EVENT_OnGainLife, EVENT_OnDeath, life);

        lifesystem.AddCallback_LifeChange(OnLifeChange);
        Debug_UI_Tools.instance.CreateToogle("GODMODE", false, ToogleDebug);
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

    //////////////////////////////////////////////////////////////////////////////////
    /// CALLBACKS
    //////////////////////////////////////////////////////////////////////////////////
    void OnLifeChange(int current, int max)
    {
        lifechange.Invoke(current, max);

        if(frontendLife) frontendLife.OnValueChange(current, max);
    }
    void EVENT_OnLoseLife() { loselife.Invoke(); }
    void EVENT_OnGainLife() { gainlife.Invoke(); }

    void EVENT_OnDeath()
    {
        if(!godMode)
            death.Invoke();
    }

    //////////////////////////////////////////////////////////////////////////////////
    /// PUBLIC METHODS
    //////////////////////////////////////////////////////////////////////////////////
    public bool Hit(int _val) => lifesystem.Hit(_val);
    public void Heal(int _val) => lifesystem.AddHealth(_val);

    public void Heal_AllHealth()
    {

        lifesystem.ResetLife();

    }

    public void AddHealth(int _val) => lifesystem.IncreaseLife(_val);
    public int GetLife() => (int)lifesystem.Life;
    public int GetMax() => lifesystem.GetMax();


}
