using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class SkillActivas : SkillBase
{
    Action<SkillInfo, float> CallbackCooldown = delegate { };
    Action<SkillInfo> CallbackEndCooldown = delegate { };

    Action<SkillInfo> CallbackSuscessfullUsed = delegate { };

    [Header("Cooldown Settings")]
    public float cooldown;
    float time_cooldown;
    bool begincooldown;

    [Header("Use in Time Settings")]
    public bool one_time_use = true;
    public float useTime = 5f;
    
    float timer_use = 0;
    bool beginUse;
    Func<bool> predicate;
    bool usePredicate;

    [Header("Update By Coroutine")]
    public bool use_coroutine = false;
    bool stop;


    public void SetCallbackSuscessfulUsed(Action<SkillInfo> callback) { CallbackSuscessfullUsed = callback; }

    public void SetCallbackCooldown(Action<SkillInfo, float> callback) => CallbackCooldown = callback;
    public void SetCallbackEndCooldown(Action<SkillInfo> callback) => CallbackEndCooldown = callback;
    public void RemoveCallbackCooldown() => CallbackCooldown = delegate { };
    protected void SetPredicate(Func<bool> pred)
    {
        predicate = pred;
        usePredicate = true;
    }

    public override void BeginSkill()
    {
        begincooldown = true;

        if (!is3D)
        {
            ui_skill.SetImages(skillinfo.img_avaliable, skillinfo.img_actived);
            ui_skill.Cooldown_ConfigureTime(cooldown);
        }
        else
        {
            CallbackCooldown(skillinfo, cooldown);
        }
        
        base.BeginSkill();
    }
    public override void EndSkill()
    {
        base.EndSkill();
    }

    public void Execute()
    {
        if (usePredicate)
            if (!predicate()) return;

        if (!begincooldown)
        {
            begincooldown = true;
            time_cooldown = 0;

            CallbackSuscessfullUsed(this.skillinfo);

            if (one_time_use)
            {
                OnOneShotExecute();
            }
            else
            {
                OnStartUse();

                if (!use_coroutine)
                {
                    timer_use = 0;
                    beginUse = true;
                }
                else
                {
                    StartCoroutine(UseTime());
                    StartCoroutine(CustomUpdate());
                }
            }
        }
    }

    protected abstract void OnStartUse();
    protected abstract void OnStopUse();
    protected abstract void OnUpdateUse();

    internal override void absUpdate()
    {
        base.absUpdate();
        if (beginUse)
        {
            if (timer_use < useTime)
            {
                timer_use = timer_use + 1 * Time.deltaTime;
                OnUpdateUse();
            }
            else
            {
                timer_use = 0;
                beginUse = false;
                OnStopUse();
            }
        }
    }

    IEnumerator UseTime()
    {
        stop = false;
        yield return new WaitForSecondsRealtime(useTime);
        stop = true;
        OnStopUse();

        yield return null;
    }
    IEnumerator CustomUpdate()
    {
        while (!stop)
        {
            CoroutineUpdate();
            yield return new WaitForFixedUpdate();
        }
    }
    protected virtual void CoroutineUpdate()
    {

    }

    internal override void cooldownUpdate()
    {
        base.cooldownUpdate();
        if (begincooldown)
        {
            if (time_cooldown < cooldown)
            {
                time_cooldown = time_cooldown + 1 * Time.deltaTime;

                var auxpercent = time_cooldown * 100 / cooldown;
                auxpercent = auxpercent * 0.01f;
                CallbackCooldown(skillinfo, auxpercent);
                //ui_skill.Cooldown_SetValueTime(time_cooldown);
            }
            else
            {
                CallbackEndCooldown.Invoke(skillinfo);
                time_cooldown = 0;
                begincooldown = false;
            }
        }
    }

    protected abstract void OnOneShotExecute();
}
