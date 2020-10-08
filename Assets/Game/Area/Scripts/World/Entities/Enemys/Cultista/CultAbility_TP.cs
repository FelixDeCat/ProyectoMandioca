using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CultAbility_TP : GOAP_Skills_Base
{
    [SerializeField] Collider col = null;
    [SerializeField] CombatArea spawneablePosition = null;

    protected override void OnEndSkill()
    {
    }

    protected override void OnExecute()
    {
    }

    protected override void OnFixedUpdate()
    {
    }

    protected override void OnInitialize()
    {
        throw new System.NotImplementedException();
    }

    protected override void OnPause()
    {
        throw new System.NotImplementedException();
    }

    protected override void OnResume()
    {
        throw new System.NotImplementedException();
    }

    protected override void OnTurnOff()
    {
        throw new System.NotImplementedException();
    }

    protected override void OnTurnOn()
    {
        throw new System.NotImplementedException();
    }

    protected override void OnUpdate()
    {
        throw new System.NotImplementedException();
    }

    void TeleportForAnim()
    {
        Vector2 aux;
        if (spawneablePosition.isCircle)
        {
            float ang = Random.Range(0, 360);

            Vector3 pos;
            pos.x = spawneablePosition.transform.position.x + spawneablePosition.circleRadius * Random.value * Mathf.Sin(ang * Mathf.Deg2Rad);
            pos.y = spawneablePosition.transform.position.y;
            pos.z = spawneablePosition.transform.position.z + spawneablePosition.circleRadius * Random.value * Mathf.Cos(ang * Mathf.Deg2Rad);

            transform.position = pos;
        }
        else
        {
            aux.x = -(spawneablePosition.cubeArea.x / 2) + spawneablePosition.cubeArea.x * Random.value;
            aux.y = -(spawneablePosition.cubeArea.y / 2) + spawneablePosition.cubeArea.y * Random.value;
            transform.position = spawneablePosition.transform.position + new Vector3(aux.x, 0, aux.y);
        }
        col.enabled = true;
    }
}
