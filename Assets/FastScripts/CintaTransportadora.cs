using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CintaTransportadora : MonoBehaviour
{
    public Transform endpos;
    public Transform inipos;
    public Vector3 dirTogo;
    public float speed;
    // Start is called before the first frame update
  

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(endpos.position,transform.position) <= 10)
        {
            transform.position = inipos.position;
        }
        transform.position += dirTogo * speed * Time.deltaTime;
    }
}
