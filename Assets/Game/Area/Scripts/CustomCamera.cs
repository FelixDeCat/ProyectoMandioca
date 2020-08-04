using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Tools.Extensions;
using Tools.EventClasses;

public class CustomCamera : MonoBehaviour
{
    public Transform target;
    private Vector3 velocity = Vector3.zero;
    public float smooth = 1f;
    public Vector3 offset;
    public bool lookAt;
    public float shakeAmmount;
    float original_shake_amount;
    private float shakeDurationCurrent;
    public float shakeDuration;
    private bool activeShake;
    public bool active=true;
    Collider currentObstacle;
    [SerializeField] float sensitivity;
    [SerializeField] private SkillCloseUp_Camera skillCloseUp_Camera = null;

    public float zoomDuration;
    const int FIELD_OF_VIEW_ORIGINAL = 60;
    float fieldOfView_toZoom = 60;

    PingPongLerp pingpongZoom = new PingPongLerp();

    public LayerMask _layermask_raycast_mask;
    [SerializeField, Range(0,1)] float fade = 0.05f;
    public float speedRot;
    Camera mycam;
    public List<CamConfiguration> myCameras = new List<CamConfiguration>();
   public int index;
    JoystickBasicInput _joystick;

    [SerializeField] EventInt invertAxis;

    private void Start()
    {
        _joystick = new JoystickBasicInput();
        _joystick.SUBSCRIBE_START(ChangeCamera);
        shakeDurationCurrent = shakeDuration;
        mycam = GetComponent<Camera>();
        pingpongZoom.Configure(Zoom, false);
        changeCameraconf(0);
        original_shake_amount = shakeAmmount;
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
        pingpongZoom.Updatear();
        ShaderMask();
        transform.forward = Vector3.Lerp(transform.forward, myCameras[index].transform.forward, speedRot * Time.deltaTime);
        _joystick.Refresh();
        if (Input.GetKeyDown(KeyCode.C))
        {
            NextCamera();
        }
    }
    private void FixedUpdate()
    {
        if (!active)
            return;
        SmoothToTarget();
    }
    private void LateUpdate()
    {
        if (!active)
            return;
        if (activeShake) Shake();
    }

    public void DoFastZoom(float _speedanim, float _fieldOfViewToZoom = 55)
    {
        fieldOfView_toZoom = _fieldOfViewToZoom;
        pingpongZoom.Play(_speedanim);
    }
    void Zoom(float valtozoom) => mycam.fieldOfView = Mathf.Lerp(FIELD_OF_VIEW_ORIGINAL, fieldOfView_toZoom, valtozoom);
    void SmoothToTarget() {
        Vector3 desiredposition = target.position + offset;
        float axisX = Input.GetAxis("Horizontal");
        float axisZ = Input.GetAxis("Vertical");
        Vector3 moveOffset = desiredposition;
        if (axisX != 0)
        {
            moveOffset += transform.right * axisX * sensitivity;
        }
        if (axisZ != 0)
        {
            moveOffset += transform.up * axisZ * sensitivity;
        }
        Vector3 smoothedposition = Vector3.Lerp(transform.position, moveOffset, smooth * Time.deltaTime);
        transform.position = smoothedposition;
        if (lookAt) transform.LookAt(target);
    }
    public void InstantPosition()
    {
        transform.position = target.position + offset;
    }
    //void ShaderMask()
    //{
        
    //    RaycastHit hit;
    //    var dir = Main.instance.GetChar().transform.position - this.transform.transform.position;
    //    dir.Normalize();
    //    if (Physics.Raycast(this.transform.transform.position, dir, out hit, 10000, _layermask_raycast_mask, QueryTriggerInteraction.Ignore))
    //    {
    //        if (hit.transform.GetComponent<MeshRenderer>())
    //        {
    //            if (currentObstacle != hit.collider)
    //            {
    //                currentObstacle?.GetComponent<MeshRenderer>().material.SetFloat("_Intensity", 1);
    //                currentObstacle = hit.collider;
    //                StartCoroutine(ShaderFade());
    //            }
    //        }
    //        else
    //        {
    //            currentObstacle?.GetComponent<MeshRenderer>()?.material.SetFloat("_Intensity", 1);
    //            currentObstacle = null;
    //            StopCoroutine(ShaderFade());
    //        }
    //        DebugCustom.Log("CameraThings", "Raycast Hit Element", hit.transform.gameObject.name);
    //        Main.instance.GetChar().Mask(!hit.transform.GetComponent<CharacterHead>());

    //    }
    //    else
    //    {
    //        currentObstacle?.GetComponent<MeshRenderer>().material.SetFloat("_Intensity", 1);
    //        currentObstacle = null;
    //        StopCoroutine(ShaderFade());
    //    }    
    //}


    IEnumerator ShaderFade()
    {
        float currentFade = 1f;

        while (true)
        {
            currentFade -= 0.1f;
            yield return new WaitForSeconds(fade);
            if (currentObstacle && currentFade > 0.3f) currentObstacle.GetComponent<MeshRenderer>().material.SetFloat("_Intensity", currentFade);
            else break;
        }
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
            transform.position += Random.insideUnitSphere * shakeAmmount;
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
        smooth = myCameras[i].smoothTime;
       
        Camera camera = GetComponent<Camera>();
        camera.cullingMask = myCameras[i].CullingMask;
        camera.fieldOfView = myCameras[i].fieldOfView;
    }
    
    public void ChangeToDefaultCamera()
    {
        active = true;
        index = 0;
        changeCameraconf(index);
        transform.forward = Vector3.Lerp(transform.forward, myCameras[index].transform.forward, speedRot * Time.deltaTime);
    }
}
