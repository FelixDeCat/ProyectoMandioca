using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedbackSoundDataBase : MonoBehaviour
{
    [SerializeField] AudioClip _getDamageClip = null;
    [SerializeField] AudioClip _throwAttackClip = null;
    [SerializeField] AudioClip _beginFightClip = null;
    [SerializeField] AudioClip _deathClip = null;
    [SerializeField] AudioClip _hitTheGround = null;
    [SerializeField] AudioClip _walk = null;
    Transform root;

    void Start()
    {
        AudioManager.instance.GetSoundPool(_getDamageClip.name, AudioManager.SoundDimesion.ThreeD, AudioGroups.GAME_FX, _getDamageClip);
        AudioManager.instance.GetSoundPool(_throwAttackClip.name, AudioManager.SoundDimesion.ThreeD, AudioGroups.GAME_FX, _throwAttackClip);
        AudioManager.instance.GetSoundPool(_beginFightClip.name, AudioManager.SoundDimesion.ThreeD, AudioGroups.GAME_FX, _beginFightClip);
        AudioManager.instance.GetSoundPool(_deathClip.name, AudioManager.SoundDimesion.ThreeD, AudioGroups.GAME_FX, _deathClip);
        AudioManager.instance.GetSoundPool(_hitTheGround.name, AudioManager.SoundDimesion.ThreeD, AudioGroups.GAME_FX, _hitTheGround);
        AudioManager.instance.GetSoundPool(_walk.name, AudioManager.SoundDimesion.ThreeD, AudioGroups.GAME_FX, _walk);
    }
    public void SetRoot(Transform root) => this.root = root;
    public void GetDamageClip() => AudioManager.instance.PlaySound(_getDamageClip.name, root);
    public void ThrowAttackClip() => AudioManager.instance.PlaySound(_throwAttackClip.name, root);
    public void BeginFightClip() => AudioManager.instance.PlaySound(_beginFightClip.name, root);
    public void HitTheGround() => AudioManager.instance.PlaySound(_hitTheGround.name, root);
    public void DeathClip() => AudioManager.instance.PlaySound(_deathClip.name, root);
    public void WalkClip() => AudioManager.instance.PlaySound(_walk.name, root);
}
