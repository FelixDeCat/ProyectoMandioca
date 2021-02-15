using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class EditModeGoToPosition : MonoBehaviour
{
    [SerializeField] bool Run = false;
    [SerializeField] Transform blockposition = null;

    void Update()
    {
        if (Run && blockposition != null)
        {
            this.transform.position = blockposition.position;
        }
    }
}
