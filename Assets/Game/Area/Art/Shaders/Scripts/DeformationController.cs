using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class DeformationController : MonoBehaviour
{
    public Material mat;

    private InteractSensor _posPlayer;

    public GameObject pos;

    public Camera cam;

    private void Awake()
    {
        _posPlayer = FindObjectOfType<InteractSensor>();
    }

    private void Update()
    {
        mat.SetVector("_Pos", pos.transform.position);

        if (cam != null)
        {
            Shader.SetGlobalVector("RTCameraPosition", cam.transform.position);
            Shader.SetGlobalFloat("RTCameraSize", cam.orthographicSize);


        }
    }

}
