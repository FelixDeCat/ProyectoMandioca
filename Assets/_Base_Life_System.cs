using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _Base_Life_System : MonoBehaviour
{
    protected LifeSystemBase lifesystem;
    public int life = 100;

    public void Initialize()
    {
        lifesystem = new LifeSystemBase();
    }
    public void CreateADummyLifeSystem()
    {
        lifesystem.Config(1, LoseLife, GainLife, Death);
    }
    void LoseLife() { }
    void GainLife() { }
    void Death() { }
    public virtual bool Hit(int _val) => lifesystem.Hit(_val);
}
