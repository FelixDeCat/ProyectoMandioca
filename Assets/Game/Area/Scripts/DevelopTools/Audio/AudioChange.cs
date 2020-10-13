using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioChange : MonoBehaviour
{
    public AudioClip clip_to_change;

    public void UE_ChangeClip()
    {
        AudioAmbienceSwitcher.instance.SwitchClip(clip_to_change);
    }
}
