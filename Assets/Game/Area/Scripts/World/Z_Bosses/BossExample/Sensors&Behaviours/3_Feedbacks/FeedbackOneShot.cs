using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedbackOneShot : FeedbackBase
{
    public ParticleSystem[] parts;

    protected override void OnPlayFeedback()
    {
        for (int i = 0; i < parts.Length; i++)
        {
            parts[i].Play();
        }
    }
}
