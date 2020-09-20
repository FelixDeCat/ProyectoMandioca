using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBouyancy : MonoBehaviour
{
    private Rigidbody _rb;

    public float forceIntensity;
    public float viscocityIntensity;
    public float speed;


    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    //private void FixedUpdate()
    //{
    //    _rb.velocity = new Vector3(_rb.velocity.x, _rb.velocity.y, _rb.velocity.z*speed *Time.deltaTime);
    //}

    void Bouyancy()
    {
        Vector3 force = new Vector3(0, forceIntensity, 0);
        _rb.AddForce(force,ForceMode.Impulse);
      //  _rb.AddForce(new Vector3(_rb.velocity.x * -viscocityIntensity, _rb.velocity.y,_rb.velocity.z * -viscocityIntensity));
        
      
        Debug.Log(_rb.velocity);
    }


   

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 24)
            Bouyancy();
    }
}
