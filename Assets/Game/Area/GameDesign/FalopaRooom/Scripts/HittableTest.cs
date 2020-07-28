using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HittableTest : MonoBehaviour
{
    [SerializeField] AudioClip onHitSound = null;

    public void Start()
    {
        AudioManager.instance.GetSoundPool(onHitSound.name, AudioGroups.AMBIENT_FX, onHitSound);
    }
    public void OnHit()
    {
        Debug.Log("Get Hitted");
        AudioManager.instance.PlaySound(onHitSound.name);
    }
}
