using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricBullet : Waves
{
    [Header("Spawnea si colisiona con orbe")]
    [SerializeField] int speed = 5;
    [SerializeField] float lifeTime = 2;
    [SerializeField] int divAmmount = 8;
    [SerializeField] Waves prefabBullet = null;

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        if (other.gameObject.GetComponent<ElectricOrb>())
        {
            Destroy(other.gameObject);

            float rot = 360f / divAmmount;

            for (int i = 0; i < divAmmount; i++)
            {
                float internalAngle = rot * i;
                Vector3 aux = transform.position + transform.forward * Mathf.Cos(internalAngle * Mathf.Deg2Rad) + transform.right * Mathf.Sin(internalAngle * Mathf.Deg2Rad);

                Waves auxGO = Instantiate(prefabBullet);
                auxGO.SetSpeed(speed).SetLifeTime(lifeTime);
                auxGO.transform.forward = aux - transform.position;
            }
            Destroy(gameObject);
        }
    }
}
