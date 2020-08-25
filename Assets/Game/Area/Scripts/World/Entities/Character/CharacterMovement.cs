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
    [SerializeField] float dashCd = 0.1f;
    [SerializeField] float dashSpeed = 8;
    float currentTimerDash = 0.5f;
    float timerDash;
    float currentCD;
    float cdTimer;
    float currentDashSpeed;
    private Vector3 dashDir;
    public bool InDash { get; private set; }
    bool dashCdOk;
    bool _falling;
    float _DMGMultiplier;
    public void Set_DMGMultiplier(float multiplier)
    {
        _DMGMultiplier = multiplier;
    }
    int _fallDMG;
    float _currentTime;
    float _maxTimer;
    public void SetFallTimer(float _time)
    {
        _maxTimer = _time;
    }
    public void ForcedCanDash(bool candash)
    {
        InDash = candash;
    }

    float gravity = -7;
    Func<bool> isGrounded = delegate { return true; };
    float timer_gravity_curve;
    bool begin_gravityCurve;
    [SerializeField] AnimationCurve gravityCurve;
    float gravity_multiplier = 1f;

    CharacterAnimator anim;

    Action OnBeginRollFeedback_Callback;
    Action OnEndRollFeedback_Callback;
    Action EndRoll;
    public Action Dash;

    public Action<float> MovementHorizontal;
    public Action<float> MovementVertical;
    public Action<float> RotateHorizontal;
    public Action<float> RotateVertical;

    private ParticleSystem introTeleport_ps;
    private ParticleSystem outroTeleport_ps;
    private ParticleSystem endTeleport;
    private float _teleportDistance;
    private bool teleportActive;

    CharFeedbacks feedbacks;
    [SerializeField] Transform myCamera;

    [SerializeField] float bashDashDistance;
    [SerializeField] float bashDashSpeed;
    [SerializeField] float bashDashCD;

    public float GetDefaultSpeed => speed;
    public bool TeleportActive
    {
        get => teleportActive;
        set { teleportActive = value; }
    }
    

    public void Initialize(Rigidbody rb, Transform rot, CharacterAnimator a, CharFeedbacks _feedbacks, Func<bool> _isGrounded)
    {
        currentSpeed = speed;
        currentCD = dashCd;
        currentDashSpeed = dashSpeed;
        currentTimerDash = maxTimerDash;
        _rb = rb;
        rotTransform = rot;
        anim = a;
        MovementHorizontal += LeftHorizontal;
        MovementVertical += LeftVertical;
        RotateHorizontal += RightHorizontal;
        RotateVertical += RightVerical;
        Dash += Roll;
        feedbacks = _feedbacks;
        isGrounded = _isGrounded;
        AudioManager.instance.GetSoundPool("DashSounds", AudioGroups.GAME_FX);
        ActualizeDash(true);
    }

    #region BUILDER
    public CharacterMovement SetSpeed(float n = -1)
    {
        currentSpeed = n < 0 ? speed : n;
        return this;
    }
    public CharacterMovement SetDashCD(float n = -1)
    {
        currentCD = n < 0 ? dashCd : n;
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

    float velY = 0;

    void Move()
    {
        Vector3 auxNormalized = Vector3.zero;

        Vector3 right = Vector3.Cross(Vector3.up, myCamera.forward);
        Vector3 forward = Vector3.Cross(right, Vector3.up);

        auxNormalized += right.normalized * (movX * currentSpeed);
        auxNormalized += forward.normalized * (movY * currentSpeed);

        Rotation(auxNormalized.normalized.x, auxNormalized.normalized.z);

        if (!forcing)
            _rb.velocity = new Vector3(auxNormalized.x, velY, auxNormalized.z);

        if (currentSpeed <= 0)
        {
            anim.Move(0, 0);
            return;
        }

        if (movX >= 0.1 || movX <= -0.1 || movY >= 0.1 || movY <= -0.1)
            anim.Move(0, Mathf.Abs(movX) + Mathf.Abs(movY));
        else
            anim.Move(0, 0);

        //if (rotX >= 0.3 || rotX <= -0.3 || rotY >= 0.3 || rotY <= -0.3)
        //{

        //    if (movX <= 0.1 && movX >= -0.1 && movY <= 0.1 && movY >= -0.1)
        //        anim.Move(0, 0);
        //    else
        //    {
        //        if (Mathf.Abs(rotTransform.forward.z) >= Mathf.Abs(rotTransform.forward.x))
        //            anim.Move(-movX * rotTransform.right.x, movY * rotTransform.forward.z);
        //        else
        //            anim.Move(-movY * rotTransform.right.z, movX * rotTransform.forward.x);
        //    }
        //}
        //else
        //{
        //    if (movX >= 0.1 || movX <= -0.1 || movY >= 0.1 || movY <= -0.1)
        //        anim.Move(0, 1);
        //    else
        //        anim.Move(0, 0);
        //}

    }
    #endregion

    bool forcing;

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
        _rb.velocity = Vector3.zero;
    }

    public void AttackMovement(float moveForce)
    {
        if (forcing) return;
        _rb.AddForce(rotTransform.transform.forward * moveForce, ForceMode.VelocityChange);
    }

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
    public void OnUpdate()
    {
        #region toda la parte de calculos relacionados a la curva de gravedad
        if (begin_gravityCurve)
        {
            if (timer_gravity_curve < 1)
            {
                timer_gravity_curve = timer_gravity_curve + 1 * Time.deltaTime;
                gravity_multiplier = gravityCurve.Evaluate(timer_gravity_curve);
            }
            else
            {
                timer_gravity_curve = 0;
                begin_gravityCurve = false;
            }
        }

        velY = 0;
        if (!isGrounded.Invoke())
        {
            velY = gravity;

            if (begin_gravityCurve)
            {
                velY = gravity * gravity_multiplier;
            }
            else
            {
                velY = gravity;
            }
            _currentTime += Time.deltaTime;
            if (_currentTime >= _maxTimer)
            {
                _falling = true;
                _fallDMG = (int)((_currentTime * _DMGMultiplier) - _maxTimer);
            }
            DebugCustom.Log("Gravity", "Gravity", "TRUE");
        }
        else
        {
            velY = 0;
            if (_falling)
            {
                Main.instance.GetChar().Life.Hit(_fallDMG);
                
            }

            _fallDMG = 0;
            _currentTime = 0;
            _falling = false;
            DebugCustom.Log("Gravity", "Gravity", "false");
        }
        #endregion
        #region el dash fisico mero mero
        if (InDash)
        {
            timerDash += Time.deltaTime;

            dashDir = new Vector3(dashDir.x, 0, dashDir.z);

            _rb.velocity = currentDashSpeed * dashDir + new Vector3(0, velY, 0);

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
                if (endTeleport)
                {
                    endTeleport.transform.position = rotTransform.position;
                    endTeleport.Play();
                }
                dashCdOk = false;
                cdTimer = 0;
            }
        }
        #endregion

    }

    public void StopRoll()
    {
        currentCD = dashCd;
        OnEndRollFeedback_Callback.Invoke();
        anim.Dash(false);
        InDash = false;
        _rb.velocity = Vector3.zero;
        timerDash = 0;
        dashDir = Vector3.zero;
        dashCdOk = true;
        timer_gravity_curve = 0;
        begin_gravityCurve = false;
    }


    public void ANIM_EVENT_RollEnded()
    {
        //este es el que viene desde evento del animator
        //se ejecuta cuando la animacion terminó
        StopRoll();
    }

    public void RollForAnim()
    {
        OnBeginRollFeedback_Callback();
        begin_gravityCurve = true;
        InDash = true;
        currentDashSpeed = dashSpeed;
    }

    public void Roll()
    {
        EndRoll = StopRoll;
        if (InDash) return;

        dashDir = rotTransform.forward;

        float dotX = Vector3.Dot(rotTransform.forward, dashDir);
        float dotY = Vector3.Dot(rotTransform.right, dashDir);

        dashDir = new Vector3(dashDir.x, 0, dashDir.z);

        #region calculo para mandarselo al BlendTree
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
        #endregion

        anim.Dash(true);

        //feedback... luego ponerselo a un ANIM EVENT para que suene cuando tocas el suelo
        feedbacks.sounds.Play_Dash();

        RollForAnim();
    }
    public bool CanUseDash { get; private set; }

    public void ActualizeDash(bool b) => CanUseDash = b;

    public bool InCD() => InDash || dashCdOk ? true : false;

    //Pulir para que quede mas lindo
    public void Teleport()
    {
        RollForAnim();
        AudioManager.instance.PlaySound("TeleportAudio", rotTransform);
        introTeleport_ps.transform.position = _rb.position;
        introTeleport_ps.Play();

        InDash = true;
        dashCdOk = true;
        if (movX != 0 || movY != 0)
            dashDir = new Vector3(movX, 0, movY);
        else
            dashDir = rotTransform.forward;

        dashDir.Normalize();

        _rb.position = _rb.position + (dashDir * _teleportDistance);
        outroTeleport_ps.transform.position = _rb.position + dashDir * _teleportDistance - dashDir * 1.5f;
        outroTeleport_ps.Play();
    }

    //Puli esto Fran
    void BlockedTeleport(float distance)
    {
        RollForAnim();

        introTeleport_ps.transform.position = _rb.position;
        introTeleport_ps.Play();

        InDash = true;
        dashCdOk = true;

        _rb.position = _rb.position + (dashDir * distance);
        outroTeleport_ps.transform.position = _rb.position;
        outroTeleport_ps.Play();
    }

    //arreglar esto
    public bool CheckIfCanTeleport()
    {
        if (movX != 0 || movY != 0)
            dashDir = new Vector3(movX, 0, movY); //Actualizo direccion
        else
            dashDir = rotTransform.forward;

        var player = Main.instance.GetChar(); //con esto consigo el punto de donde sale el rayo
        RaycastHit hit;
        if (Physics.Raycast(player.rayPivot.position, dashDir, out hit, _teleportDistance * 2, 1 << 20))
        {
            var aux = Vector3.Distance(hit.point, _rb.position); //veo la distancia a donde pego el rayo
            Debug.Log(aux);
            if (aux <= 2f)//esto es para que no traspase la pared
            {
                BlockedTeleport(0);
                return false;
            }

            BlockedTeleport(aux - aux / 3); //lo tiro un poquito antes de la pared
            Debug.Log("Le pego a la pared invisible y me teleporto a ella");
            return false;
        }
        else
        {
            //Si llega aca, va todo normal
            Debug.Log("Puedo hacer teleport");
            return true;
        }
    }

    public Vector3 GetRotatorDirection() => rotTransform.forward;

    public void ConfigureTeleport(float teleportDistance, ParticleSystem intro, ParticleSystem outro, ParticleSystem endCD)
    {
        introTeleport_ps = intro;
        outroTeleport_ps = outro;
        endTeleport = endCD;
        _teleportDistance = teleportDistance;
    }

    #endregion

    #region BASH DASH
    public void StartBashDash()
    {
        dashDir = rotTransform.forward;
        anim.BashDashAnim();
        OnBeginRollFeedback_Callback();
        begin_gravityCurve = true;
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
        timer_gravity_curve = 0;
        begin_gravityCurve = false;
    }

    #endregion
}
