using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class BossSkills : MonoBehaviour
{
    Action SkillOver;

    public void UseSkill(Action _SkillOver)
    {
        SkillOver = _SkillOver;
        OnUseSkill();
    }

    protected abstract void OnUseSkill();

    protected void OverSkill()
    {
        SkillOver?.Invoke();
        OnOverSkill();
    }

    protected abstract void OnOverSkill();

    public void InterruptSkill()
    {
        OnInterruptSkill();
        OverSkill();
    }

    protected abstract void OnInterruptSkill();
}
