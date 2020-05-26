using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class SkillActive_DamageInRoom : SkillActivas
{
    [SerializeField] private int damagePower = 10;
    [SerializeField] private ParticleSystem feedback = null;
    [SerializeField] private Damagetype dmgType = Damagetype.normal;
    [SerializeField] LayerMask layerenem = 0;
    [SerializeField] private float radius = 10;

    [SerializeField] Atenea atenea;

    protected override void OnBeginSkill() 
    {
        atenea.GetComponent<AnimEvent>().Add_Callback("HitTheFloor", HitTheFloor);
        //SetPredicate(UsePredicate);
    }
    void HitTheFloor()
    {
        Vector3 pos_collision = new Vector3(atenea.transform.position.x, Main.instance.GetChar().transform.position.y, atenea.transform.position.z);

        feedback.transform.position = pos_collision;
        feedback.Play();
        var enems = Physics.OverlapSphere(pos_collision, radius, layerenem).Select(x => x.GetComponent<EnemyBase>()).ToList();

        foreach (EnemyBase enemy in enems)
        {
            Debug.Log(enemy.gameObject.name);
            //enemy.TakeDamage(damagePower, pos_collision, dmgType, Main.instance.GetChar());
            enemy.OnPetrified();
        }
    }

    Action GetBackControl = delegate { };

    public override void ConfigureRequest(Action request)
    {
        //base.ConfigureRequest(request); <--- esto nunca deberia estar descomentado

        GetBackControl = Main.instance.GetChar().RequestExecuteASkill(request);
        // si el character me da luz verde de... che, podes usar la skill
        //ejecuta el OnOneShotExecute o el OnStartUse
    }

    protected override void OnOneShotExecute()
    {
        atenea.gameObject.SetActive(true);
        atenea.GoToHero();
        atenea.Anim_DamageRoom();
    }

    
    protected override void OnEndSkill() 
    {
        
    }
    protected override void OnUpdateSkill() { }
    protected override void OnStartUse() { }
    protected override void OnStopUse() { }
    protected override void OnUpdateUse() { }
}
