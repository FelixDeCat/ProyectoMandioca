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

    Action StopCallback;

    public void Configure(Action<EntityBase> _callback, Action _Stop)
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

    public override void Play()
    {

    }

    public override void Stop()
    {
        StopCallback();
    }

    void Calculate()
    {
        EntityBase entity = null;

        var obstacles = Physics.OverlapSphere(rot.position, 1, obstacleLayer);
        for (int i = 0; i < obstacles.Length; i++)
        {
            Vector3 dir = obstacles[i].transform.position - rot.position;
            float angle = Vector3.Angle(rot.forward, dir);

            if (angle < 45)
            {
                Stop();
            }
        }

        var enemies = Physics.OverlapSphere(rot.position, distance, _lm);
        for (int i = 0; i < enemies.Length; i++)
        {
            Vector3 dir = enemies[i].transform.position - rot.position;
            float angle = Vector3.Angle(rot.forward, dir);

            if (enemies[i].GetComponent<EntityBase>() && dir.magnitude <= distance && angle < angleAttack)
            {
                if (entity == null)
                    entity = enemies[i].GetComponent<EntityBase>();
            }
        }

        if (entity != null)
            callback.Invoke(entity);
    }

}
