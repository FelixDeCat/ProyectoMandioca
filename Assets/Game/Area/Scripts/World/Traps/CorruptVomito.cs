using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorruptVomito : PlayObject
{
    [SerializeField, Range(0,1)] float speedSlowPercent = 0.5f;

    CharacterHead head;
    List<EnemyBase> enemies = new List<EnemyBase>();

    protected override void OnFixedUpdate()
    {
    }

    protected override void OnInitialize()
    {
    }

    protected override void OnPause()
    {
    }

    protected override void OnResume()
    {
    }

    protected override void OnTurnOff()
    {
        head?.SetNormalSpeed();
        for (int i = 0; i < enemies.Count; i++)
        {
            float newSpeed = enemies[i].ChangeSpeed(-1);
            enemies[i].ChangeSpeed(newSpeed);
        }
        head = null;
        enemies = new List<EnemyBase>();
    }

    protected override void OnTurnOn()
    {
    }

    protected override void OnUpdate()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CharacterHead>())
        {
            head = other.GetComponent<CharacterHead>();
            float newSpeed = head.GetCharMove().GetSpeed() * speedSlowPercent;
            head.SetSlow(newSpeed);
        }
        else if (other.GetComponent<EnemyBase>())
        {
            enemies.Add(other.GetComponent<EnemyBase>());
            float newSpeed = other.GetComponent<EnemyBase>().ChangeSpeed(-1) * speedSlowPercent;
            other.GetComponent<EnemyBase>().ChangeSpeed(newSpeed);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<CharacterHead>())
        {
            other.GetComponent<CharacterHead>().SetNormalSpeed();
            head = null;
        }
        else if (other.GetComponent<EnemyBase>())
        {
            enemies.Remove(other.GetComponent<EnemyBase>());
            float newSpeed = other.GetComponent<EnemyBase>().ChangeSpeed(-1);
            other.GetComponent<EnemyBase>().ChangeSpeed(newSpeed);
        }
    }
}
