using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaronteCinematicSounds : MonoBehaviour
{
    [Header("Start")]
    [SerializeField] ParticleSystem portalParticle = null;
    [SerializeField] ParticleSystem caronteEntryExplosion = null;
    [SerializeField] AudioClip portalSound = null;
    [SerializeField] AudioClip explosionSound = null;
    [SerializeField] AudioClip thunderSound = null;
    [SerializeField] AudioClip slashSound = null;
    [SerializeField] AudioClip screamSound = null;
    [SerializeField] AudioClip earthquakeSound = null;

    [Header("Dead")]
    [SerializeField] AudioClip openPortal = null;
    [SerializeField] AudioClip enterToPortal = null;

    private void Start()
    {
        AudioManager.instance.GetSoundPool(openPortal.name, AudioManager.SoundDimesion.ThreeD, AudioGroups.GAME_FX, openPortal);
        AudioManager.instance.GetSoundPool(enterToPortal.name, AudioManager.SoundDimesion.ThreeD, AudioGroups.GAME_FX, enterToPortal);
        AudioManager.instance.GetSoundPool(portalSound.name, AudioManager.SoundDimesion.ThreeD, AudioGroups.GAME_FX, portalSound);
        AudioManager.instance.GetSoundPool(thunderSound.name, AudioManager.SoundDimesion.ThreeD, AudioGroups.GAME_FX, thunderSound);
        AudioManager.instance.GetSoundPool(explosionSound.name, AudioManager.SoundDimesion.ThreeD, AudioGroups.GAME_FX, explosionSound);
        AudioManager.instance.GetSoundPool(slashSound.name, AudioManager.SoundDimesion.ThreeD, AudioGroups.GAME_FX, slashSound);
        AudioManager.instance.GetSoundPool(screamSound.name, AudioManager.SoundDimesion.ThreeD, AudioGroups.GAME_FX, screamSound);
        AudioManager.instance.GetSoundPool(earthquakeSound.name, AudioManager.SoundDimesion.ThreeD, AudioGroups.GAME_FX, earthquakeSound);
         transform.localPosition = Vector3.up *2000;
    }

    public void StartCinematic()
    {
        transform.localPosition = Vector3.zero;
        GetComponent<Animator>().Play("StartCinematic");
        Debug.Log("???!?!");
    }

    void DEAD_OpenPortal()
    {
        AudioManager.instance.PlaySound(openPortal.name, transform);
    }

    void DEAD_EnterToPortal()
    {
        AudioManager.instance.PlaySound(enterToPortal.name, transform);
    }

    void START_PlayPortalParticle()
    {
        portalParticle.Play();
        AudioManager.instance.PlaySound(portalSound.name, transform);
    }

    void START_PlayCaronteExplosion()
    {
        caronteEntryExplosion.Play();
        AudioManager.instance.PlaySound(thunderSound.name, transform);
        AudioManager.instance.PlaySound(explosionSound.name, transform);
    }

    void START_SlashSound()
    {
        AudioManager.instance.PlaySound(slashSound.name, transform);
    }

    void START_ScreamSound()
    {
        AudioManager.instance.PlaySound(screamSound.name, transform);
    }

    public void START_EarthquakeSound()
    {
        AudioManager.instance.PlaySound(earthquakeSound.name, transform.parent);
    }
    public void START_StopEarthquakeSound()
    {
        AudioManager.instance.StopAllSounds(earthquakeSound.name);
    }
}
