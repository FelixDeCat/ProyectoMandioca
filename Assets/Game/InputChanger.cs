using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputChanger : MonoBehaviour
{
    public SpriteRenderer sprt;
    public Image img;
    public InputImageDatabase.InputImageType type;

    public void Start()
    {
        Main.instance.eventManager.SubscribeToEvent(GameEvents.CHANGE_INPUT, OnChange);

        if (img) img.sprite = InputImageDatabase.instance.GetSprite(type);
        if (sprt) sprt.sprite = InputImageDatabase.instance.GetSprite(type);
    }

    public void OnChange(params object[] p)
    {
        string val = (string)p[0];
        if (img) img.sprite = InputImageDatabase.instance.GetSprite(type);
        if (sprt) sprt.sprite = InputImageDatabase.instance.GetSprite(type);
    }
}
