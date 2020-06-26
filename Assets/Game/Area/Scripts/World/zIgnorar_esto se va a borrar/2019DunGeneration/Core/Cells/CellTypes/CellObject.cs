using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools.Extensions;

[ExecuteInEditMode]
public abstract class CellObject : MonoBehaviour
{
    [Header("Rotation")]
    public bool rotate;
    public Transform rotator;

    [Header("Sets")]
    public List<GameObject> sets;
    public bool randomize;
    public bool reset;
    public bool changeset;
    bool side;


    public int index = 0;

    protected bool aux;

    public bool SphereMask;
    bool auxspheremask;

    protected virtual void Update()
    {
        if (rotate)
        {
            rotate = false;
            if(rotator) rotator.eulerAngles += new Vector3(0, 90, 0);
        }

        if (SphereMask)
        {
            SphereMask = false;
            auxspheremask = !auxspheremask;

            for (int i = 0; i < sets.Count; i++)
            {
                //var mask = sets[i].GetComponent<Spheremask>();
                //if (auxspheremask)
                //{
                //    if (mask == null)
                //    {
                //        sets[i].AddComponent<Spheremask>();
                //    }
                //}
                //else
                //{
                //    if (mask != null)
                //    {
                //        DestroyImmediate( sets[i].GetComponent<Spheremask>());
                //    }
                //}
            }

            
        }

        if (changeset)
        {
            changeset = false;
            aux = true;
            if (reset) { index = 0; reset = false; }

            if (randomize)
            {
                index = Random.Range(0, sets.Count);
            }
            else
            {
                index = index.NextIndex(sets.Count);
            }

            for (int i = 0; i < sets.Count; i++)
            {
                sets[i].SetActive(i == index);
            }
        }
    }
    public int Xpos { get { return (int)transform.position.x; } }
    public int Zpos { get { return (int)transform.position.z; } }

    public abstract void Refresh();


   

}
