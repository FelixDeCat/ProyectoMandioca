using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyAtack : CombatComponent
{
    [Header("Overlap")]
    [SerializeField] LayerMask _lm = 0;
    [SerializeField] float distance = 3;
    [SerializeField] float angleAttack = 45;
    [SerializeField] Transform rot = null;

    public override void ManualTriggerAttack()
    {
        Calculate();
    }
    public override void BeginAutomaticAttack()
    {

    }

    public override void Play()
    {

    }

    public override void Stop()
    {
    }

    void Calculate()
    {
        DamageReceiver entity = null;

        var enemies = Physics.OverlapSphere(rot.position, distance, _lm);
        for (int i = 0; i < enemies.Length; i++)
        {
            Vector3 dir = enemies[i].transform.position - rot.position;
            float angle = Vector3.Angle(rot.forward, dir);

            if (enemies[i].GetComponent<DamageReceiver>() && dir.magnitude <= distance && angle < angleAttack)
            {
                if (entity == null)
                {
                    entity = enemies[i].GetComponent<DamageReceiver>();
                    giveDmgCallback.Invoke(entity);
                }
            }
        }
    }

}
