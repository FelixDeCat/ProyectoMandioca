using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class ComboWomboSystem
{

    Action OnComboReady;
    Action OnComboResetFeedback;

    int hitCount;
    public int hitsNeededToCombo;
    public float cdToAddHit;
    public float timeToCombo;
    AudioClip comboSounds;

    float _count;
    bool comboRunning;

    public void AddCallback_OnComboready(Action callback) => OnComboReady = callback;
    public void AddCallback_OnComboReset(Action callback) => OnComboResetFeedback = callback;

    public void Initialize(int hitsNeeded,AudioClip sound)
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
        if(comboRunning)
        {
            _count += Time.deltaTime;
           
            if (_count >= timeToCombo)
            {
                OnComboResetFeedback.Invoke();
                ClearCombo();
            }
        }
        
    }

    public void AddHit()
    {
        hitCount++;
        
        comboRunning = true;
        _count = 0;
        if (hitCount >= hitsNeededToCombo)
            hitCount = hitsNeededToCombo;

        if(ComboReady())
        {
            OnComboReady?.Invoke();
            AudioManager.instance.PlaySound(comboSounds.name);
            ClearCombo();
        }
    }
    bool ComboReady() => hitCount >= hitsNeededToCombo;




    void ClearCombo()
    {
        
          hitCount = 0;
        _count = 0;
        comboRunning = false;
    }

    

}
