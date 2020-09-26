using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class _Base_Life_System : MonoBehaviour
{
    protected LifeSystemBase lifesystem;
    public int life = 100;

    public int Life { get { return (int)lifesystem.Life; } }

    public void Initialize()
    {
        lifesystem = new LifeSystemBase();
    }
    public void Initialize(int life, Action LoseLife, Action GainLife, Action Death)
    {
        lifesystem = new LifeSystemBase();
        lifesystem.Config(life, LoseLife, GainLife, Death);
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
