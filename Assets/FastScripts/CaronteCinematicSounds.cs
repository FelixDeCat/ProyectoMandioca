using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaronteCinematicSounds : MonoBehaviour
{
    [Header("Start")]
    [SerializeField] ParticleSystem portalParticle = null;
    [SerializeField] ParticleSystem caronteEntryTrail = null;
    [SerializeField] ParticleSystem caronteEntryExplosion = null;
    [SerializeField] AudioClip portalSound = null;
    [SerializeField] AudioClip explosionSound = null;
    [SerializeField] AudioClip slashSound = null;
    [SerializeField] AudioClip screamSound = null;

    [Header("Dead")]
    [SerializeField] AudioClip openPortal = null;
    [SerializeField] AudioClip enterToPortal = null;

    private void Start()
    {
        AudioManager.instance.GetSoundPool(openPortal.name, AudioManager.SoundDimesion.ThreeD, AudioGroups.GAME_FX, openPortal);
        AudioManager.instance.GetSoundPool(enterToPortal.name, AudioManager.SoundDimesion.ThreeD, AudioGroups.GAME_FX, enterToPortal);
    }

    void DEAD_OpenPortal()
    {
        AudioManager.instance.PlaySound(openPortal.name, transform);
    }

    void DEAD_EnterToPortal()
    {
        AudioManager.instance.PlaySound(enterToPortal.name, transform);
    }

    void START_PlayPortalParticle() => portalParticle.Play();

    void START_PlayCaronteTrail() => caronteEntryTrail.Play();

    void START_StopCaronteTrail() => caronteEntryTrail.Stop();

    void START_PlayCaronteExplosion() => caronteEntryExplosion.Play();
}
