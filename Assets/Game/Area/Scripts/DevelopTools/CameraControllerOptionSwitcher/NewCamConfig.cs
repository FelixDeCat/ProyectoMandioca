using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewCamConfig : MonoBehaviour
{
    Camera thiscamera;

    [Header("Follow Configs")]
    public float smoothTime = 0.1f;

    [Header("Shake configs")]
    public float shakeDuration = 0.1f;
    // Amplitude of the shake. A larger value shakes the camera harder.
    public float shakeAmount = 0.1f;
    public float decreaseFactor = 1.0f;

    CompleteCameraController.CamConfig conf;

    void Awake()
    {
        thiscamera = GetComponent<Camera>();
        conf = new CompleteCameraController.CamConfig();
        conf.config_name = this.name;
        conf.CullingMask = thiscamera.cullingMask;
        conf.fieldOfView = thiscamera.fieldOfView;
        conf.poscamara = thiscamera.transform;
        conf.smoothTime = smoothTime;
        conf.shakeDuration = shakeDuration;
        conf.shakeAmount = shakeAmount;
        conf.decreaseFactor = decreaseFactor;
        thiscamera.enabled = false;
    }

    public CompleteCameraController.CamConfig GetCameraConfig()
    {
        return conf;
    }


    

}
