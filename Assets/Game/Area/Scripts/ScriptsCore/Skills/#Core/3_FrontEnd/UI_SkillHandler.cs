using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools.Extensions;
using UnityEngine.UI;
using System;

public class UI_SkillHandler : UI_Base
{
    Dictionary<SkillType, UI_SkillContainer> positions = new Dictionary<SkillType, UI_SkillContainer>();

    [Header("UI_SkillHandler")]
    public GameObject parentcontainers;
    public GameObject model_parent_containers;
    public GameObject model_skill;

    public Text name_skill;
    public Text desc_lore_skill;
    public Text desc_tec_skill;
    public Image img;

    UI_Skill first;

    protected override void OnAwake() 
    {

    }

    public void SetInfoSelected(SkillInfo info)
    {
        name_skill.text = info.name;
        desc_lore_skill.text = info.description_lore;
        desc_tec_skill.text = info.description_technical;
        img.sprite = info.img_actived;
    }

    bool oneshot = false;
    public void Build(List<SkillBase> skills, Action<int> callback_OnSelected)
    {
        int index = 0;
        foreach (var s in skills)
        {
            
            var type = s.skillinfo.skilltype;

            if (!positions.ContainsKey(s.skillinfo.skilltype))
            {
                var container = parentcontainers.CreateDefaultSubObject<UI_SkillContainer>("Parent::Container_"+ type, model_parent_containers);
                container.skilltype = type;
                positions.Add(type, container);
                positions[type].SubdivisionName.text = s.skillinfo.skilltype.ToString();


            }

            var parent = positions[type];
            var info = s.skillinfo;

            var ui = parent.parent.CreateDefaultSubObject<UI_Skill>("Skill::Container:_ " + info.skill_name, model_skill);
            ui.Initialize(index, callback_OnSelected);
            ui.Set_SkillInfo(info);
            ui.Refresh();
            s.SetUI(ui);
            ui.OnUI_Unselect();

            if (!oneshot)
            {
                oneshot = true;
                first = ui;
                ConfigurateFirst(ui.gameObject);
            }

            index++;
        }
    }

    public override void Refresh() { }
    
    protected override void OnEndCloseAnimation() 
    {
        Main.instance.GetMyEventSystem().DeselectGameObject();
        oneshot = false;
    }
    protected override void OnEndOpenAnimation()
    {
        first.Select();
    }
    protected override void OnStart() { }
    protected override void OnUpdate() { }
}
