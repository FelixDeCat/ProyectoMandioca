﻿using UnityEngine;
public abstract class SkillBase : MonoBehaviour
{
    bool canupdate = false;
    public SkillInfo skillinfo;
    protected UI_Skill ui_skill;
    protected UI3D_Element_SkillsActivas ui3D_skill;
    public void SetUI(UI_Skill _ui) => ui_skill = _ui;
    public void SetUI(UI3D_Element_SkillsActivas _ui) => ui3D_skill = _ui;
    public void RemoveUI() => ui3D_skill = null;
    public UI_Skill GetUI() => ui_skill;
    bool alreadyActived;
    public bool is3D;

    [Header("Feedback")] [SerializeField] private AudioClip skillInCD = null;
    public void Start()//LoadMe
    {
        OnStart();
        AudioManager.instance.GetSoundPool("skillIncooldown", AudioGroups.GAME_FX, skillInCD);
    }
    public virtual void BeginSkill()
    {
        if (!alreadyActived)
        {
            alreadyActived = true;
            canupdate = true;
            if(ui_skill) ui_skill.OnUI_Select();
            OnBeginSkill();
        }
    }
    public virtual void EndSkill()
    {
        alreadyActived = false;
        canupdate = false;
        if (ui_skill) ui_skill.OnUI_Unselect();
        OnEndSkill();
    }
    private void Update() { absUpdate(); cooldownUpdate(); }
    internal virtual void absUpdate() { if (canupdate) OnUpdateSkill(); }
    internal virtual void cooldownUpdate() { }
    protected virtual void OnStart() { }
    protected abstract void OnBeginSkill();
    protected abstract void OnEndSkill();
    protected abstract void OnUpdateSkill();
}
