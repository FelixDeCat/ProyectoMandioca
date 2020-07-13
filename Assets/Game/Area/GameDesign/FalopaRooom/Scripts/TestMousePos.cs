using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMousePos : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos = Camera.main.WorldToScreenPoint(mousePos);
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        Vector3 finalmousePos = new Vector3(mousePos.x, mousePos.y, 0);
        Debug.Log(finalmousePos);
    }
}
