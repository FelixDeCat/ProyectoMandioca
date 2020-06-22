using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedbackSoundDataBase : MonoBehaviour
{
    [SerializeField] AudioClip _getDamageClip;
    [SerializeField] AudioClip _throwAttackClip;
    [SerializeField] AudioClip _beginFightClip;
    [SerializeField] AudioClip _deathClip;


    // Start is called before the first frame update
    void Start()
    {
        AudioManager.instance.GetSoundPool(_getDamageClip.name, AudioGroups.GAME_FX, _getDamageClip);
        AudioManager.instance.GetSoundPool(_throwAttackClip.name, AudioGroups.GAME_FX, _throwAttackClip);
        AudioManager.instance.GetSoundPool(_beginFightClip.name, AudioGroups.GAME_FX, _beginFightClip);
        AudioManager.instance.GetSoundPool(_deathClip.name, AudioGroups.GAME_FX, _deathClip);


    }



    public void GetDamageClip()
    {
        AudioManager.instance.PlaySound(_getDamageClip.name, transform);
    }

    public void ThrowAttackClip()
    {
        AudioManager.instance.PlaySound(_throwAttackClip.name, transform);
    }

    public void BeginFightClip()
    {
        AudioManager.instance.PlaySound(_beginFightClip.name, transform);
    }

    public void DeathClip()
    {
        AudioManager.instance.PlaySound(_deathClip.name, transform);

    }
}
