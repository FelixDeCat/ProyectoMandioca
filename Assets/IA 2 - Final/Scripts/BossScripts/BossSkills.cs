using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class BossSkills : MonoBehaviour
{
    Action SkillOver;
    bool onUse;
    bool pause;

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

    protected void OverSkill()
    {
        if (!onUse) return;
        SkillOver?.Invoke();
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
}
