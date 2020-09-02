using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools.Extensions;

public class FallByBezier : MonoBehaviour
{
    [SerializeField] BezierPoint[] bezierPoints = new BezierPoint[2];
    float slider = 0;
    [Range(0,5)]
    [SerializeField] float fallSpeed = 5;

    [SerializeField] GameObject objToFall = null;


    public void FallOnHit()
    {
        StartCoroutine(Fall());
    }

    IEnumerator Fall()
    {
        while(slider < 1)
        {
            slider = Mathf.Clamp( slider + (fallSpeed / 100), 0,1);

            objToFall.transform.position = Extensions.GetPointOnBezierCurve(bezierPoints[0], bezierPoints[1], slider);
            objToFall.transform.eulerAngles = Extensions.GetRotatioOnBezierCurve(bezierPoints[0], bezierPoints[1], slider);

            yield return new WaitForSeconds(0.01f);
        }
    }
}
