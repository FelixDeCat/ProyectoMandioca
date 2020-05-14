using UnityEngine;
using XInputDotNetPure;
using ToolsMandioca.Extensions;
using System.Collections.Generic;
using System;


public class CompleteCameraController : MonoBehaviour
{
    public static CompleteCameraController instancia;

    Transform target;
    float smoothTime = 0.1f;
    private Vector3 velocity = Vector3.zero;

    PlayerIndex pindex;

    public List<CamConfig> camconfig;

    public void SetTargetToLookAt(Transform _target) { target = _target; }

    //public Transform[] poscamaras;
    int posCamindex;

    float reset;


    Vector3 originalPos;

    public Camera myCam;

    bool is_look_at = false;
    Transform postolookat;
    Quaternion original_rot;

    float timerrot;

    Action callback_endAnimation;

    public void BeginLookAt(Transform pos)
    {
        is_look_at = true;
        postolookat = pos;
    }

    public void EndLookat()
    {
        is_look_at = false;
        this.transform.rotation = original_rot;
    }

    private void Awake()
    {
        instancia = this;
        myCam = GetComponent<Camera>();
        original_rot = transform.rotation;
    }

    public void InstantAjust()
    {
        transform.position = camconfig[posCamindex].poscamara.position;
        transform.rotation = camconfig[posCamindex].poscamara.rotation;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            posCamindex = posCamindex.NextIndex(camconfig.Count);
            Reconfigure();
        }
    }

    public void ChangeToNormal()
    {
        posCamindex = 0;
        smoothTime = 0.1f;
        Reconfigure();
    }
    public void ChangeToDungeon()
    {
        posCamindex = 1;
        smoothTime = 0.1f;
        Reconfigure();
    }
    public void ChangeToHerrero()
    {
        posCamindex = 2;
        smoothTime = 0.1f;
        Reconfigure();
    }

    bool camtransition;
    public void ChangeToCustom(CamConfig _camconfig)
    {
        callback_endAnimation = delegate { };
        posCamindex = 3;
        camconfig[posCamindex] = _camconfig;
        camtransition = true;
        timerrot = 0;
        Reconfigure();
    }
    public void ChangeToCustomInstant(CamConfig _camconfig)
    {
        callback_endAnimation = delegate { };
        posCamindex = 3;
        camconfig[posCamindex] = _camconfig;
        camtransition = true;
        timerrot = 0;
        Reconfigure();
        transform.position = camconfig[posCamindex].poscamara.position;
        transform.rotation = camconfig[posCamindex].poscamara.rotation;
    }

    public void AddCallback(Action callback)
    {
        timerrot = 0;
        camtransition = true;
        callback_endAnimation = callback;
    }

    public void Reconfigure()
    {
        myCam.fieldOfView = camconfig[posCamindex].fieldOfView;
        myCam.cullingMask = camconfig[posCamindex].CullingMask;
        smoothTime = camconfig[posCamindex].smoothTime;
    }

    [System.Serializable]
    public class CamConfig
    {
        public string config_name;
        [Range(1, 179)] public float fieldOfView = 60;
        public Transform poscamara;
        public LayerMask CullingMask;

        [Header("Follow Configs")]
        public float smoothTime = 0.1f;

        [Header("Shake configs")]
        public float shakeDuration = 0.1f;
        // Amplitude of the shake. A larger value shakes the camera harder.
        public float shakeAmount = 0.1f;
        public float decreaseFactor = 1.0f;
    }


    void FixedUpdate()
    {
        if (is_look_at)
        {
            Vector3 targetPosition = target.TransformPoint(Vector3.zero);
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
            transform.LookAt(postolookat);
        }
        else
        {
            // Smoothly move the camera towards that target position
            transform.position = Vector3.SmoothDamp(transform.position,
                                                    camconfig[posCamindex].poscamara.position,
                                                    ref velocity,
                                                    smoothTime);

            if (camtransition)
            {
                if (timerrot < smoothTime)
                {
                    timerrot = timerrot + 1 * Time.deltaTime;
                    transform.rotation = Quaternion.Lerp(transform.rotation,
                                                         camconfig[posCamindex].poscamara.rotation,
                                                         timerrot);
                }
                else
                {
                    timerrot = 0;
                    camtransition = false;
                    callback_endAnimation.Invoke();
                }

                if (Vector3.SqrMagnitude(velocity) <= 0.1f)
                {
                    // timerrot = 0;

                }
            }
            else
            {
                transform.rotation = camconfig[posCamindex].poscamara.rotation;
            }
        }
    }
}