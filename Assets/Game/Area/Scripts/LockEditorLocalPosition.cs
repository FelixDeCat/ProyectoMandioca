using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LockEditorLocalPosition : MonoBehaviour
{
    Vector3 postolock;
    private void Awake()
    {
        postolock = this.transform.localPosition;
    }
    private void Update()
    {
        this.transform.localPosition = new Vector3(0, 0, 0);
    }
}
