using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UI_Skill : UI_ItemBase
{
    SkillInfo skillinfo;
    GenericBar genbar;
    [SerializeField] Image backgroundImage = null;

    protected override void Awake()
    {

    }

    public void SetImages(Sprite bkg, Sprite main)
    {
        mainImage.sprite = main;
        backgroundImage.sprite = bkg;
    }

    public void Cooldown_ConfigureTime(float _cooldown)
    {

        genbar = GetComponentInChildren<GenericBar>();
        genbar.Configure(_cooldown, 0.01f);
    }
    public void Cooldown_SetValueTime(float _currentValue)
    {
        genbar.SetValue(_currentValue);
    }

    protected override void BeginFeedback() => SetImage(skillinfo.img_actived);
    protected override void EndFeedback() => SetImage(skillinfo.img_avaliable);

    public void Refresh() => SetImage(skillinfo.img_avaliable);

    public void Set_SkillInfo(SkillInfo s) => skillinfo = s;
    public SkillInfo Get_SkilInfo() => skillinfo;
}
