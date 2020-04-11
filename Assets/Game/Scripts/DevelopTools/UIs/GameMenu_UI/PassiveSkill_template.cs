﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PassiveSkill_template : Selectable, ISubmitHandler, ISelectHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    private SkillInfo _skill;

    [SerializeField] private GameObject selector;

    private Action<SkillInfo> OnSelectedSkill;
    
    public void Configure(SkillInfo skill, Action<SkillInfo> callBack)
    {
        _skill = skill;
        OnSelectedSkill += callBack;
        GetComponent<Image>().sprite = skill.img_actived;
    }

    public void OnSubmit(BaseEventData eventData)
    {
        OnSelectedSkill(_skill);
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        
    }

    public void OnSelect(BaseEventData eventData)
    {
        selector.SetActive(true);
        OnSelectedSkill(_skill);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        selector.SetActive(true);
        OnSelectedSkill(_skill);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        selector.SetActive(false);
    }
}
