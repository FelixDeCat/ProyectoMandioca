using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BommerangShiledFeedbacks : MonoBehaviour
{
    [SerializeField] AudioClip ac_throw;
    [SerializeField] AudioClip ac_movement;
    [SerializeField] AudioClip ac_Impact;
    private void Start()
    {
        AudioManager.instance.GetSoundPool(ac_throw.name, AudioGroups.GAME_FX, ac_throw);
        AudioManager.instance.GetSoundPool(ac_movement.name, AudioGroups.GAME_FX, ac_movement);
        AudioManager.instance.GetSoundPool(ac_Impact.name, AudioGroups.GAME_FX, ac_Impact);

    }

    public void Play_ThrowActive()
    {
        AudioManager.instance.PlaySound(ac_throw.name, transform);
    }
    public void Play_Movement()
    {
        AudioManager.instance.PlaySound(ac_movement.name, transform);
    }

    public void Play_Impact()
    {
        AudioManager.instance.PlaySound(ac_Impact.name, transform);
    }
}
