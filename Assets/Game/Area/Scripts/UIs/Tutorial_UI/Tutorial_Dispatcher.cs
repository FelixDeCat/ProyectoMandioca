using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_Dispatcher : MonoBehaviour
{
    [SerializeField] TutorialSettings settings = null;
    bool alreadyActived;
    bool alreadyDelay;
    [SerializeField] AudioClip _feedBack = null;

    private void Start()
    {
        AudioManager.instance.GetSoundPool(_feedBack.name, AudioManager.SoundDimesion.TwoD, AudioGroups.GAME_FX, _feedBack);
    }
    public void DispatchTutorial()
    {
        if (settings == null || alreadyDelay || alreadyActived) return;

        if (PauseManager.Instance.inPauseHud) { PauseManager.Instance.ResumeHud();
            DispatchTutorialWithDelay(1);
            return;
        }
        Tutorial_UIController.instance.SetNewTutorial(settings);
        enabled = false;
        alreadyActived = true;
        //AudioManager.instance.PlaySound(_feedBack.name);
    }

    public void DispatchTutorialWithDelay(float delay)
    {
        if (settings == null || alreadyDelay ||alreadyActived) return;

        if (PauseManager.Instance.inPauseHud)
            PauseManager.Instance.ResumeHud();

            alreadyDelay = true;        
        StartCoroutine(DelayTutorial(delay));
        
    }

    IEnumerator DelayTutorial(float delay)
    {
        yield return new WaitForSeconds(delay);
        Tutorial_UIController.instance.SetNewTutorial(settings);
        enabled = false;
        alreadyActived = true;
        //AudioManager.instance.PlaySound(_feedBack.name);
    }

    public void CompleteTutorial()
    {
        if (settings == null) return;
        Tutorial_UIController.instance.CompleteTutorial(settings);
        settings = null;
    }
}
