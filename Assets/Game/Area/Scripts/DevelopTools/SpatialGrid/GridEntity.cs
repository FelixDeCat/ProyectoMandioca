using System;
using UnityEngine;


public class GridEntity : MonoBehaviour
{
    public PlayObject[] myPlayObjects;

    public bool debug;
    public bool acty;

    bool alreadyInitialized = false;
    public void Initialize()
    {
        if (!alreadyInitialized)
        {
            alreadyInitialized = true;
            myPlayObjects = this.gameObject.GetComponentsInChildren<PlayObject>();
            for (int i = 0; i < myPlayObjects.Length; i++)
            {
                Debug.Log("TENGO UNA ENTIDAD");
                myPlayObjects[i].Initialize();
            }
        }
    }

    public void On()
    {
        acty = true;
        for (int i = 0; i < myPlayObjects.Length; i++)
        {
            myPlayObjects[i].On();
        }
    }
    public void Off()
    {
        acty = false;
        for (int i = 0; i < myPlayObjects.Length; i++)
        {
            myPlayObjects[i].Off();
        }
    }
    public bool onGrid;


    private void OnDrawGizmos()
    {
        if (debug)
        {
            if (acty)
            {
                Gizmos.color = Color.green;
            }
            else
            {
                Gizmos.color = Color.red;
            }
            Gizmos.color = acty ? Color.green : Color.red;
            Gizmos.DrawSphere(transform.position + transform.up * 3, 0.3f);
        }
    }

}
