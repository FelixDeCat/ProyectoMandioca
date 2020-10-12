using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Feedbacks_Craft : MonoBehaviour
{

    [SerializeField] AudioClip beginCraft;
    [SerializeField] AudioClip endCraft;


    public void Start()
    {
        AudioManager.instance.GetSoundPool(beginCraft.name, AudioGroups.GAME_FX, beginCraft);
        AudioManager.instance.GetSoundPool(endCraft.name, AudioGroups.GAME_FX, endCraft);
    }

    public void Play_Sound_BeginCraft()
    {
        AudioManager.instance.PlaySound(beginCraft.name, transform);
    }
    public void Play_Sound_EndCraft()
    {
        AudioManager.instance.PlaySound(endCraft.name, transform);
    }
}
