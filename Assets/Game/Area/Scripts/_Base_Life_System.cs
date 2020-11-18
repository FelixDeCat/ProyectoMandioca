using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class _Base_Life_System : MonoBehaviour
{
    protected LifeSystemBase lifesystem = new LifeSystemBase();
    public int life = 100;
    

    public int Life { get { return (int)lifesystem.Life; } }
    public int LifeMax { get { return (int)lifesystem.GetMax(); } }

    public void Initialize(int life, Action _LoseLife, Action _GainLife, Action _Death)
    {
        lifesystem.Config(life, _LoseLife, _GainLife, _Death);
    }
    public void CreateADummyLifeSystem()
    {
        lifesystem.Config(life, LoseLife, GainLife, Death);
    }

    public void ResetLifeSystem() => lifesystem.ResetLife();
    void LoseLife() { }
    void GainLife() { }
    void Death() { }
    public virtual bool Hit(int _val)
    {
        return lifesystem.Hit(_val);
    }
}
