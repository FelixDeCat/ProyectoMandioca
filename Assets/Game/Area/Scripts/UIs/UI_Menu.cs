using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UI_Menu : UI_Base
{
    
    public Button btn_newSkill;

    public override void Open()
    {
        base.Open();
        btn_newSkill.interactable = Main.instance.GetPasivesManager().I_Have_An_Active_Request();
    }

    public void BTN_Spend_SkillPoint()
    {
        //Main.instance.skillmanager_pasivas.EVENT_GetRequest();
    }

    protected override void OnAwake() { }
    public override void Refresh() { }
    protected override void OnEndCloseAnimation() { }
    protected override void OnEndOpenAnimation() { }
    protected override void OnStart() { }
    protected override void OnUpdate() { }
}
