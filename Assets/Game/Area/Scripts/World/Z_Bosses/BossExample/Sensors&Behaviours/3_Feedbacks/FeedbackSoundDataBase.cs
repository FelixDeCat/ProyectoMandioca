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
    [SerializeField] AudioClip _hitTheGround;
    Transform root;

    void Start()
    {
        AudioManager.instance.GetSoundPool(_getDamageClip.name, AudioGroups.GAME_FX, _getDamageClip);
        AudioManager.instance.GetSoundPool(_throwAttackClip.name, AudioGroups.GAME_FX, _throwAttackClip);
        AudioManager.instance.GetSoundPool(_beginFightClip.name, AudioGroups.GAME_FX, _beginFightClip);
        AudioManager.instance.GetSoundPool(_deathClip.name, AudioGroups.GAME_FX, _deathClip);
        AudioManager.instance.GetSoundPool(_hitTheGround.name, AudioGroups.GAME_FX, _hitTheGround);
    }
    public void SetRoot(Transform root) => this.root = root;
    public void GetDamageClip() => AudioManager.instance.PlaySound(_getDamageClip.name, root);
    public void ThrowAttackClip() => AudioManager.instance.PlaySound(_throwAttackClip.name, root);
    public void BeginFightClip() => AudioManager.instance.PlaySound(_beginFightClip.name, root);
    public void HitTheGround() => AudioManager.instance.PlaySound(_hitTheGround.name, root);
    public void DeathClip() => AudioManager.instance.PlaySound(_deathClip.name, root);
}
