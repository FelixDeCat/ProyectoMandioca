using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BashDashSensor : TriggerReceiver
{
    public BoxCollider boxCollider;

    public float _powerOfForce = 5;

    public void Awake()
    {
        DisableSensor();
    }

    public void EnableSensor() { boxCollider.enabled = true; }
    public void DisableSensor() { boxCollider.enabled = false; }

    protected override void OnExecute(params object[] parameters)
    {

        Debug.Log("HagoBashdash");
        var col = (Collider)parameters[0];

        if (col == null) return;

        var effectReceiver = col.GetComponent<EffectReceiver>();

        if (effectReceiver != null)
        {
            effectReceiver.GetComponent<EffectReceiver>().TakeEffect(EffectName.OnPetrify);
            Vector3 playerpos = Main.instance.GetChar().transform.position;
            Vector3 enemyPos = effectReceiver.transform.position;
            Vector3 dir = enemyPos - playerpos;
            Rigidbody rb = effectReceiver.GetComponent<Rigidbody>();
            AudioManager.instance.PlaySound("OnPetrifyBegin");
            rb.AddForce(dir.normalized * _powerOfForce, ForceMode.Impulse);
        }
    }
}
