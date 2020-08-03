using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CombatArea : MonoBehaviour
{
    public bool isCircle;
    public Vector2 cubeArea;
    public float circleRadius;

    private void OnDrawGizmosSelected()
    {
        if (isCircle)
            Gizmos.DrawWireSphere(transform.position, circleRadius);
        else
            Gizmos.DrawWireCube(transform.position, new Vector3(cubeArea.x, 1, cubeArea.y));
    }
}
