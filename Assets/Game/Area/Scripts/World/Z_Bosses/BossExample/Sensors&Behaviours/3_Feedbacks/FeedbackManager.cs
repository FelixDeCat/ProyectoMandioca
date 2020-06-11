using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedbackManager : MonoBehaviour
{
    [SerializeField] FeedbackOneShot FeedbackOnHit;
    [SerializeField] FeebackEmission feedbackEmission;
    [SerializeField] FeedbackActivator feedbackInHand;
    [SerializeField] FeedbackOneShot FeedbackPluckRock;

    public void Play_FeedbackOnHit() => FeedbackOnHit.PlayFeedback();
    public void Play_HitDamageEmission() => feedbackEmission.PlayFeedback();
    public void Play_PluckRock() => FeedbackPluckRock.PlayFeedback();

    public void EnableRockInHand() => feedbackInHand.Activate(true);
    public void DisableRockInHand() => feedbackInHand.Activate(false);
}
