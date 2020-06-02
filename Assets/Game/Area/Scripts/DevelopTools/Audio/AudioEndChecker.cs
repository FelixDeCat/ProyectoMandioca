using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioEndChecker
{
    Action endCallback;
    bool check;
    AudioSource audioSource;
    public void CheckIfEnded(Action _endCallback, AudioSource _as)
    {
        endCallback = _endCallback;
        audioSource = _as;
        check = true;
    }

    public void Refresh()
    {
        if (check)
        {
            if (!audioSource.isPlaying)
            {
                check = false;
                endCallback.Invoke();
            }
        }
    }
}
