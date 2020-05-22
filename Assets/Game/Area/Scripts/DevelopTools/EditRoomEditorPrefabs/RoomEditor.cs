using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class RoomEditor : MonoBehaviour
{
    private void Awake()
    {

    }
    // Update is called once per frame
    void Update()
    {
        Ray ray = new Ray();
        RaycastHit hit = new RaycastHit();
        //Debug.Log("Click registered");

        Debug.Log(Input.mousePosition);
        Vector3 pos = Input.mousePosition;
        //pos.z = 20;
        pos = Camera.main.ScreenToWorldPoint(pos,Camera.MonoOrStereoscopicEye.Mono);
        ray = new Ray(pos, Vector3.down);
        Debug.Log(pos);

        if (Physics.Raycast(ray, out hit))
        {
            hit.collider.GetComponent<Renderer>().material.color = Color.red;
            //Debug.Log(hit);
        }
    }
}
