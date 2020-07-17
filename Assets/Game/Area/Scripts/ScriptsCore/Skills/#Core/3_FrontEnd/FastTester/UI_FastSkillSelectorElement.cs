using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class UI_FastSkillSelectorElement : MonoBehaviour
{
    [Header("Info")]
    [SerializeField] Image img = null;
    [SerializeField] Text txt_info = null;
    [SerializeField] Text txt_name = null;
    int index;
    Action<int, bool> callbackSelection = delegate { };
    bool selected;
    [Header("feedback")]
    [SerializeField] Image imgSelected = null;
    Func<Sprite> GetOnImage;
    Func<Sprite> GetOffImage;
    public void SetInfo(string _name, string info, Sprite sprt, int _index, Action<int, bool> _callbackSelection, Func<Sprite> GetOnImage, Func<Sprite> GetOffImage)
    {
        this.txt_name.text = _name;
        this.txt_info.text = info;
        img.sprite = sprt;
        index = _index;
        callbackSelection = _callbackSelection;
        this.GetOnImage = GetOnImage;
        this.GetOffImage = GetOffImage;
        imgSelected.sprite = !selected ? GetOffImage() : GetOnImage();
    }
    public void BTN_Select() {
        if (!selected) { selected = true; SetSelectedFeedback(); callbackSelection.Invoke(index, true); }
        else { selected = false; SetUnSelectedFeedback(); callbackSelection.Invoke(index, false); }
    }
    void SetSelectedFeedback() => imgSelected.sprite = GetOnImage();
    void SetUnSelectedFeedback() => imgSelected.sprite = GetOffImage();
}
