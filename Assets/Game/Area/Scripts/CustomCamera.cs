using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Tools.Extensions;
using Tools.EventClasses;
using System;

public class CustomCamera : MonoBehaviour
{
    public Transform target;
    public Transform charTransform;
    private Vector3 velocity = Vector3.zero;
    public float smooth = 1f;
    public Vector3 offset;
    public bool lookAt;
    public float shakeAmmount;
    float original_shake_amount;
    private float shakeDurationCurrent;
    public float shakeDuration;
    private bool activeShake;
    public bool active = true;
    public float horizontalSpeed;
    public float verticalSpeed;
    [SerializeField] float _speedAwayOfMesh = 0.5f;
    public float horizontal;
    public float vertical;

    [SerializeField] float sensitivity;
    [SerializeField] private SkillCloseUp_Camera skillCloseUp_Camera = null;

    public float zoomDuration;
    const int FIELD_OF_VIEW_ORIGINAL = 60;
    float fieldOfView_toZoom = 60;

    PingPongLerp pingpongZoom = new PingPongLerp();

    public LayerMask _layermask_raycast_mask;
    //[SerializeField, Range(0, 1)] float fade = 0.05f;
    public float speedRot;
    Camera mycam;
    public Camera MyCam { get { return mycam; } }
    bool setOverTheSholder;
    public List<CamConfiguration> myCameras = new List<CamConfiguration>();
    public CamConfiguration overTheSholderCam;
    public bool activateOverTheSholder;
    public int index;
    float startHorizontal;
    float StartVertical;
    [SerializeField] RotateTheCamera _rotOfCamera = null;
    [SerializeField] CameraRotate cameraRotate = null;
    Transform cinematicCamParent = null;

    [SerializeField] EventInt invertAxis = null;


    private void Start()
    {
        //_joystick = new JoystickBasicInput();
        //_joystick.SUBSCRIBE_START(ChangeCamera);
        shakeDurationCurrent = shakeDuration;
        mycam = GetComponent<Camera>();
        pingpongZoom.Configure(Zoom, false);
        changeCameraconf(0);
        original_shake_amount = shakeAmmount;
        charTransform = Main.instance.GetChar().GetLookatPosition();
        lookAtTarget = Main.instance.GetChar().GetLookatPosition().GetComponent<SmoothToPos>();
        cinematicCamParent = myCameras[1].transform.parent;

        //skillCloseUp_Camera.SubscribeOnTurnOnCamera(CloseToCharacter);
        //skillCloseUp_Camera.SubscribeOnTurnOnCamera(ExitToCharacte);
    }

    #region Close Camera
    public void DoCloseCamera() => skillCloseUp_Camera.TurnOnSkillCamera();
    public void DoExitCamera() => skillCloseUp_Camera.TurnOffSkillCamera();
    void CloseToCharacter() { }
    void ExitToCharacte() { }
    #endregion

