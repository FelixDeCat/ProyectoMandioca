using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum comboStates
{
    disabled,
    running,
    cdBetweenHit,
    comboReady
}

[Serializable]
public class ComboWomboSystem
{
    Action OnComboReady = default;
    Action OnComboResetFeedback = default;

    delegate void OnExecute(params object[] objs);
    Action executeCombo = default;

    int currHitCount = 0;
    [SerializeField] int hitsNeededToCombo = 3;
    [Tooltip("If true: Se resetea el contador si hago mas de los ataques necesarios")]
    [SerializeField] bool resetOnSurpass = false;

    [SerializeField] float cdToAddHit = 0.1f;
    public float CdToAddHit { get => cdToAddHit; }

    float currTimeToCombo = 0;
    [SerializeField] float timeToCombo = 1;
    AudioClip comboSounds;

    comboStates comboState = comboStates.disabled;

    public void AddCallback_OnComboready(Action callback, bool add = true) { if (add) OnComboReady += callback; else OnComboReady -= callback; }
    public void AddCallback_OnComboReset(Action callback) => OnComboResetFeedback = callback;
    public void AddCallback_OnExecuteCombo(Action callback) => executeCombo += callback;
    public void RemoveCallback_OnExecuteCombo(Action callback) => executeCombo -= callback;
    public void Clear_OnExecuteCombo() => executeCombo = null;


    public void Initialize(int hitsNeeded)//, AudioClip sound)
    {
        hitsNeededToCombo = hitsNeeded;
        //comboSounds = sound;
        //AudioManager.instance.GetSoundPool(comboSounds.name, AudioGroups.GAME_FX, comboSounds);
    }
    public void SetSound(AudioClip sound)
    {
        comboSounds = sound;
        AudioManager.instance.GetSoundPool(comboSounds.name, AudioGroups.GAME_FX, comboSounds);
    }

    public void OnUpdate()
    {
        if (comboState != comboStates.disabled)
        {
            currTimeToCombo += Time.deltaTime;

            if (currTimeToCombo >= timeToCombo)
            {
                if(OnComboResetFeedback != null) OnComboResetFeedback.Invoke();
                ClearCombo();
            }
        }
        if (comboState == comboStates.cdBetweenHit)
        {
            timer += Time.deltaTime;
            if (timer > cdToAddHit)
            {
                comboState = comboStates.running;
                timer = 0;
            }
        }
    }

    float timer = 0;

    public void TryAddHit()
    {
        if (comboState == comboStates.cdBetweenHit) return;
        else if (comboState == comboStates.comboReady && resetOnSurpass) ClearCombo();
        currHitCount++;

        currTimeToCombo = 0;
        timer = 0;
        if (currHitCount >= hitsNeededToCombo)
        {
            comboState = comboStates.comboReady;
            if(OnComboReady != null) OnComboReady.Invoke();
            if (comboSounds != null) AudioManager.instance.PlaySound(comboSounds.name);
        }
        else
            comboState = comboStates.cdBetweenHit;
    }

    public bool TryExecuteCombo()
    {
        if (comboState == comboStates.comboReady)
        {
            executeCombo.Invoke();
            ClearCombo();
            return true;
        }
        ClearCombo();
        return false;
    }

    void ClearCombo()
    {
        currHitCount = 0;
        currTimeToCombo = 0;
        timer = 0;
        comboState = comboStates.disabled;
    }
}
