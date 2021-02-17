using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeDungeonMusic : MonoBehaviour
{
    [SerializeField]AudioClip dungeonMusic = null;
    public void changeTheMusic()
    {
        ChangeTheMusic.instance.ChangeTheMusicAdventure(dungeonMusic);
    }
}
