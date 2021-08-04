using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetoCinematicSounds : MonoBehaviour
{
    [Header("Start")]
    [SerializeField] ParticleSystem flyParticles = null;
    [SerializeField] Animator[] fonts = new Animator[0];
    [SerializeField] Renderer mainFont = null;
    [SerializeField] float timeToCorruptWater = 2;
    [SerializeField] AudioClip statueOn = null;
    [SerializeField] AudioClip screamSound = null;
    [SerializeField] AudioClip earthquakeSound = null;
    [SerializeField] string bossName = "Deimos";
    [SerializeField] string bossDesc = "Dios Corrupto";
    [SerializeField] Renderer[] statueEyes = new Renderer[0];

    [Header("Dead")]
    [SerializeField] AudioClip tentaclesSound = null;
    [SerializeField] AudioClip colorOffSound = null;

    private void Start()
    {
        AudioManager.instance.GetSoundPool(statueOn.name, AudioManager.SoundDimesion.ThreeD, AudioGroups.GAME_FX, statueOn);
        //AudioManager.instance.GetSoundPool(tentaclesSound.name, AudioManager.SoundDimesion.ThreeD, AudioGroups.GAME_FX, tentaclesSound);
        AudioManager.instance.GetSoundPool(colorOffSound.name, AudioManager.SoundDimesion.ThreeD, AudioGroups.GAME_FX, colorOffSound);
        AudioManager.instance.GetSoundPool(screamSound.name, AudioManager.SoundDimesion.ThreeD, AudioGroups.GAME_FX, screamSound);
        AudioManager.instance.GetSoundPool(earthquakeSound.name, AudioManager.SoundDimesion.ThreeD, AudioGroups.GAME_FX, earthquakeSound);
        transform.localPosition = Vector3.up * 2000;
    }

    public void StartCinematic()
    {
        transform.localPosition = Vector3.zero;
        GetComponent<Animator>().Play("StartCinematic");
        Debug.Log("???!?!");
    }

    void END_TentacleSound()
    {
        AudioManager.instance.PlaySound(tentaclesSound.name, transform);
    }

    void END_ColorOff()
    {
        AudioManager.instance.PlaySound(colorOffSound.name, transform);
    }

    void END_ColorOffStop()
    {
        AudioManager.instance.StopAllSounds(colorOffSound.name);
    }

    void START_StatueOn()
    {
        AudioManager.instance.PlaySound(statueOn.name, transform);
        //for (int i = 0; i < statueEyes.Length; i++)
        //{
        //    statueEyes[i].material.SetColor("_TintColor", new Color32(229, 16, 16, 255));
        //    statueEyes[i].material.SetColor("_EmissionColor", new Color32(185, 0, 20, 255));
        //}

    }

    void START_StopFlyParticle()
    {
        flyParticles.Stop();
    }

    public void START_CorruptFonts()
    {
        StartCoroutine(CorruptFonts());
    }

    IEnumerator CorruptFonts()
    {
        for (int i = 0; i < fonts.Length; i++)
        {
            fonts[i].Play("Llego la kk");

            yield return new WaitForSeconds(0.5f);
        }
    }

    public void START_CorruptMainFont()
    {
        StartCoroutine(CorruptMainFont());
    }

    IEnumerator CorruptMainFont()
    {
        float timer = timeToCorruptWater;
        while (timer > 0)
        {
            timer -= Time.deltaTime;

            mainFont.material.SetFloat("_MainOpacity", Mathf.Lerp(0, 1, timer / timeToCorruptWater));
            yield return new WaitForEndOfFrame();
        }
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


    void ApppearBossName() => Main.instance.gameUiController.OpenBossName(bossName, bossDesc);
}
