using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedbackManager : MonoBehaviour
{
    [SerializeField] FeedbackOneShot FeedbackOnHit;
    [SerializeField] FeebackEmission feedbackEmission;

    public void Play_FeedbackOnHit()
    {
        FeedbackOnHit.PlayFeedback();
    }
    public void Play_HitDamageEmission()
    {
        feedbackEmission.PlayFeedback();
    }
}
