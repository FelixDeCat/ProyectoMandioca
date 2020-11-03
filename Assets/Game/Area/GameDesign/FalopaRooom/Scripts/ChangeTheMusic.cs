using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeTheMusic : MonoBehaviour
{
    CharacterHead player;
    public static ChangeTheMusic instance;
    // Start is called before the first frame update
    void Start()
    {
        if (instance == null) instance = this;
        player = Main.instance.GetChar();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeTheMusicAdventure(AudioClip adventureMusic)
    {
        player.feedbacks.sounds.NewOffFightMusic(adventureMusic);
    }

    public void ChangeTheMusicFight(AudioClip FightMusic)
    {
        player.feedbacks.sounds.NewOnFightMusic(FightMusic);
    }
}
