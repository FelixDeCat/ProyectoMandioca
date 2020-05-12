using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCamera : MonoBehaviour
{
    public Transform target;
    private Vector3 velocity = Vector3.zero;
    public float smooth = 1f;
    public Vector3 offset;
    public bool lookAt;
    public float shakeAmmount;
    private float shakeDurationCurrent;
    public float shakeDuration;
    private bool activeShake;

    public float zoomDuration;
    const int FIELD_OF_VIEW_ORIGINAL = 60;
    float fieldOfView_toZoom =60;

    PingPongLerp pingpongZoom;

    public LayerMask _layermask_raycast_mask;

    Camera mycam;

    private void Start()
    {
        shakeDurationCurrent = shakeDuration;
        mycam = GetComponent<Camera>();
        pingpongZoom = new PingPongLerp();
        pingpongZoom.Configure(Zoom, false);
    }
    private void LateUpdate()
    {
        if (activeShake)
        {
            Shake();
        }
    }
    public void DoFastZoom(float _speedanim, float _fieldOfViewToZoom = 55)
    {
        fieldOfView_toZoom = _fieldOfViewToZoom;
        pingpongZoom.Play(_speedanim);
    }

    void Zoom(float valtozoom) => mycam.fieldOfView = Mathf.Lerp(FIELD_OF_VIEW_ORIGINAL, fieldOfView_toZoom, valtozoom);
    private void Update()
    {
        pingpongZoom.Updatear();


        RaycastHit hit;

        var dir = target.position - this.transform.transform.position;

        if (Physics.Raycast(this.transform.transform.position, dir, out hit, 1000, _layermask_raycast_mask))
        {
            if (!hit.transform.GetComponent<CharacterHead>())
            {
                Main.instance.GetChar().Mask(true);
            }
            else
            {
                Main.instance.GetChar().Mask(false);
            }
        }

    }

    private void FixedUpdate()
    {
        Vector3 desiredposition = target.position + offset;

        Vector3 smoothedposition = Vector3.Lerp(transform.position, desiredposition, smooth * Time.deltaTime);
        transform.position = smoothedposition;
       
        
        if (lookAt)
        {
            transform.LookAt(target);
        }
    }

    public void BeginShakeCamera()
    {
        activeShake = true;
        shakeDurationCurrent = shakeDuration;
    }

    private void Shake()
    {
        if (shakeDurationCurrent > 0)
        {
            transform.position += Random.insideUnitSphere * shakeAmmount;
            shakeDurationCurrent -= Time.deltaTime;
        }
        else
        {
            shakeDurationCurrent = 0;
            activeShake = false;
        }

    }
    void SmoothDump()
    {
        transform.position = Vector3.SmoothDamp(transform.position,target.position,ref velocity,smooth);
                                                      
    }
}
