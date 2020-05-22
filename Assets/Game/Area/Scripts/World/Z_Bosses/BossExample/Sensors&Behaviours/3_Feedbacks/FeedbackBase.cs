using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FeedbackBase : MonoBehaviour
{
    public void PlayFeedback() => OnPlayFeedback();
    protected abstract void OnPlayFeedback();
}
