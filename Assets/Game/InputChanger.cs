using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputChanger : MonoBehaviour
{
    public Sprite sprt;
    public Image img;
    public InputImageDatabase.InputImageType type;

    public void Start()
    {
        Main.instance.eventManager.SubscribeToEvent(GameEvents.CHANGE_INPUT, OnChange);
    }

    public void OnChange(params object[] p)
    {
        string val = (string)p[0];
        if(img) img.sprite = InputImageDatabase.instance.GetSprite(type);
    }
}
