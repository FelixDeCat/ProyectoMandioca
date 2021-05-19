using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_Dispatcher : MonoBehaviour
{
    [SerializeField] TutorialSettings settings = null;
    bool alreadyDelay;
    [SerializeField] AudioClip _feedBack;

    private void Start()
    {
        AudioManager.instance.GetSoundPool(_feedBack.name, AudioManager.SoundDimesion.TwoD, AudioGroups.GAME_FX, _feedBack);
    }
    public void DispatchTutorial()
    {
        if (settings == null || alreadyDelay) return;

        if (PauseManager.Instance.inPauseHud) { PauseManager.Instance.ResumeHud();
            DispatchTutorialWithDelay(1);
            return;
        }
        Tutorial_UIController.instance.SetNewTutorial(settings);
        enabled = false;
        settings = null;
        AudioManager.instance.PlaySound(_feedBack.name);
    }

    public void DispatchTutorialWithDelay(float delay)
    {
        if (settings == null || alreadyDelay) return;

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
        settings = null;
        AudioManager.instance.PlaySound(_feedBack.name);
    }
}
