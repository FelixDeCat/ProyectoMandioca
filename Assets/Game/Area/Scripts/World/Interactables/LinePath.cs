using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[ExecuteInEditMode]
public class LinePath : MonoBehaviour
{
    public LineRenderer ln;
    public Transform[] positions;

    public bool execute;

    public void Update()
    {
        if (execute)
        {
            //execute = false;
            ln.positionCount = positions.Length;
            ln.SetPositions(positions.Select(x => x.localPosition).ToArray());
        }
    }
}
