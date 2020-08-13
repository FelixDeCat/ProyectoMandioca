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

    public TextAnimCharXChar animation;

    public RectTransform parent_to_force_rebuild;

    public GameObject photo_go;
    public Image photo;

    public Button buttonNext;
    public Button buttonExit;

    public ReconfigureNavigateButtons reconfigure;

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

    public void SetPhoto(Sprite sp)
    {
        photo_go.SetActive(true);
        photo.sprite = sp;
    }

    public void NoUsePhoto()
    {
        photo_go.SetActive(false);
    }

    public void SetDialogue(string s, Action OnFinishCarret, bool force = false)
    {
        ShutDownOptions();

        if (force)
        {
            animation.Force(s);
            OnFinishCarret.Invoke();
        }
        else
        {
            animation.BeginAnim(s, OnFinishCarret);
        }
    }


    public void SetOption(int index, string option)
    {
        if (index == 0)
        {
            buttonOptions[0].Select();
            ForceDirectConfigurateFirst(buttonOptions[0].gameObject);
        }
        buttonOptionsGos[index].gameObject.SetActive(true);
        buttonOptions[index].SetIndex(index, option);

        reconfigure.Reconfigure();

        Repaint();

    }

    void Repaint()
    {
        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(parent_to_force_rebuild);
    }

    public void TurnOn_ButtonNext(bool val)
    {
        buttonNext.gameObject.SetActive(val);
        buttonNext.Select();
        reconfigure.Reconfigure();
        ForceDirectConfigurateFirst(buttonNext.gameObject);
        Repaint();
    }
    public void TurnOn_ButtonExit(bool val)
    {
        buttonExit.gameObject.SetActive(val);
        buttonExit.Select();
        reconfigure.Reconfigure();
        ForceDirectConfigurateFirst(buttonExit.gameObject);
        Repaint();
    }
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
