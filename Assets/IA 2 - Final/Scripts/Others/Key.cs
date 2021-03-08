using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    [SerializeField] float _speed=1;
   

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, _speed);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Key_container>())
        {
            other.gameObject.GetComponent<Key_container>().haveKey = true;
            Destroy(this.gameObject);
        }
    }
}
