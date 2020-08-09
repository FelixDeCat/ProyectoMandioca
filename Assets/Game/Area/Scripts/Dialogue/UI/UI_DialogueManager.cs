using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class UI_DialogueManager : UI_Base
{
    public TextMeshProUGUI dialogue;
    public GameObject[] buttonOptionsGos;
    public ButtonOption[] buttonOptions;

    public Button buttonNext;
    public Button buttonExit;

    public void Init_Configuration(Action OnNextCallback, Action OnExitCallback, Action<int> optionSelected)
    {
        buttonNext.gameObject.SetActive(true);
        buttonExit.gameObject.SetActive(true);

        buttonNext.onClick.AddListener(OnNextCallback.Invoke);
        buttonExit.onClick.AddListener(OnExitCallback.Invoke);
        for (int i = 0; i < buttonOptions.Length; i++)
        {
            buttonOptions[i].ConfigureCallbacks(optionSelected);
        }

        buttonNext.gameObject.SetActive(false);
        buttonExit.gameObject.SetActive(false);
    }

    public void SetDialogue(string s)
    {
        dialogue.text = s;
        ShutDownOptions();
    }

    public void SetOption(int index, string option)
    {
        buttonOptionsGos[index].gameObject.SetActive(true);
        buttonOptions[index].SetIndex(index, option);
    }

    public void TurnOn_ButtonNext(bool val) => buttonNext.gameObject.SetActive(val);
    public void TurnOn_ButtonExit(bool val) => buttonExit.gameObject.SetActive(val);
    void ShutDownOptions() { for (int i = 0; i < buttonOptionsGos.Length; i++) buttonOptionsGos[i].gameObject.SetActive(false); }


    #region en desuso
    public override void Refresh() { }
    protected override void OnAwake() { }
    protected override void OnEndCloseAnimation() { }
    protected override void OnEndOpenAnimation() { }
    protected override void OnStart() { }
    protected override void OnUpdate() { }
    #endregion
}
