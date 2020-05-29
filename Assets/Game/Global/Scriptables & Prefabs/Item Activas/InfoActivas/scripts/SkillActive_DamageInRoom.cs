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

     private GameObject glasses_face;
     private GameObject glasses_hand;
     private ParticleSystem cachin_particle;

    // [SerializeField] Atenea atenea;

    protected override void OnStart()
    {
        Main.instance.GetChar().charAnimEvent.Add_Callback("Skill_EndDrink", OnEnd);
        Main.instance.GetChar().charAnimEvent.Add_Callback("Skill_Cachin", Cachin);
    }

    protected override void OnBeginSkill() 
    {
        // atenea.GetComponent<AnimEvent>().Add_Callback("HitTheFloor", HitTheFloor);
        //SetPredicate(UsePredicate);
        glasses_face = Main.instance.GetChar().glasses_face;
        glasses_hand = Main.instance.GetChar().glasses_hand;
        cachin_particle = Main.instance.GetChar().cachin;
        glasses_face.SetActive(false);
        glasses_hand.SetActive(false);
    }

    Action GetBackControl = delegate { };

    public override void ConfigureRequest(Action request)
    {
        //base.ConfigureRequest(request); <--- esto nunca deberia estar descomentado

        //GetBackControl = Main.instance.GetChar().RequestExecuteASkill(request);
        // si el character me da luz verde de... che, podes usar la skill
        //ejecuta el OnOneShotExecute o el OnStartUse
    }

    protected override void OnOneShotExecute()
    {
        //Main.instance.GetChar().charanim.ForceAnimation("Drink");
        

        //Main.instance.GetMyCamera().DoCloseCamera();

        //glasses_face.SetActive(false);
        //glasses_hand.SetActive(true);
        //atenea.gameObject.SetActive(true);
        //atenea.GoToHero();
        //atenea.Anim_DamageRoom();
    }
    void OnEnd()
    {
        Main.instance.GetMyCamera().DoExitCamera();
        glasses_face.SetActive(false);
        glasses_hand.SetActive(false);
        GetBackControl.Invoke();
    }

    void Cachin()
    {
        Vector3 pos_collision = Main.instance.GetChar().transform.position;

        feedback.transform.position = pos_collision;
        feedback.Play();
        var enems = Physics.OverlapSphere(pos_collision, radius, layerenem).Select(x => x.GetComponent<EnemyBase>()).ToList();

        foreach (EnemyBase enemy in enems)
        {
            Debug.Log(enemy.gameObject.name);
            //enemy.TakeDamage(damagePower, pos_collision, dmgType, Main.instance.GetChar());
            enemy.OnPetrified();
        }


        cachin_particle.Play();
        glasses_face.SetActive(true);
        glasses_hand.SetActive(false);
    }

    
    protected override void OnEndSkill() 
    {
        
    }
    protected override void OnUpdateSkill() { }
    protected override void OnStartUse() { }
    protected override void OnStopUse() { }
    protected override void OnUpdateUse() { }
}
