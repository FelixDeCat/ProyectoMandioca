using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class JabaliPushComponent : CombatComponent
{
    [Header("Overlap")]
    [SerializeField] LayerMask _lm = 0;
    [SerializeField] LayerMask obstacleLayer = 0;
    [SerializeField] float distance = 2;
    [SerializeField] float angleAttack = 180;
    [SerializeField] Transform rot = null;

    Action StopCallback = delegate { };

    public void Configure(Action<DamageReceiver> _callback, Action _Stop)
    {
        StopCallback = _Stop;
        Configure(_callback);
    }

    public override void ManualTriggerAttack()
    {
        Calculate();
    }
    public override void BeginAutomaticAttack()
    {

    }

    bool play;
    public override void Play()
    {
        play = true;
    }

    public override void Stop()
    {
        play = false;
    }

    void Calculate()
    {
        if (!play) return;

        DamageReceiver entity = null;
        var enemies = Physics.OverlapSphere(rot.position, distance, _lm);
        for (int i = 0; i < enemies.Length; i++)
        {
            Vector3 dir = enemies[i].transform.position - rot.position;
            float angle = Vector3.Angle(rot.forward, dir);

            if (enemies[i].GetComponent<DamageReceiver>() && dir.magnitude <= distance && angle < angleAttack)
            {
                if (entity == null)
                    entity = enemies[i].GetComponent<DamageReceiver>();
            }
        }

        if (entity != null)
            giveDmgCallback.Invoke(entity);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!play) return;  
        if ((1 << other.gameObject.layer & obstacleLayer) != 0)
        {
            if (other.GetComponent<DamageReceiver>())
                giveDmgCallback.Invoke(other.GetComponent<DamageReceiver>());
            else
                StopCallback();
            Stop();
        }
    }

}
