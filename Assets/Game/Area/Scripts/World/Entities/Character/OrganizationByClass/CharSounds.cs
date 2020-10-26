﻿using UnityEngine;

[System.Serializable]
public class CharSounds
{
    [SerializeField] AudioClip takeHeal = null;
    [SerializeField] AudioClip swingSword = null;
    [SerializeField] AudioClip footstep = null;
    [SerializeField] AudioClip dashSounds = null;
    [SerializeField] AudioClip take_normal_damage = null;
    [SerializeField] AudioClip take_big_damage = null;
    [SerializeField] AudioClip parry = null;
    [SerializeField] AudioClip block = null;
    [SerializeField] AudioClip dashGemido=null;
    [SerializeField] AudioClip dashBashHit = null;
    [SerializeField] AudioClip heavySwing = null;
    [SerializeField] AudioClip offFightMusic = null;
    [SerializeField] AudioClip onFightMusic = null;
    [SerializeField] public int lerpSpeed;
    [SerializeField] public int index;
    public void Initialize()
    {
        AudioManager.instance.GetSoundPool(takeHeal.name,           AudioGroups.GAME_FX, takeHeal);
        AudioManager.instance.GetSoundPool(swingSword.name,         AudioGroups.GAME_FX, swingSword);
        AudioManager.instance.GetSoundPool(footstep.name,           AudioGroups.GAME_FX, footstep);
        AudioManager.instance.GetSoundPool(dashSounds.name,         AudioGroups.GAME_FX, dashSounds);
        AudioManager.instance.GetSoundPool(take_normal_damage.name, AudioGroups.GAME_FX, take_normal_damage);
        AudioManager.instance.GetSoundPool(take_big_damage.name,    AudioGroups.GAME_FX, take_big_damage);
        AudioManager.instance.GetSoundPool(parry.name,              AudioGroups.GAME_FX, parry);
        AudioManager.instance.GetSoundPool(block.name,              AudioGroups.GAME_FX, block);
        AudioManager.instance.GetSoundPool(dashGemido.name,              AudioGroups.GAME_FX, dashGemido);
        AudioManager.instance.GetSoundPool(dashBashHit.name, AudioGroups.GAME_FX, dashBashHit);
        AudioManager.instance.GetSoundPool(heavySwing.name, AudioGroups.GAME_FX, heavySwing);
        AudioManager.instance.GetSoundPool(offFightMusic.name, AudioGroups.OffFightMusic, offFightMusic, true);
        AudioManager.instance.GetSoundPool(onFightMusic.name, AudioGroups.OnFightMusic, onFightMusic, true);
        Play_OffFightMusic();
        Play_OnFightMusic();
    }
    public void Play_TakeHeal() =>          AudioManager.instance.PlaySound(takeHeal.name);
    public void Play_SwingSword() =>        AudioManager.instance.PlaySound(swingSword.name);
    public void Play_FootStep() =>          AudioManager.instance.PlaySound(footstep.name);
    public void Play_Dash()               { AudioManager.instance.PlaySound(dashSounds.name); AudioManager.instance.PlaySound(dashGemido.name); }
    public void Play_TakeNormalDamage() =>  AudioManager.instance.PlaySound(take_normal_damage.name);
    public void Play_TakeBigDamage() =>     AudioManager.instance.PlaySound(take_big_damage.name);
    public void Play_Parry() =>             AudioManager.instance.PlaySound(parry.name);
    public void Play_Block() =>             AudioManager.instance.PlaySound(block.name);
    public void Play_DashBashHit() =>       AudioManager.instance.PlaySound(dashBashHit.name);
    public void Play_heavySwing() =>        AudioManager.instance.PlaySound(heavySwing.name);
    public void Play_OnFightMusic() { AudioManager.instance.PlaySound(onFightMusic.name);  }
    public void Play_OffFightMusic() { AudioManager.instance.PlaySound(offFightMusic.name);  }
    public void NewOffFightMusic(AudioClip music)
    {
        offFightMusic = music;
        AudioManager.instance.GetSoundPool(offFightMusic.name, AudioGroups.GAME_FX, offFightMusic);
        Play_OffFightMusic();
    }
    public void NewOnFightMusic(AudioClip music)
    {
        onFightMusic = music;
        AudioManager.instance.GetSoundPool(onFightMusic.name, AudioGroups.GAME_FX, onFightMusic);
        Play_OnFightMusic();
    }

}
