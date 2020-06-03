using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicThrowable : MonoBehaviour
{
    public float local_force_multiplier = 5;
    public Sensor sensor;
    int damage = 5;
    bool canDisapear;
    Rigidbody myrig;
    Transform ttotrack;
    bool track;

    public void Throw(Vector3 postion, Vector3 vectorDirection, float forceMultiplerAux = 1, int _damage = 5)
    {
        damage = _damage;
        sensor.AddCallback_OnTriggerEnter(ReceiveEntityToDamage);
        myrig = GetComponent<Rigidbody>();
        this.transform.position = postion;
        this.transform.eulerAngles = new Vector3(Random.Range(0,360), Random.Range(0, 360), Random.Range(0, 360));
        myrig.AddForce(vectorDirection * local_force_multiplier * forceMultiplerAux, ForceMode.VelocityChange);
        canDisapear = true;
    }

    public void BegigTrackTransform(Transform t)
    {
        ttotrack = t;
        track = true;
    }
    public void EndTranckTransform() => track = false;

    private void Update()
    {
        if (canDisapear)
        {
            if (myrig.velocity.magnitude <= 1)
            {
                gameObject.SetActive(false);
                return;
            }
        }
        if (track) transform.position = ttotrack.position;
    }

    void ReceiveEntityToDamage(GameObject go)
    {
        var ent = go.GetComponent<EntityBase>();
        if (ent != null)
        {
            ent.TakeDamage(damage, transform.position, Damagetype.normal);
        }
    }
}
