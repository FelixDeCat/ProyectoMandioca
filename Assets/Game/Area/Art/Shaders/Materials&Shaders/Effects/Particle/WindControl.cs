using UnityEngine;

[ExecuteAlways]
public class WindControl : MonoBehaviour
{
    public Camera RTCamera;

    void Update()
    {
        if (RTCamera != null)
        {
            Shader.SetGlobalVector("RTCameraPosition", RTCamera.transform.position);
            Shader.SetGlobalFloat("RTCameraSize", RTCamera.orthographicSize);
        }
    }
}
