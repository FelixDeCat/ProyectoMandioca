using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CharacterMovement
{

    Rigidbody _rb;
    Transform rotTransform;
    float speed;

    float rotX;
    float rotY;
    float movX;
    float movY;
    private Vector3 dashDir;

    bool inDash;
    bool canRotate = true;

    float timerDash;
    float maxTimerDash;
    float dashSpeed;
    float dashCd;
    float cdTimer;
    float dashDecreaseSpeed;
    float dashMaxSpeed;
    bool dashCdOk;
    

    CharacterAnimator anim;

    Action OnBeginRoll;
    Action OnEndRoll;
    public Action Dash;

    public Action<float> MovementHorizontal;
    public Action<float> MovementVertical;
    public Action<float> RotateHorizontal;
    public Action<float> RotateVertical;

    private float _teleportDistance;
    private bool teleportActive;

    public bool TeleportActive
    {
        get => teleportActive;
        set { teleportActive = value; }
    }

    public CharacterMovement(Rigidbody rb, Transform rot, CharacterAnimator a)
    {
        _rb = rb;
        rotTransform = rot;
        anim = a;
        MovementHorizontal += LeftHorizontal;
        MovementVertical += LeftVertical;
        RotateHorizontal += RightHorizontal;
        RotateVertical += RightVerical;
        Dash += Roll;
    }

    #region BUILDER
    public CharacterMovement SetSpeed(float n)
    {
        speed = n;
        return this;
    }
    public CharacterMovement SetTimerDash(float n)
    {
        maxTimerDash = n;
        return this;
    }
    public CharacterMovement SetDashSpeed(float n)
    {
        dashMaxSpeed = n;
        return this;
    }
    public CharacterMovement SetDashCD(float n)
    {
        dashCd = n;
        return this;
    }
    public CharacterMovement SetRollDeceleration(float n)
    {
        dashDecreaseSpeed = n;
        return this;
    }
    public CharacterMovement SetPushAttack(float n)
    {
        pushForce = n;
        return this;
    }

    #endregion

    public void SetCallbacks(Action _OnBeginRoll, Action _OnEndRoll)
    {
        OnBeginRoll = _OnBeginRoll;
        OnEndRoll = _OnEndRoll;
    }

    #region MOVEMENT
    //Joystick Izquierdo, Movimiento
    void LeftHorizontal(float axis)
    {
        float velZ = _rb.velocity.z;

        movX = axis;
        Move();

    }

    void LeftVertical(float axis)
    {
        float velX = _rb.velocity.x;

        movY = axis;
        Move();

    }

    float pushForce; 
    Vector3 attackPushDir;
    Vector3 PushForward { get => rotTransform.transform.forward * pushForce; }
    public void FrontPushAttack()
    {

    }

    void Move()
    {

        float velY = _rb.velocity.y;

        Vector3 auxNormalized = new Vector3(movX, velY, movY);
        auxNormalized.Normalize();

        _rb.velocity = new Vector3(auxNormalized.x * speed, velY, auxNormalized.z * speed );

        var prom = Mathf.Abs(movY) + Mathf.Abs(movX);

        if (rotX >= 0.3 || rotX <= -0.3 || rotY >= 0.3 || rotY <= -0.3)
        {
            //Rotation(rotY, rotX);

            //float dotX = Vector3.Dot(rotTransform.forward, new Vector3(rotY, 0, rotX));
            //float dotY = Vector3.Dot(rotTransform.right, new Vector3(rotY, 0, rotX));
            if(Mathf.Abs(rotTransform.forward.z) >= Mathf.Abs(rotTransform.forward.x))
                anim.Move(prom, -movX * rotTransform.right.x, movY * rotTransform.forward.z);
            else
                anim.Move(prom, -movY * rotTransform.right.z, movX * rotTransform.forward.x);

            //if (rotY >= 0)
            //    anim.Move(prom, axisX, axisY);
            //else
            //    anim.Move(prom, axisX, -axisY);
        }
        else
        {
            anim.Move(prom, 0, 1);
        }

    }
    #endregion

    #region ROTATION
    //Joystick Derecho, Rotacion
    void RightHorizontal(float axis)
    {
        rotX = axis;
        Rotation();
    }
    public void EnableRotation() => canRotate = true;
    public void CancelRotation() => canRotate = false;

    void RightVerical(float axis)
    {
        rotY = axis;
        Rotation();
    }
    void Rotation()
    {
        if (canRotate)
        {
            Vector3 dir;
            if (rotX >= 0.3 || rotX <= -0.3 || rotY >= 0.3 || rotY <= -0.3)
            {
                dir = rotTransform.forward + new Vector3(rotX, 0, rotY);
            }
            else
            {
                dir = new Vector3(movX, 0, movY);
            }

            //if (dir == Vector3.zero)
            //    rotTransform.forward = new Vector3(rotY, 0, rotX);
            //else
            //    dir = new Vector3(movY, 0, movX);

            rotTransform.forward += dir;
        }
    }

    public Transform GetTransformRotation() => rotTransform;

    #endregion

    #region ROLL
    public void OnUpdate()
    {
        if (inDash)
        {
            timerDash += Time.deltaTime;

            if (timerDash / maxTimerDash >= 0.7f && dashSpeed != dashDecreaseSpeed)
            {
                _rb.velocity = Vector3.zero;
                dashSpeed = dashDecreaseSpeed;
            }

            _rb.velocity = dashDir * dashSpeed;

            if (timerDash >= maxTimerDash)
            {
                OnEndRoll.Invoke();
                inDash = false;
                _rb.velocity = Vector3.zero;
                timerDash = 0;
                dashDir = Vector3.zero;
            }
        }

        if (dashCdOk)
        {
            cdTimer += Time.deltaTime;

            if (cdTimer >= dashCd)
            {
                dashCdOk = false;
                cdTimer = 0;
            }
        }
    }

    public void RollForAnim()
    {
        OnBeginRoll();

        inDash = true;
        dashCdOk = true;


        dashSpeed = dashMaxSpeed;
    }

    public void Roll()
    {

        if (movX != 0 || movY != 0)
            dashDir = new Vector3(movX, 0, movY).normalized;
        else
            dashDir = rotTransform.forward;
        float dotX = Vector3.Dot(rotTransform.forward, dashDir);
        float dotY = Vector3.Dot(rotTransform.right, dashDir);
        //anim.SetVerticalRoll(dotX);
        //anim.SetHorizontalRoll(dotY);

        if (dotX >= 0.5f)
        {
            anim.SetVerticalRoll(1);
            anim.SetHorizontalRoll(0);
        }
        else if (dotX <= -0.5f)
        {
            anim.SetVerticalRoll(-1);
            anim.SetHorizontalRoll(0);
        }
        else
        {
            if (dotY >= 0.5f)
            {
                anim.SetVerticalRoll(0);
                anim.SetHorizontalRoll(1);
            }
            else if (dotY <= -0.5f)
            {
                anim.SetVerticalRoll(0);
                anim.SetHorizontalRoll(-1);
            }
        }

        anim.Roll();
    }

    public bool IsDash()
    {
        return inDash;
    }

    public bool InCD()
    {
        return dashCdOk;
    }

    public void Teleport()
    {
        inDash = true;
        dashCdOk = true;
        if (movX != 0 || movY != 0)
            dashDir = new Vector3(movX, 0, movY);
        else
            dashDir = rotTransform.forward;


        _rb.position = _rb .position + (dashDir * _teleportDistance);
    }
    
    public bool CheckIfCanTeleport()
    {
        RaycastHit hit;
        if (Physics.Raycast(_rb.position, dashDir, out hit, _teleportDistance, 1 << 20))
        {
            Debug.Log("Le pego a la pared invisible");
            return false;
        }
        else
        {
            Debug.Log("Puedo hacer teleport");
            return true;
        }
    }

    public Vector3 GetRotatorDirection()
    {
        return rotTransform.forward;
    }

    public Vector3 GetLookDirection()
    {
        if (movX != 0 || movY != 0)
            dashDir = new Vector3(movX, 0, movY);
        else
            dashDir = rotTransform.forward;
        
        return dashDir;
    }

    public void ConfigureTeleport(float teleportDistance)
    {
        _teleportDistance = teleportDistance;
    }

    #region SCRIPT TEMPORAL, BORRAR
    public void ChangeDashCD(float _cd)
    {
        dashCd = _cd;
    }
    #endregion

    #endregion


}
