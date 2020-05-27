using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UI_FastSkillSelector : UI_Base
{
    public GameObject model;
    public RectTransform parent_rect;
    public Sprite img_On; Sprite GetOnImage() => img_On;
    public Sprite img_Off; Sprite GetOffImage() => img_Off;
    public List<UI_FastSkillSelectorElement> elements = new List<UI_FastSkillSelectorElement>();

    public void Build(List<SkillBase> collection, Action<int, bool> _callbackSelection)
    {
        parent.SetActive(true);
        for (int i = 0; i < collection.Count; i++)
        {
            GameObject go = Instantiate(model, parent_rect);
            var elem = go.GetComponent<UI_FastSkillSelectorElement>();
            elem.SetInfo(
                collection[i].skillinfo.skill_name,
                collection[i].skillinfo.description_technical,
                collection[i].skillinfo.img_actived,
                i,
                _callbackSelection,
                GetOnImage,
                GetOffImage);

            elements.Add(elem);
        }
        parent.SetActive(false);
    }

    protected override void OnAwake() { }
    protected override void OnStart() { }
    protected override void OnEndOpenAnimation() { }
    protected override void OnEndCloseAnimation() { }
    protected override void OnUpdate() { }
    public override void Refresh() { }
}
