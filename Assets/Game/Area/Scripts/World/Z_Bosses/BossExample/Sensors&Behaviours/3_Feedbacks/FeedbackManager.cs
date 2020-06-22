using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedbackManager : MonoBehaviour
{
    [SerializeField] FeedbackOneShot FeedbackOnHit;
    [SerializeField] FeedbackActivator feedbackInHand;
    [SerializeField] FeedbackOneShot FeedbackPluckRock;
    [SerializeField] FeedbackFlashMaterial FeedbackFlashHitEmission;
    [SerializeField] FeedbackSoundDataBase FeedbackSoundDataBase;

    public void Play_FeedbackOnHit() => FeedbackOnHit.PlayFeedback();
    public void Play_PluckRock() => FeedbackPluckRock.PlayFeedback();
    public void Play_OnHitFlashEmission() => FeedbackFlashHitEmission.PlayFeedback();

    public void Play_SonidoDeEjemplo() => FeedbackSoundDataBase.PlayEjemplo();


    public void EnableRockInHand() => feedbackInHand.Activate(true);
    public void DisableRockInHand() => feedbackInHand.Activate(false);
}
