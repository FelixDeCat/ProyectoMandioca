using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeTheMusic : MonoBehaviour
{
    public static ChangeTheMusic instance;

    void Start()
    {
        if (instance == null) instance = this;
    }

    public void ChangeTheMusicAdventure(AudioClip adventureMusic)
    {
        AudioAmbienceSwitcher.instance.ChangeMusic(adventureMusic);
    }

    public void ChangeTheMusicFight(AudioClip FightMusic)
    {
        AudioAmbienceSwitcher.instance.ChangeFightMusic(FightMusic);
    }
}
