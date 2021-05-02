using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_Dispatcher : MonoBehaviour
{
    [SerializeField] TutorialSettings settings = null;
    bool alreadyDelay;

    public void DispatchTutorial()
    {
        if (settings == null || alreadyDelay) return;
        Tutorial_UIController.instance.SetNewTutorial(settings);
        enabled = false;
        settings = null;
    }

    public void DispatchTutorialWithDelay(float delay)
    {
        if (settings == null || alreadyDelay) return;

        alreadyDelay = true;
        StartCoroutine(DelayTutorial(delay));
    }

    IEnumerator DelayTutorial(float delay)
    {
        yield return new WaitForSeconds(delay);
        Tutorial_UIController.instance.SetNewTutorial(settings);
        enabled = false;
        settings = null;
    }
}
