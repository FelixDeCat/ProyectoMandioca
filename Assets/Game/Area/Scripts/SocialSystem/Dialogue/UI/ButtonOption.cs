using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

public class ButtonOption : MonoBehaviour
{
    [SerializeField] public Button button;
    Action<int> callbackSendInput;
    [SerializeField] TextMeshProUGUI txt_number;
    [SerializeField] TextMeshProUGUI txt_option;

    int index;
    public int Index { set => index = value; }

    public void Select()
    {
        button.Select();
    }

    public void SetInteractable(bool val)
    {
        button.interactable = val;
    }

    public void SetIndex(int index, string _string_option)
    {
        this.index = index;
        txt_number.text = (index + 1).ToString();
        txt_option.text = _string_option;
    }
    public void ConfigureCallbacks(Action<int> _callback_send_input)
    {
        button.onClick.AddListener(SendIndex);
        callbackSendInput = _callback_send_input;
    }

    void SendIndex()
    {
        callbackSendInput(index);
    }
}
