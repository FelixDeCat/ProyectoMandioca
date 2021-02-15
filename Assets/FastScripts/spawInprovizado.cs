using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class spawInprovizado : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Main.instance.GetChar().transform.position = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
