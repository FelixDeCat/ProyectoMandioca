using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public abstract class Interactable : MonoBehaviour
{
    [Header("Interactable Settings")]
    public float distancetoInteract = 1f;
    public bool autoexecute;
    public Transform pointToMessage;
    public FeedbackInteractBase[] feedback;
    public void Enter(WalkingEntity entity)
    {
        if (!autoexecute) if (feedback.Length > 0) foreach (var fdbck in feedback) fdbck.Show();
        OnEnter(entity);
    }
    public void Exit()
    {
        if (feedback.Length > 0) foreach (var fdbck in feedback) fdbck.Hide();
        OnExit();
    }
    public void Execute(WalkingEntity entity)
    {
        OnExecute(entity);
    }
    public abstract void OnEnter(WalkingEntity entity);
    public abstract void OnExecute(WalkingEntity collector);
    public abstract void OnExit();

}
