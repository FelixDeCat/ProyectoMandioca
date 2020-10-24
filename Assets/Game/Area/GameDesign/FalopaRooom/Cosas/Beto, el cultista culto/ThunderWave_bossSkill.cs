using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;
using Tools.Extensions;

public class ThunderWave_bossSkill : GOAP_Skills_Base
{
    Ente _ent;
    DamageData dmgData;

    [SerializeField] int damage;
    [SerializeField] int knock;
    [SerializeField] float radious = 5f;

    protected override void OnEndSkill()
    {
        _ent.OnSkillAction -= ThuderWave;
    }

    protected override void OnExecute()
    {
        owner.GetComponentInChildren<Animator>().Play("ThunderWave");

        _ent.OnSkillAction += ThuderWave;
    }

    protected override void OnFixedUpdate()
    {

    }

    void ThuderWave()
    {
        var affectedPO  = Extensions.FindInRadius<PlayObject>(owner, radious);

        for (int i = 0; i < affectedPO.Count; i++)
        {
            DamageReceiver obj = affectedPO[i].GetComponent<DamageReceiver>();

            if(obj != null)
            {
                obj.TakeDamage(dmgData);
            }
        }
    }

    protected override void OnInitialize()
    {
        _ent = owner.GetComponent<Ente>();

        dmgData = GetComponent<DamageData>().SetDamage(damage).SetDamageInfo(DamageInfo.NonParry).SetKnockback(knock);
    }

    protected override void OnPause()
    {

    }

    protected override void OnResume()
    {

    }

    protected override void OnTurnOff()
    {

    }

    protected override void OnTurnOn()
    {

    }

    protected override void OnUpdate()
    {

    }

 
}
