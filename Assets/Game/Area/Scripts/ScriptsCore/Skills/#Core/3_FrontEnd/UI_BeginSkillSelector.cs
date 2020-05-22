using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UI_BeginSkillSelector : MonoBehaviour
{
    public Button playButton;
    public Text selected;

    public Button first;

    public SkillType skilltype;

    Action<SkillType> callback_selection;

    public void Initialize(Action<SkillType> _cbk_selection)
    {
        callback_selection = _cbk_selection;
        playButton.interactable = false;
        Main.instance.GetMyEventSystem().Set_First(first.gameObject);
        first.Select();
    }

    public void Select_Culpa()
    {
        selected.text = "Culpa";
        skilltype = SkillType.culpa;
        playButton.interactable = true;

    }
    public void Select_Obligacion()
    {
        selected.text = "Obligacion";
        skilltype = SkillType.obligacion;
        playButton.interactable = true;
    }
    public void Select_Control()
    {
        selected.text = "Control";
        skilltype = SkillType.control;
        playButton.interactable = true;
    }

    public void Play()
    {
        Main.instance.GetMyEventSystem().DeselectGameObject();
        callback_selection(skilltype);
    }
}
