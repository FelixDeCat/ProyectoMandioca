using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleAudios : MonoBehaviour
{
    [SerializeField] AudioClip _clip = null;
    [SerializeField] string _nameOfClip = null;
    [SerializeField] Transform myTransform = null;

    void Start()
    {
        AudioManager.instance.GetSoundPool(_nameOfClip, AudioGroups.GAME_FX, _clip);
        AudioManager.instance.PlaySound(_nameOfClip, myTransform);
    }
}
