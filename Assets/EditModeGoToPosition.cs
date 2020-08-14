using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class EditModeGoToPosition : MonoBehaviour
{
    [SerializeField] bool Run;
    [SerializeField] Transform blockposition;

    void Update()
    {
        if (Run && blockposition != null)
        {
            this.transform.position = blockposition.position;
        }
    }
}
