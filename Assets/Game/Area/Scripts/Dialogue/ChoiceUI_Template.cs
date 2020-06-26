using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceUI_Template : MonoBehaviour
{
    [SerializeField] private Text text;
    private int destinationID;
    
    Action pressedButton;

    private void Start()
    {
        text = GetComponentInChildren<Text>();
        GetComponent<Button>().onClick.AddListener(OnPressedButton);
        
    }

    /// <summary>
    /// Para el continue dentro del nodo
    /// </summary>
    /// <param name="text"></param>
    public void Configure(string text,Action callback)
    {
        this.text.text = text;
        pressedButton = callback;
    }

    void OnPressedButton()
    {
        pressedButton.Invoke();
        Destroy(gameObject);
    }
}
