using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class CharacterGroundSensor : MonoBehaviour
{
    public Action<float> GroundOneShot;

    [SerializeField] float widht = 0.2f;
    [SerializeField] float lenght = 0.2f;
    [SerializeField] float height = 0.3f;
    [SerializeField] LayerMask floorMask = 1 << 21;

    [SerializeField] float gravityMultiplier = 0.2f;
    [SerializeField] float maxAceleration = 15;

    [SerializeField] float maxGroundAngle = 120;

    float groundAngle;

    RaycastHit raycastInfo;

    bool isGrounded;
    float timer;
    bool dontSnap;
    public float VelY { get; private set; }
    public bool IsInGround { get => isGrounded; private set { } }
    bool on;

    float lastY;
    bool isFalling;
    float disToFall;
    Action Falling;
    Action FallingEnd;

    private void Awake() => GroundOneShot += InGroundOneShot;

    public void SetFallingSystem(float _disToFall, Action _Falling, Action _FallingEnd)
    {
        disToFall = _disToFall;
        Falling = _Falling;
        FallingEnd = _FallingEnd;
    }

    public void TurnOn()
    {
        if (on) return;

        on = true;
        StartCoroutine(OnUpdate());
    }

    public void TurnOff()
    {
        if (!on) return;

        on = false;
        VelY = 0;

        isFalling = false;
    }

    IEnumerator OnUpdate()
    {
        while (on)
        {
            IsGrounded();

            if (!isGrounded)
            {
                VelY = Mathf.Clamp(VelY - timer * gravityMultiplier, -maxAceleration, 15);
                timer += Time.deltaTime;

                //DebugCustom.Log("Gravity", "Gravity", "TRUE");

                if (!isFalling &&  lastY - (transform.position.y - height) >= disToFall)
                {
                    Falling?.Invoke();
                    isFalling = true;
                }
            }

            yield return new WaitForSeconds(0.05f);
        }
    }

    public void IsGrounded()
    {

        if (Physics.Raycast(transform.position, -transform.up, out raycastInfo, height, floorMask))
            GroundOneShot(raycastInfo.point.y);
        else if(Physics.Raycast(transform.position + Vector3.right * widht, -transform.up, out raycastInfo, height, floorMask))
            GroundOneShot(raycastInfo.point.y);
        else if (Physics.Raycast(transform.position + Vector3.left * widht, -transform.up, out raycastInfo, height, floorMask))
            GroundOneShot(raycastInfo.point.y);
        else if (Physics.Raycast(transform.position + Vector3.forward * lenght, -transform.up, out raycastInfo, height, floorMask))
            GroundOneShot(raycastInfo.point.y);
        else if (Physics.Raycast(transform.position + Vector3.back * lenght, -transform.up, out raycastInfo, height, floorMask))
            GroundOneShot(raycastInfo.point.y);
        else if (Physics.Raycast(transform.position + Vector3.back * lenght + Vector3.right * widht, -transform.up, out raycastInfo, height, floorMask))
            GroundOneShot(raycastInfo.point.y);
        else if (Physics.Raycast(transform.position + Vector3.back * lenght + Vector3.left * widht, -transform.up, out raycastInfo, height, floorMask))
            GroundOneShot(raycastInfo.point.y);
        else if (Physics.Raycast(transform.position + Vector3.forward * lenght + Vector3.left * widht, -transform.up, out raycastInfo, height, floorMask))
            GroundOneShot(raycastInfo.point.y);
        else if (Physics.Raycast(transform.position + Vector3.forward * lenght + Vector3.right * widht, -transform.up, out raycastInfo, height, floorMask))
            GroundOneShot(raycastInfo.point.y);
        else
            isGrounded = false;
    }

    void InGroundOneShot(float y)
    {
        if (!isGrounded)
        {
            timer = 1;

            VelY = 0;
            //DebugCustom.Log("Gravity", "Gravity", "false");
            isGrounded = true;

            if (isFalling)
            {
                FallingEnd?.Invoke();
                isFalling = false;
            }
        }
        lastY = y;
    }
    [HideInInspector] public bool dontMultiply;
    float timerToSnap;
    public Vector3 SnapToGround(Transform transformToSnap)
    {
        dontMultiply = true;
        Vector3 forward = new Vector3(transform.forward.x, VelY, transform.forward.z);
        if (!isGrounded) return forward;
        if (dontSnap)
        {
            timerToSnap += Time.deltaTime;

            if (timerToSnap >= 1.5f)
            {
                dontSnap = false;
                timerToSnap = 0;
            }

            return forward;
        }

        groundAngle = Vector3.Angle(raycastInfo.normal, transformToSnap.forward);
        if (groundAngle >= maxGroundAngle) return forward;

        forward = Vector3.Cross(raycastInfo.normal, -transformToSnap.right);
        dontMultiply = false;

        return forward;
    }

    public void AddForce(float force)
    {
        VelY += force;
        dontSnap = true;
        timerToSnap = 0;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + -transform.up * height);
        Gizmos.DrawLine(transform.position + transform.right * widht, transform.position + transform.right * widht + ( -transform.up * height));
        Gizmos.DrawLine(transform.position - transform.right * widht, transform.position - transform.right * widht + ( -transform.up * height));
        Gizmos.DrawLine(transform.position + transform.forward * lenght, transform.position + transform.forward * lenght + (-transform.up * height));
        Gizmos.DrawLine(transform.position - transform.forward * lenght, transform.position - transform.forward * lenght + ( -transform.up * height));
    }

}
