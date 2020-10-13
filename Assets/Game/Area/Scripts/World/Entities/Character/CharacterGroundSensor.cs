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
    [SerializeField] float maxhHeight = 1f;

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

                DebugCustom.Log("Gravity", "Gravity", "TRUE");

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
        RaycastHit hit;

        if (Physics.Raycast(transform.position, -transform.up, out hit, height, floorMask))
            GroundOneShot(hit.point.y);
        else if(Physics.Raycast(transform.position + Vector3.right * widht, -transform.up, out hit, height, floorMask))
            GroundOneShot(hit.point.y);
        else if (Physics.Raycast(transform.position + Vector3.left * widht, -transform.up, out hit, height, floorMask))
            GroundOneShot(hit.point.y);
        else if (Physics.Raycast(transform.position + Vector3.forward * lenght, -transform.up, out hit, height, floorMask))
            GroundOneShot(hit.point.y);
        else if (Physics.Raycast(transform.position + Vector3.back * lenght, -transform.up, out hit, height, floorMask))
            GroundOneShot(hit.point.y);
        else if (Physics.Raycast(transform.position + Vector3.back * lenght + Vector3.right * widht, -transform.up, out hit, height, floorMask))
            GroundOneShot(hit.point.y);
        else if (Physics.Raycast(transform.position + Vector3.back * lenght + Vector3.left * widht, -transform.up, out hit, height, floorMask))
            GroundOneShot(hit.point.y);
        else if (Physics.Raycast(transform.position + Vector3.forward * lenght + Vector3.left * widht, -transform.up, out hit, height, floorMask))
            GroundOneShot(hit.point.y);
        else if (Physics.Raycast(transform.position + Vector3.forward * lenght + Vector3.right * widht, -transform.up, out hit, height, floorMask))
            GroundOneShot(hit.point.y);
        else
            isGrounded = false;
    }

    void InGroundOneShot(float y)
    {
        if (!isGrounded)
        {
            timer = 1;

            VelY = 0;
            DebugCustom.Log("Gravity", "Gravity", "false");
            isGrounded = true;

            if (isFalling)
            {
                FallingEnd?.Invoke();
                isFalling = false;
            }
        }
        lastY = y;
    }

    float timerToSnap;
    public void SnapToGround(Transform transformToSnap)
    {
        if (dontSnap)
        {
            timerToSnap += Time.deltaTime;

            if (timerToSnap >= 1.5f)
            {
                dontSnap = false;
                timerToSnap = 0;
            }

            return;
        }

        RaycastHit hit;
        if (!Slope(transformToSnap))
        {
            if (Physics.Raycast(transform.position, -transform.up, out hit, 0.3f, floorMask))
            {
                if (Mathf.Abs(hit.point.y - transformToSnap.position.y) <= 0.05f) return;

                transformToSnap.position = new Vector3(transformToSnap.position.x, hit.point.y, transformToSnap.position.z);
            }
        }
    }

    bool Slope(Transform transformToSnap)
    {
        return false; //Para que no rompa para mañana

        RaycastHit hit;
        if (Physics.Raycast(transform.position + transform.forward * 0.35f + transform.up * 0.35f, -transform.up, out hit, 0.4f, floorMask))
        {
            var temp = Mathf.Abs(hit.point.y - transformToSnap.position.y);
            if (hit.point.y <= transformToSnap.position.y || temp <= 0.05f || temp >=maxhHeight) return false;

            transformToSnap.position = new Vector3(transformToSnap.position.x, hit.point.y, transformToSnap.position.z);

            Debug.Log("stoy slopeando: " + temp);
            return true;
        }
        return false;
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
