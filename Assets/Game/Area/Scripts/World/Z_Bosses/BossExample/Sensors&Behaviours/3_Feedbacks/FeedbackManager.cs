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

    // public void Play_SonidoDeEjemplo() => FeedbackSoundDataBase.PlayEjemplo();

    public void Play_BegginFightClip() => FeedbackSoundDataBase.BeginFightClip();
    public void Play_GetDamage() => FeedbackSoundDataBase.GetDamageClip();
    public void Play_ThrowAttack() => FeedbackSoundDataBase.ThrowAttackClip();
    public void Play_DeathClip() => FeedbackSoundDataBase.DeathClip();
    public void EnableRockInHand() => feedbackInHand.Activate(true);
    public void DisableRockInHand() => feedbackInHand.Activate(false);
}
