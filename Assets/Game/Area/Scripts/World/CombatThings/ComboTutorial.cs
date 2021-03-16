using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboTutorial : MonoBehaviour
{
    bool tutorialOn;
    [SerializeField] NPC_Dialog dialog = null;

    private void Start()
    {
        Main.instance.GetChar().AddCallbackToReadyCombo(PauseAll);
    }

    private void Update()
    {
        if (tutorialOn)
        {
            if (Input.GetButtonDown("Interact")) ResumeAll();
        }
    }

    void PauseAll()
    {
        PauseManager.Instance.Pause();
        tutorialOn = true;
        Debug.Log("estoy re ready vieja");
        Main.instance.gameUiController.ActiveOrDesactiveComboScreen(true);
        Main.instance.GetChar().AddCallbackToReadyCombo(PauseAll, false);
    }

    void ResumeAll()
    {
        PauseManager.Instance.Resume();
        Main.instance.gameUiController.ActiveOrDesactiveComboScreen(false);
        Main.instance.GetChar().UNITY_EVENT_OnInteractDown();
        Main.instance.eventManager.SubscribeToEvent(GameEvents.COMBAT_EXIT, DisplayDialogue);
    }

    void DisplayDialogue()
    {
        dialog.Talk();
        Main.instance.eventManager.UnsubscribeToEvent(GameEvents.COMBAT_EXIT, DisplayDialogue);
        PauseManager.Instance.Pause();
    }

    public void DestroyAfterDialogue()
    {
        PauseManager.Instance.Resume();
        Destroy(this.gameObject);
    }
}
