using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class BossSkills : MonoBehaviour,IPauseable
{
    Action SkillOver;
    bool onUse;
    bool pause;
    bool updating;
    int itterationAmmount;
    int currentItteration;
    float timer;
    float timeToItter;
    Action ItterFunc;
    Action ItterationOver;


    public virtual void Initialize()
    {

    }

    public void UseSkill(Action _SkillOver)
    {
        if (onUse) return;

        SkillOver = _SkillOver;
        OnUseSkill();
        onUse = true;
    }

    protected abstract void OnUseSkill();

    public virtual void OnUpdate()
    {
        if (updating)
        {
            if (timer == 0) ItterFunc();

            timer += Time.deltaTime;
            if (timer >= timeToItter)
            {
                timer = 0;
                currentItteration += 1;
            }

            if (currentItteration >= itterationAmmount)  ItterationOver?.Invoke();
        }
    }

    protected void OverSkill()
    {
        if (!onUse) return;
        SkillOver?.Invoke();
        updating = false;
        onUse = false;
        currentItteration = 0;
        timer = 0;
        OnOverSkill();
    }

    protected abstract void OnOverSkill();

    public void InterruptSkill()
    {
        if(!onUse) return;

        OnInterruptSkill();
        OverSkill();
    }

    protected abstract void OnInterruptSkill();

    public virtual void Pause()
    {
    }

    public virtual void Resume()
    {
    }
    protected void Itteration(int _itterationAmmount, float timeBetweenItteration, Action ItterationAction, Action _ItterationOver)
    {
        currentItteration = 0;
        timer = 0;
        itterationAmmount = _itterationAmmount;
        timeToItter = timeBetweenItteration;
        ItterFunc = ItterationAction;
        ItterationOver = _ItterationOver;
        updating = true;
    }
}
