using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedbackManager : MonoBehaviour
{
    [SerializeField] FeedbackOneShot FeedbackOnHit = null;
    [SerializeField] FeedbackActivator feedbackInHand = null;
    [SerializeField] FeedbackOneShot FeedbackPluckRock = null;
    [SerializeField] FeedbackFlashMaterial FeedbackFlashHitEmission = null;
    [SerializeField] FeedbackSoundDataBase FeedbackSoundDataBase = null;

    public void SetRoot(Transform _root)
    {
        FeedbackSoundDataBase.SetRoot(_root);
    }

    public void Play_FeedbackOnHit() => FeedbackOnHit.PlayFeedback();
    public void Play_PluckRock() => FeedbackPluckRock.PlayFeedback();
    public void Play_OnHitFlashEmission() => FeedbackFlashHitEmission.PlayFeedback();

    // public void Play_SonidoDeEjemplo() => FeedbackSoundDataBase.PlayEjemplo();

    public void Play_BegginFightClip() => FeedbackSoundDataBase.BeginFightClip();
    public void Play_GetDamage() => FeedbackSoundDataBase.GetDamageClip();
    public void Play_ThrowAttack() => FeedbackSoundDataBase.ThrowAttackClip();
    public void Play_DeathClip() => FeedbackSoundDataBase.DeathClip();
    public void Play_HitTheGround() => FeedbackSoundDataBase.HitTheGround();
    public void EnableRockInHand() => feedbackInHand.Activate(true);
    public void DisableRockInHand() => feedbackInHand.Activate(false);
}