    private void Update()
    {
        if (!active)
            return;
        if (activateOverTheSholder)
        {
            OverTheSholder();
            return;
        }

        pingpongZoom.Updatear();
        if (!lookAt)
            transform.forward = Vector3.Slerp(transform.forward, myCameras[index].transform.forward, speedRot * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        if (!active || activateOverTheSholder)
            return;
        //BezierCinematic();
        MakeCinematic();
        if (!inCinematic) SmoothToTarget();
    }
    private void LateUpdate()
    {
        if (!active || activateOverTheSholder)
            return;
        if (activeShake) Shake();
    }

    public void DoFastZoom(float _speedanim, float _fieldOfViewToZoom = 55)
    {
        fieldOfView_toZoom = _fieldOfViewToZoom;
        pingpongZoom.Play(_speedanim);
    }
    void Zoom(float valtozoom) => mycam.fieldOfView = Mathf.Lerp(FIELD_OF_VIEW_ORIGINAL, fieldOfView_toZoom, valtozoom);
    void SmoothToTarget()
    {
        Vector3 desiredposition = target.position + offset;
        float axisX = Input.GetAxis("Horizontal");
        float axisZ = Input.GetAxis("Vertical");
        Vector3 moveOffset = desiredposition;
        //if (axisX != 0)
        //{
        //    moveOffset -= transform.right * axisX * sensitivity;
        //}
        //if (axisZ != 0)
        //{
        //    moveOffset -= transform.up * axisZ * sensitivity;
        //}
        Vector3 smoothedposition = Vector3.Slerp(transform.position, moveOffset, smooth * Time.deltaTime);
        transform.position = smoothedposition;
        if (lookAt) transform.LookAt(lookAtTarget.transform);
    }
    public void InstantPosition()
    {
        transform.position = target.position + offset;
    }

    public void BeginShakeCamera(float shake = -1)
    {
        shakeAmmount = shake != -1 ? shake : original_shake_amount;
        activeShake = true;
        shakeDurationCurrent = shakeDuration;

    }
    private void Shake()
    {
        if (shakeDurationCurrent > 0)
        {
            transform.position += UnityEngine.Random.insideUnitSphere * shakeAmmount;
            shakeDurationCurrent -= Time.deltaTime;
        }
        else
        {
            shakeDurationCurrent = 0;
            activeShake = false;
        }
    }
    void SmoothDump() => transform.position = Vector3.SmoothDamp(transform.position, target.position, ref velocity, smooth);


    public void NextCamera()
    {
        index = index.NextIndex(myCameras.Count);
        changeCameraconf(index);
        invertAxis.Invoke(index);
    }
    public void PrevCamera()
    {
        index = index.BackIndex(myCameras.Count);
        changeCameraconf(index);
        invertAxis.Invoke(index);
    }

    //private void nextIndex()
    //{
    //    //cambiar lo de poner el input. es solo para probar
    //    if (Input.GetKeyDown(KeyCode.C))
    //    {
    //        if (index < myCameras.Count - 1)
    //            index++;
    //        else
    //            index = 0;
    //        changeCameraconf(index);
    //    }
    //}

    void ChangeCamera()
    {
        if (index < myCameras.Count - 1)
            index++;
        else
            index = 0;
        changeCameraconf(index);
    }

    void changeCameraconf(int i)
    {
        target = myCameras[i].transform;
        shakeAmmount = myCameras[i].shakeAmmount;
        shakeDuration = myCameras[i].shakeDuration;

        Camera camera = GetComponent<Camera>();
        camera.cullingMask = myCameras[i].CullingMask;
        camera.fieldOfView = myCameras[i].fieldOfView;
    }

    public void ChangeToDefaultCamera()
    {
        active = true;
        index = 0;
        changeCameraconf(index);
        invertAxis.Invoke(index);
        if (!lookAt)
            transform.forward = Vector3.Lerp(transform.forward, myCameras[index].transform.forward, speedRot * Time.deltaTime);
    }
    void OverTheSholder()
    {
        if (!setOverTheSholder)
        {
            target = overTheSholderCam.transform;
            shakeAmmount = overTheSholderCam.shakeAmmount;
            shakeDuration = overTheSholderCam.shakeDuration;

            Camera camera = GetComponent<Camera>();
            camera.cullingMask = overTheSholderCam.CullingMask;
            camera.fieldOfView = overTheSholderCam.fieldOfView;
            setOverTheSholder = true;
            transform.forward = overTheSholderCam.transform.forward;
            horizontal = overTheSholderCam.transform.parent.eulerAngles.y;
            vertical = overTheSholderCam.transform.parent.eulerAngles.x;
            Debug.Log(overTheSholderCam.transform.parent.eulerAngles.y);
            startHorizontal = horizontal;
            StartVertical = vertical;
        }

        transform.position = Vector3.Lerp(transform.position, overTheSholderCam.transform.position, Time.deltaTime * smooth);

        horizontal += horizontalSpeed * Input.GetAxis("Horizontal");
        horizontal = Mathf.Clamp(horizontal, (startHorizontal - 45), (startHorizontal + 45));
        vertical += verticalSpeed * Input.GetAxis("Vertical");
        vertical = Mathf.Clamp(vertical, (StartVertical - 45), (StartVertical + 45));

        transform.rotation = Quaternion.Euler(-vertical, horizontal, 0);

    }

    public void inputToOverTheSholder(bool active)
    {

        if (active)
        {
            activateOverTheSholder = false;
            setOverTheSholder = false;
            ChangeToDefaultCamera();
            //horizontal = 0;
            //vertical = 0;
        }
        else
        {
            activateOverTheSholder = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 21)
            _rotOfCamera.MovementFromMesh(_speedAwayOfMesh);
    }

    CameraCinematicState cameraState = CameraCinematicState.off;

    float timer = 0f;
    Vector3 startPos;
    float goTime;
    float cinematicTime;
    float returnTime;
    Transform finalPos;
    SmoothToPos lookAtTarget;
    Vector3 lookPos;
    public event Action OnFinishCinematicEvent;
    bool inCinematic;
    BezierPoint[] beziers;
    AnimationCurve moveCurveSmooth;
    AnimationCurve lookAtCurveSmooth;
    public void StartCinematic(float _goTime, float _cinematicTime, float _returnTime, AnimationCurve _moveCurve, AnimationCurve _lookAtCurve, Transform _finalPos, Transform _lookAt, Action callback = null)
    {
        goTime = _goTime;
        cinematicTime = _cinematicTime;
        returnTime = _returnTime;
        moveCurveSmooth = _moveCurve;
        lookAtCurveSmooth = _lookAtCurve;
        finalPos = _finalPos;
        lookPos = _lookAt.position;
        OnFinishCinematicEvent = callback;
        cameraState = CameraCinematicState.cameraGoing;
        Main.instance.GetChar().Pause();
        myCameras[1].transform.parent = null;
        startPos = transform.position;
        ChangeCamera();
        cameraRotate.forceStop = true;
        inCinematic = true;
        startLookAtPos = lookAtTarget.transform.position;
    }

    public void InstantCinematic(float _goTime, float _cinematicTime, float _returnTime, AnimationCurve _moveCurve, AnimationCurve _lookAtCurve, Transform _finalPos, Transform _lookAt, Action callback = null)
    {
        goTime = _goTime;
        cinematicTime = _cinematicTime;
        returnTime = _returnTime;
        moveCurveSmooth = _moveCurve;
        lookAtCurveSmooth = _lookAtCurve;
        finalPos = _finalPos;
        lookPos = _lookAt.position;
        OnFinishCinematicEvent = callback;
        cameraState = CameraCinematicState.inCinematic;
        Main.instance.GetChar().Pause();
        myCameras[1].transform.parent = null;
        startPos = transform.position;
        ChangeCamera();
        cameraRotate.forceStop = true;
        inCinematic = true;
        startLookAtPos = lookAtTarget.transform.position;
        transform.position = finalPos.position;
        lookAtTarget.transform.position = lookPos;
        transform.LookAt(lookAtTarget.transform);
    }

    public void CinematicOver()
    {
        if (cameraState == CameraCinematicState.off) return;

        timer = 0;
        cameraState = CameraCinematicState.cameraReturn;
    }

    public void CinematicInstantOver()
    {
        if (cameraState == CameraCinematicState.off) return;
        Debug.Log(myCameras[0].transform.position);
        lookAtTarget.transform.position = startLookAtPos;
        transform.position = myCameras[0].transform.position;
        timer = 0;
        cameraRotate.forceStop = false;
        myCameras[1].transform.parent = cinematicCamParent;
        ChangeCamera();
        cameraState = CameraCinematicState.off;
        inCinematic = false;

        OnFinishCinematicEvent?.Invoke();
        Main.instance.GetChar().Resume();
    }

    Vector3 startLookAtPos;
    Vector3 lookAtVelocity = Vector3.zero;
   
    void MakeCinematic()
    {
        if (cameraState == CameraCinematicState.off)
            return;
        else if (cameraState == CameraCinematicState.cameraGoing)
        {
            timer += Time.deltaTime;
            //myCameras[1].transform.position = Vector3.Lerp(startPos, finalPos.position, timer / goTime);
            transform.position = Vector3.Lerp(startPos, finalPos.position, moveCurveSmooth.Evaluate(timer / goTime));
            
            //Esto es para el smooth
            lookAtTarget.transform.position = Vector3.Lerp(startLookAtPos, lookPos, lookAtCurveSmooth.Evaluate(timer / goTime));
          
            //El calculo esta bien pero el SmoothToPos del lookAtPos hace que la camara gire rarito
            transform.LookAt(lookAtTarget.transform);

            if (timer> goTime)
            {
                timer = 0;
                cameraState = CameraCinematicState.inCinematic;
            }
        }
        else if (cameraState == CameraCinematicState.inCinematic)
        {
            timer += Time.deltaTime;

            lookAtTarget.transform.position = lookPos;

            if (timer > cinematicTime)
            {
                timer = 0;
                cameraState = CameraCinematicState.cameraReturn;
            }
        }
        else if (cameraState == CameraCinematicState.cameraReturn)
        {
            timer += Time.deltaTime;
            //myCameras[1].transform.position = Vector3.Lerp(finalPos.position, myCameras[0].transform.position, timer / returnTime);
            transform.position = Vector3.Slerp(finalPos.position, myCameras[0].transform.position, moveCurveSmooth.Evaluate(timer/returnTime));

            //Esto es para el smooth
            //Vector3 aux = Vector3.Lerp(lookPos, startLookAtPos, timer / returnTime);            
            lookAtTarget.transform.position = Vector3.Lerp(lookPos, startLookAtPos, lookAtCurveSmooth.Evaluate(timer/returnTime));
            transform.LookAt(lookAtTarget.transform);
            //Aca también

            if (timer>returnTime)
            {
                timer = 0;
                cameraRotate.forceStop = false;
                myCameras[1].transform.parent = cinematicCamParent;
                ChangeCamera();
                cameraState = CameraCinematicState.off;
                inCinematic = false;

                OnFinishCinematicEvent?.Invoke();
                Main.instance.GetChar().Resume();
            }
        }
    }
}

enum CameraCinematicState
{
    off,
    cameraGo,
    cameraGoing,
    inCinematic,
    cameraReturn
}
