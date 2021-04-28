using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Feedback_Maniqui
{
    public AudioClip clip_hitManiqui;
    public AudioClip clip_death;
    public ParticleSystem part_hit;
    public ParticleSystem part_death;
    public Renderer meshRenderer;
    Transform myRoot;
    public void Initialize(Transform myRoot)
    {
        this.myRoot = myRoot;
        if (clip_hitManiqui) AudioManager.instance.GetSoundPool(clip_hitManiqui.name, AudioManager.SoundDimesion.ThreeD, AudioGroups.GAME_FX, clip_hitManiqui);
        if (clip_hitManiqui) AudioManager.instance.GetSoundPool(clip_death.name, AudioManager.SoundDimesion.ThreeD, AudioGroups.GAME_FX, clip_death);
    }
    public void Play_part_Hit() => part_hit?.Play();
    public void Play_part_Death() => part_death?.Play();
    public void Play_Sound_Hit() => AudioManager.instance.PlaySound(clip_hitManiqui.name, myRoot);
    public void Play_Sound_Death() => AudioManager.instance.PlaySound(clip_death.name, myRoot);

    public IEnumerator OnHitted(float onHitFlashTime, Color onHitColor)
    {
        if (meshRenderer != null)
        {
            var myMat = meshRenderer.materials;
            Material[] mats = meshRenderer.materials;
            meshRenderer.materials = mats;
            for (int i = 0; i < onHitFlashTime; i++)
            {
                if (i < (onHitFlashTime / 2f))
                {
                    mats[0].SetColor("_EmissionColor", Color.Lerp(Color.black, onHitColor, i / (onHitFlashTime / 2f)));
                }
                else
                {
                    mats[0].SetColor("_EmissionColor", Color.Lerp(onHitColor, Color.black, (i - (onHitFlashTime / 2f)) / (onHitFlashTime / 2f)));
                }
                yield return new WaitForSeconds(0.01f);
            }
        }
    }
}
