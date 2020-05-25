using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
    }
    void HitTheFloor()
    {
        Vector3 pos_collision = new Vector3(atenea.transform.position.x, Main.instance.GetChar().transform.position.y, atenea.transform.position.z);

        feedback.transform.position = pos_collision;
        feedback.Play();
        var enems = Physics.OverlapSphere(pos_collision, radius, layerenem).Select(x => x.GetComponent<EnemyBase>());

        foreach (EnemyBase enemy in enems)
        {
            //enemy.TakeDamage(damagePower, pos_collision, dmgType, Main.instance.GetChar());
            enemy.OnStun();
        }
    }

    protected override void OnOneShotExecute()
    {
        atenea.gameObject.SetActive(true);
        atenea.GoToHero();
        atenea.Anim_DamageRoom();
    }

    
    protected override void OnEndSkill() { }
    protected override void OnUpdateSkill() { }
    protected override void OnStartUse() { }
    protected override void OnStopUse() { }
    protected override void OnUpdateUse() { }
}
