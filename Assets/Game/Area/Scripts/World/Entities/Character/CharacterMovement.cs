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

        Move(axis * speed, velZ);

        movX = axis;
    }

    void LeftVertical(float axis)
    {
        float velX = _rb.velocity.x;

        Move(velX, axis * speed);

        movY = axis;
    }

    void Move(float axisX, float axisY)
    {

        float velY = _rb.velocity.y;

        Vector3 auxNormalized = new Vector3(axisX, velY, axisY);
        auxNormalized.Normalize();

        _rb.velocity = new Vector3(auxNormalized.x * speed, velY, auxNormalized.z * speed );

        var prom = Mathf.Abs(axisY) + Mathf.Abs(axisX);

        if (rotX >= 0.3 || rotX <= -0.3 || rotY >= 0.3 || rotY <= -0.3)
        {
            Rotation(rotY, rotX);

            //float dotX = Vector3.Dot(rotTransform.forward, new Vector3(rotY, 0, rotX));
            //float dotY = Vector3.Dot(rotTransform.right, new Vector3(rotY, 0, rotX));
            anim.Move(prom, -axisX * rotTransform.right.x, axisY * rotTransform.forward.z);

            //if (rotY >= 0)
            //    anim.Move(prom, axisX, axisY);
            //else
            //    anim.Move(prom, axisX, -axisY);
        }
        else
        {
            Rotation(axisY, axisX);
            anim.Move(prom, 0, 1);
        }

    }
    #endregion

    #region ROTATION
    //Joystick Derecho, Rotacion
    void RightHorizontal(float axis)
    {
        rotX = axis;
        Rotation(rotTransform.forward.z, axis);
    }
    public void EnableRotation() => canRotate = true;
    public void CancelRotation() => canRotate = false;

    void RightVerical(float axis)
    {
        rotY = axis;
        Rotation(axis, rotTransform.forward.x);
    }
    void Rotation(float axisX, float axisY)
    {
        if (canRotate)
        {
            Vector3 dir = rotTransform.forward + new Vector3(axisY, 0, axisX);

            if (dir == Vector3.zero)
                rotTransform.forward = new Vector3(axisY, 0, axisX);
            else
                dir = new Vector3(axisY, 0, axisX);

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
