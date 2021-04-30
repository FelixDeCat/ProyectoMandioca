using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class RayoLaser_Bounce : MonoBehaviour
{
    [SerializeField] int reflections = 0;
    [SerializeField] float maxLength = 100f;
    [SerializeField] LineRenderer lineRenderer = null;

    RaycastHit hit;
    float time;

    public void On()
    {
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, transform.position);
      
    }

    public void Off()
    {
        lineRenderer.enabled = false;
       

    }

    public void Pause()
    {
        if (lineRenderer.enabled) lineRenderer.material.SetFloat("_TimeScale", 0);
    }
    
    public void Resume()
    {
        if (lineRenderer.enabled) lineRenderer.material.SetFloat("_TimeScale", 1);
    }

    public void SetFinalPos(Vector3 pos)
    {
        lineRenderer.SetPosition(1, pos);
    }

    public void OnUpdate()
    {
        var ray = new Ray(transform.position, transform.forward);

        lineRenderer.positionCount = 1;
        lineRenderer.SetPosition(0, transform.position);
        float remainingLength = maxLength;

        for (int i = 0; i < reflections; i++)
        {
            if (Physics.Raycast(ray.origin, ray.direction, out hit, remainingLength))
            {
                lineRenderer.positionCount += 1;
                lineRenderer.SetPosition(lineRenderer.positionCount - 1, hit.point);
                remainingLength -= Vector3.Distance(ray.origin, hit.point);
                ray = new Ray(hit.point, Vector3.Reflect(ray.direction, hit.normal));

                if (hit.collider.tag != "Mirror")
                    break;
            }
            else
            {
                lineRenderer.positionCount += 1;
                lineRenderer.SetPosition(lineRenderer.positionCount - 1, ray.origin + ray.direction * remainingLength);
            }
        }

    }
}
