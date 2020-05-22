using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamConfiguration : MonoBehaviour
{
    // Start is called before the first frame update
    [Range(1, 179)]
    public float fieldOfView = 60;
    public LayerMask CullingMask;

    public float smoothTime = 0.1f;

    public float shakeDuration = 0.1f;
    public float shakeAmmount = 0.1f;
   
}
