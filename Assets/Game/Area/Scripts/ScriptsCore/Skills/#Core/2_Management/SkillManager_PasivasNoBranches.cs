using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

public class SkillManager_PasivasNoBranches : LoadComponent
{

    [Header("Data_base")]
    [SerializeField] List<SkillBase> skills = new List<SkillBase>();
    
    public UI_FastSkillSelector frontEnd;

    float prog;
    protected override IEnumerator LoadMe()
    {
        skills = GetComponentsInChildren<SkillBase>().ToList();
        frontEnd.Build(skills, Equip);
        yield return null;
    }

    bool open; public void Open() { open = !open; if (open) frontEnd.Open(); else frontEnd.Close(); }

    List<SkillInfo> coll_for_Feedback = new List<SkillInfo>();
    public void Equip(int index, bool active)
    {
        if (active) 
        { 
            if (!coll_for_Feedback.Contains(skills[index].skillinfo))
            {
                skills[index].BeginSkill();
                coll_for_Feedback.Add(skills[index].skillinfo);
            }
        }
        else 
        {
            if (coll_for_Feedback.Contains(skills[index].skillinfo))
            {
                skills[index].EndSkill();
                coll_for_Feedback.Remove(skills[index].skillinfo);
            }
        }

        Main.instance.gameUiController.RefreshPassiveSkills_UI(coll_for_Feedback);
    }

    
}
