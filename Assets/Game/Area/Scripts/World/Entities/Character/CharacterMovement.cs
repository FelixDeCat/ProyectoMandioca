using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class CharacterMovement
{
    [SerializeField] float speed = 4.5f;
    float currentSpeed;
    float rotX;
    float rotY;
    float movX;
    float movY;
    bool canRotate = true;

    Rigidbody _rb;
    Transform rotTransform;

    [SerializeField] float maxTimerDash = 0.5f;
    [SerializeField] float rollCD = 0.1f;
    [SerializeField] float rollSpeed = 8;
    float currentTimerDash = 0.5f;
    float timerDash;
    float currentCD;
    float cdTimer;
    float currentVelY;
    float currentDashSpeed;
    private Vector3 dashDir;
    public bool InDash { get; private set; }
    bool dashCdOk;
    [SerializeField] float _DMGMultiplier = 2;
    public float fallMaxDistance = 10;
    float lastYPos;

    CharacterGroundSensor isGrounded;
    CharacterAnimator anim;

    Action OnBeginRollFeedback_Callback;
    Action OnEndRollFeedback_Callback;
    Action EndRoll;
    public Action Dash;

    public Action<float> MovementHorizontal;
    public Action<float> MovementVertical;
    public Action<float> RotateHorizontal;
    public Action<float> RotateVertical;
    public Action Moving = delegate { };
    CharFeedbacks feedbacks;
    [SerializeField] Transform myCamera = null;

    [SerializeField] float bashDashDistance = 1;
    [SerializeField] float bashDashSpeed = 90;
    [SerializeField] float bashDashCD = 2;

    [SerializeField, Range(0, 5)] float jumpForce = 5;
    [SerializeField] float jumpSpeed = 9;
    [SerializeField] float jumpCD = 1;
    [SerializeField] float jumpTime = 0.5f;
    [SerializeField] LayerMask obstacleMask = 3 << 0 & 15 & 21;

    float original_angular_drag = 0.05f;
    const float snorlax_angular_drag = 1000f;
    float groundedPrecautionTimer;

    public float GetDefaultSpeed => speed;

    public void SnorlaxateCharacter(bool val) => _rb.angularDrag = val ? snorlax_angular_drag : original_angular_drag;

    public void Initialize(Rigidbody rb, Transform rot, CharacterAnimator a, CharFeedbacks _feedbacks, CharacterGroundSensor _isGrounded)
    {
        currentSpeed = speed;
        currentCD = rollCD;
        currentDashSpeed = jumpSpeed;
        currentTimerDash = maxTimerDash;
        _rb = rb;
        original_angular_drag = _rb.angularDrag;
        rotTransform = rot;
        anim = a;
        MovementHorizontal += LeftHorizontal;
        MovementVertical += LeftVertical;
        RotateHorizontal += RightHorizontal;
        RotateVertical += RightVerical;
        Dash += Roll;
        feedbacks = _feedbacks;
        isGrounded = _isGrounded;
        AudioManager.instance.GetSoundPool("DashSounds", AudioManager.SoundDimesion.TwoD, AudioGroups.GAME_FX);
        ActualizeDash(true);
        isGrounded.TurnOn();

        isGrounded.GroundOneShot += CalculateFallDamage;
    }

    #region BUILDER
    public CharacterMovement SetSpeed(float n = -1)
    {
        currentSpeed = n < 0 ? speed : n;
        return this;
    }
    public float GetSpeed() => speed;
    public CharacterMovement SetDashCD(float n = -1)
    {
        currentCD = n < 0 ? rollCD : n;
        return this;
    }
    public void SetCallbacks(Action _OnBeginRoll, Action _OnEndRoll)
    {
        OnBeginRollFeedback_Callback = _OnBeginRoll;
        OnEndRollFeedback_Callback = _OnEndRoll;
    }
    #endregion

    #region MOVEMENT
    //Joystick Izquierdo, Movimiento
    void LeftHorizontal(float axis)
    {
        movX = axis;
        Move();
    }

    void LeftVertical(float axis)
    {
        movY = axis;
        Move();
    }
    void Move()
    {
        Vector3 auxNormalized = Vector3.zero;

        float absVal = Mathf.Abs(movX) + Mathf.Abs(movY);

        if (absVal >= 1.3f) { movX = Mathf.Clamp(movX, -0.65f, 0.65f); movY = Mathf.Clamp(movY, -0.65f, 0.65f); absVal = 1.3f; }

        Vector3 right = Vector3.Cross(Vector3.up, myCamera.forward);
        Vector3 forward = Vector3.Cross(right, Vector3.up);

        auxNormalized += right.normalized * (movX * currentSpeed);
        auxNormalized += forward.normalized * (movY * currentSpeed);

        Rotation(auxNormalized.normalized.x, auxNormalized.normalized.z);
        Vector3 finalVel = Vector3.zero;

        finalVel = isGrounded.SnapToGround(rotTransform);

        if (!isGrounded.IsInGround)
        {
            groundedPrecautionTimer += Time.deltaTime;

            if(groundedPrecautionTimer >= 1)
                anim.Grounded(false);
        }
        else
        {
            groundedPrecautionTimer = 0;
            anim.Grounded(true);
        }

        if (!forcing && !addForce)
        {
            if (!isGrounded.dontMultiply) _rb.velocity = finalVel * currentSpeed * absVal;
            else _rb.velocity = new Vector3(auxNormalized.x, finalVel.y, auxNormalized.z);
        }

        if (currentSpeed <= 0)
        {
            anim.Move(0, 0);
            return;
        }

        if (movX >= 0.1 || movX <= -0.1 || movY >= 0.1 || movY <= -0.1)
        {
            var checkRay = Physics.Raycast(rotTransform.position + Vector3.up*1.2f, rotTransform.forward, 1.3f, obstacleMask,QueryTriggerInteraction.Ignore);
            
            if (checkRay)
            {
                currentVelY -= Time.deltaTime * 0.3f;
              
                if ( currentVelY <= 0.1)
                {
                    anim.Move(0, 0);
                    return;
                }
                anim.Move(0,currentVelY);
            }
            else
            {
                currentVelY = absVal;
                anim.Move(0, currentVelY);
            }
            Moving();
        }
        else
            anim.Move(0, 0);
    }

    #endregion
    #region Knockback y demás fuerzas
    bool forcing;
    bool addForce;
    float timerForce;
    public void MovementAddForce(Vector3 dir, float force, ForceMode mode)
    {
        if (!forcing)
        {
            _rb.velocity = Vector3.zero;
            forcing = true;
        }

        _rb.AddForce(dir * force, mode);
    }

    public void StopForceBool(bool move = false)
    {
        forcing = move;
    }

    public void StopForce()
    {
        _rb.velocity = new Vector3(0, _rb.velocity.y, 0);
    }

    public void AttackMovement(float moveForce)
    {
        if (forcing) return;
        _rb.AddForce(rotTransform.transform.forward * moveForce, ForceMode.VelocityChange);
    }

    public void AddForceToVelocity(Vector3 forceToApply)
    {
        ResetForce();
        addForce = true;
        _rb.AddForce(forceToApply, ForceMode.Impulse);
    }

    void ResetForce()
    {
        timerForce = 0;
        addForce = false;
        _rb.velocity = Vector3.zero;
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

    void Rotation(float x = 800, float z = 800)
    {
        if (canRotate)
        {
            Vector3 dir;

            if (x == 800 && z == 800)
            {
                Vector3 auxNormalized = Vector3.zero;

                Vector3 right = Vector3.Cross(Vector3.up, myCamera.forward);
                Vector3 forward = Vector3.Cross(right, Vector3.up);

                auxNormalized += right.normalized * rotX;
                auxNormalized += forward.normalized * rotY;

                x = auxNormalized.normalized.x;
                z = auxNormalized.normalized.z;
            }

            dir = new Vector3(x, 0, z);
            if (dir == Vector3.zero)
                dir = rotTransform.forward;

            rotTransform.forward = dir;
        }
    }

    public Transform GetTransformRotation() => rotTransform;

    #endregion

    #region ROLL
    public void ForcedCanDash(bool candash)
    {
        InDash = candash;
    }

    bool candamagefall = true;
    float timer_racall_damage_fall;
    public void StopDamageFall() { candamagefall = false; timer_racall_damage_fall = 0; }
    public bool ignoreFallDamage;
    void CalculateFallDamage(float y)
    {


        float totalFall = lastYPos - y;
        if (totalFall > fallMaxDistance)
        {
            float dmg = (totalFall - fallMaxDistance) * _DMGMultiplier;
            if (candamagefall && !ignoreFallDamage && !Main.instance.GetChar().godMode) Main.instance.GetChar().Life.Hit((int)dmg);
        }

        lastYPos = y;
        ignoreFallDamage = false;
    }

    public void OnUpdate()
    {
        #region el dash fisico mero mero
        if (InDash)
        {
            timerDash += Time.deltaTime;

            dashDir = new Vector3(dashDir.x, 0, dashDir.z);

            _rb.velocity = currentDashSpeed * dashDir + new Vector3(0, isGrounded.VelY, 0);

            if (timerDash >= currentTimerDash)
            {
                EndRoll();
            }
        }
        #endregion
        #region Dash cooldown
        if (dashCdOk)
        {
            cdTimer += Time.deltaTime;

            if (cdTimer >= currentCD)
            {
                dashCdOk = false;
                cdTimer = 0;
            }
        }
        #endregion

        if (!candamagefall)
        {
            timer_racall_damage_fall += Time.deltaTime;
            if (timer_racall_damage_fall >= 1)
            {
                timer_racall_damage_fall = 0;
                candamagefall = true;
            }
        }

        if (addForce)
        {
            timerForce += Time.deltaTime;

            if (timerForce >= 0.5f) ResetForce();
        }
    }

    public void StopRoll()
    {
        OnEndRollFeedback_Callback.Invoke();
        anim.Dash(false);
        InDash = false;
        _rb.velocity = Vector3.zero;
        timerDash = 0;
        dashDir = Vector3.zero;
        dashCdOk = true;
    }

    public void ANIM_EVENT_RollEnded()
    {
        StopRoll();
    }

    public void RollForAnim()
    {
        OnBeginRollFeedback_Callback();
        InDash = true;

        if (!Main.instance.GetChar().UpWeapons)
        {
            currentCD = jumpCD;
            isGrounded.AddForce(jumpForce);
            currentDashSpeed = jumpSpeed;
            currentTimerDash = jumpTime;
        }
        else
        {
            currentDashSpeed = rollSpeed;
            isGrounded.AddForce(jumpForce);
            currentCD = rollCD;
            currentTimerDash = maxTimerDash;
        }
    }

    public void Roll()
    {
        EndRoll = StopRoll;
        if (InDash) return;

        dashDir = rotTransform.forward;

        dashDir = new Vector3(dashDir.x, 0, dashDir.z);

        anim.Dash(true);
        feedbacks.sounds.Play_Dash();

        RollForAnim();
    }
    public bool CanUseDash { get; private set; }

    public void ActualizeDash(bool b) => CanUseDash = b;

    public bool InCD() => InDash || dashCdOk ? true : false;

    public Vector3 GetRotatorDirection() => rotTransform.forward;
    #endregion

    #region BASH DASH
    public void StartBashDash()
    {
        dashDir = rotTransform.forward;
        anim.BashDashAnim();
        OnBeginRollFeedback_Callback();
        InDash = true;
        currentDashSpeed = bashDashSpeed;
        currentTimerDash = bashDashDistance;
        EndRoll = StopBashDash;
    }

    public void StopBashDash()
    {
        OnEndRollFeedback_Callback();
        currentCD = bashDashCD;
        InDash = false;
        _rb.velocity = Vector3.zero;
        timerDash = 0;
        dashDir = Vector3.zero;
        dashCdOk = true;
    }
    
  
    #endregion
}
