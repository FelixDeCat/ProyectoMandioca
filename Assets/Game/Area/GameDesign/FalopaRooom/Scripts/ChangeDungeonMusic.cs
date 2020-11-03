using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeDungeonMusic : MonoBehaviour
{
    [SerializeField]AudioClip dungeonMusic;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void changeTheMusic()
    {
        ChangeTheMusic.instance.ChangeTheMusicAdventure(dungeonMusic);
    }
}
