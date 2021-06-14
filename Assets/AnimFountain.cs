using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimFountain : MonoBehaviour
{
    [SerializeField] AudioClip slime_caca;
    const string SLIME_CACA = "SlimeCaca";
    [SerializeField] ParticleSystem explodeCaca;
    private void Start()
    {
        AudioManager.instance.GetSoundPool(SLIME_CACA, AudioManager.SoundDimesion.TwoD, AudioGroups.GAME_FX, slime_caca);
    }
    public void ANIMATION_EVENT_SLIME_CACA()
    {
        AudioManager.instance.PlaySound(SLIME_CACA);
        explodeCaca.Play();
    }
}
