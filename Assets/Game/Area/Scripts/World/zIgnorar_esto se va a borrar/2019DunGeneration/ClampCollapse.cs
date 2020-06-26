using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools.Extensions;


[ExecuteInEditMode]
public class ClampCollapse : MonoBehaviour
{
    [Header("Execute")]
    public bool execute;
    [Header("ToIgnore")]
    public bool ignore_scale;
    public bool ignore_position;
    public bool ignore_rotation;

    private void Update()
    {
        if (execute)
        {
            var childs = transform.GetComponentsInChildren<Transform>();

            foreach (var c in childs)
            {
                if(!ignore_position) c.position = c.position.ClampV3ZeroDotFive();
                if(!ignore_rotation) c.eulerAngles = c.eulerAngles.ClampV3ZeroDotFive();
                if(!ignore_scale) c.localScale = c.localScale.ClampV3ZeroDotFive();
            }
        }
    }
}
