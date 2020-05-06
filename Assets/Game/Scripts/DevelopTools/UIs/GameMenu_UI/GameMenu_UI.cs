using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class GameMenu_UI : UI_Base
{
    [Header("GameMenu_UI")]
    private SkillManager_Pasivas skill_manager;

    Image imgexample;

    [SerializeField] private RectTransform passiveSkills_container = null;
    [SerializeField] private RectTransform passiveSkillsSelection_container = null;
    [SerializeField] private PassiveSkill_template ps_template_pf = null;
    [SerializeField] private LvlUpSkillSelection_UI psSelection_template_pf = null;
    [SerializeField] private Text skillDescription_txt = null;
    Dictionary<SkillInfo, PassiveSkill_template> templates = new Dictionary<SkillInfo, PassiveSkill_template>();

    private Action OnFinishLvlUpSkillSelection;
    
    void RegistryButtons() { /*Registro las acciones de los botones del menu*/ }
    void UpdateSkillDescription(SkillInfo skill) => skillDescription_txt.text = skill.description_technical;

    //Lo estoy usando para apagar el sign de que tenes skills para aprender. Seguro se puede hacer con el Base_UI y su refresh
    public void Configure(Action callback)
    {
        OnFinishLvlUpSkillSelection = callback;
    }
    
    public override void Refresh()
    {
        GameObject myselected = new GameObject();

        bool first = false;
        
        var infos = Main.instance.GetPasivesManager().equiped.Select(x => x.skillinfo);

        foreach (var info in infos)
        {
            if (!templates.ContainsKey(info))
            {
                PassiveSkill_template newSkill = Instantiate(ps_template_pf, passiveSkills_container);
                newSkill.Configure(info, UpdateSkillDescription);
                templates.Add(info, newSkill);
            }
            if (!first)
            {
                first = true;

                myselected = templates[info].gameObject;
                
            }
        }

        skill_manager = Main.instance.GetPasivesManager();

        if (skill_manager.I_Have_An_Active_Request())
        {
            Debug.LogWarning("TENGO UNA REQUEST ACTIVA");
           Instantiate(psSelection_template_pf, passiveSkillsSelection_container).
                        Configure(skill_manager.GetPeekedRequest(), skill_manager.EquipSkill, OnFinishLvlUpSkillSelection, out myselected);
        }


        ConfigurateFirst(myselected);
    }

    #region UI_base Methods
    protected override void OnAwake() { }
    protected override void OnStart() { }
    protected override void OnEndOpenAnimation() { }
    protected override void OnEndCloseAnimation() { }
    protected override void OnUpdate() { }
    #endregion
}
