using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MedusaParry_skill : SkillBase
{
    //Para poder usar la duracion aca, se tiene que poder decidir cuanto tiempo el OnPetrified State va a durar
    [SerializeField] private float duracion;
    [SerializeField] private int _powerOfForce;

    protected override void OnBeginSkill()
    {
        Main.instance.eventManager.SubscribeToEvent(GameEvents.ON_PLAYER_PARRY, PetrifyEnemies);
    }

    void PetrifyEnemies(params object[] param)
    {
        var entity = (EntityBase)param[0];

        if (entity.GetComponent<WalkingEntity>())
        {
            entity.GetComponent<WalkingEntity>().OnPetrified();
            Vector3 playerpos = Main.instance.GetChar().transform.position;
            Vector3 enemyPos = entity.transform.position;
            Vector3 dir = enemyPos - playerpos;
            Debug.Log(dir.normalized);
            Rigidbody rb = entity.GetComponent<Rigidbody>();
            Debug.Log("petrificado");
            rb.AddForce(dir.normalized* _powerOfForce, ForceMode.Impulse);
        }
           
    }

    protected override void OnEndSkill()
    {
        Main.instance.eventManager.UnsubscribeToEvent(GameEvents.ON_PLAYER_PARRY, PetrifyEnemies);
    }

    protected override void OnUpdateSkill()
    {
 
    }
}
