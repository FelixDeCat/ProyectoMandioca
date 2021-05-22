using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

//[ExecuteInEditMode]
public class tests : MonoBehaviour
{

    public Transform Roofs;
    bool roofs_enable = true;

    public void Switch()
    {
        roofs_enable = !roofs_enable;
        Roofs.gameObject.SetActive(roofs_enable);
    }

    void OnDrawGizmos()
    {
    }

}
