using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Sound_Lerp : MonoBehaviour
{
    public List<AudioMixerSnapshot> snapShotsList = new List<AudioMixerSnapshot>();
  

    public void TransitionBetweenSnapshots(int index, float speed)
    {
        snapShotsList[index].TransitionTo(speed);
    }
   
    public void TransitionToOriginal()
    {
        snapShotsList[0].TransitionTo(0.5f);
    }
}
