using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Horizontal3DLayoutGroup : MonoBehaviour
{
    UI3D_Element[] elements = new UI3D_Element[0];
    public bool executeInEditMode = false;

    public float separation = 1;

    private void Awake()
    {
        Order();
    }

    void Order()
    {
        var aux_biglist = GetComponentsInChildren<UI3D_Element>();
        List<UI3D_Element> myChilds = new List<UI3D_Element>();
        foreach (var a in aux_biglist)
        {
            if (a.transform.parent == this.transform)
            {
                myChilds.Add(a);
            }
        }
        elements = new UI3D_Element[myChilds.Count];
        for (int i = 0; i < myChilds.Count; i++)
        {
            elements[i] = myChilds[i];
        }

        float scale = elements[0].transform.localScale.x;
        float totalDistance = 0;
        for (int i = 0; i < elements.Length; i++)
        {
            totalDistance += elements[i].transform.localScale.x;
            if (i != elements.Length - 1) totalDistance += separation;
        }

        float middle = totalDistance / 2;
        float initialPos = this.transform.position.x - middle + scale/2;

        for (int i = 0; i < elements.Length; i++)
        {
            Vector3 pos = elements[i].transform.position;
            if (i != 0) initialPos = initialPos + scale + separation;
            pos.x = initialPos;
            elements[i].transform.position = pos;
        }
    }

    private void Update()
    {
        if (executeInEditMode)
        {
            executeInEditMode = false;

            Order();
        }
    }
}
