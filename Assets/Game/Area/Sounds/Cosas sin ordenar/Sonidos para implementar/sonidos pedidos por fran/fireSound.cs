using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireSound : MonoBehaviour
{
    [SerializeField] AudioClip _soundEffect;
    // Start is called before the first frame update
    void Start()
    {
        AudioManager.instance.GetSoundPool(_soundEffect.name, AudioManager.SoundDimesion.ThreeD, AudioGroups.GAME_FX, _soundEffect, true);
        AudioManager.instance.PlaySound(_soundEffect.name,transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
