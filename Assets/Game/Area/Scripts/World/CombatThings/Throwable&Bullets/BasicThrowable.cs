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
    public Collider col;
    public Renderer render;
    Transform ttotrack;
    bool track;

    private void Awake()
    {
        myrig = GetComponent<Rigidbody>();
    }

    public void Throw(Vector3 postion, Vector3 vectorDirection, float forceMultiplerAux = 1, int _damage = 5)
    {
        render.enabled = true;
        col.enabled = true;
        myrig.isKinematic = false;
        damage = _damage;
        sensor.AddCallback_OnTriggerEnter(ReceiveEntityToDamage);
        this.transform.position = postion;
        this.transform.forward = vectorDirection;
        //this.transform.eulerAngles = new Vector3(Random.Range(0,360), Random.Range(0, 360), Random.Range(0, 360));
        myrig.AddForce(vectorDirection * local_force_multiplier * forceMultiplerAux, ForceMode.VelocityChange);
        canDisapear = true;
    }

    public void BegigTrackTransform(Transform t)
    {
        render.enabled = false;
        col.enabled = false;
        myrig.isKinematic = true;
        ttotrack = t;
        track = true;
    }
    public void EndTranckTransform() => track = false;

    private void Update()
    {
        if (canDisapear)
        {
            //if (myrig.velocity.magnitude <= 1)
            //{
            //    gameObject.SetActive(false);
            //    return;
            //}
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
