using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveRaw : MonoBehaviour
{

    public float scrollSpeed;
    public RawImage m_material;
    // Update is called once per frame
    void Update()
    {
        float x = Mathf.Repeat(Time.time * scrollSpeed, 1); 
        m_material.uvRect = new Rect(x, x, 1, 1);
    }
}
