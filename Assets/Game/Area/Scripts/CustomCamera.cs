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

    [SerializeField] private SkillCloseUp_Camera skillCloseUp_Camera;

    public float zoomDuration;
    const int FIELD_OF_VIEW_ORIGINAL = 60;
    float fieldOfView_toZoom = 60;

    PingPongLerp pingpongZoom = new PingPongLerp();

    public LayerMask _layermask_raycast_mask;
    public float speedRot;
    Camera mycam;
    public List<CamConfiguration> myCameras = new List<CamConfiguration>();
    int index;
    private void Start()
    {
        shakeDurationCurrent = shakeDuration;
        mycam = GetComponent<Camera>();
        pingpongZoom.Configure(Zoom, false);
        changeCameraconf(0);

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
        pingpongZoom.Updatear();
        ShaderMask();
        nextIndex();
    }
    private void FixedUpdate()
    {
        SmoothToTarget();
    }
    private void LateUpdate()
    {
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
        Vector3 smoothedposition = Vector3.Lerp(transform.position, desiredposition, smooth * Time.deltaTime);
        transform.position = smoothedposition;
        if (lookAt) transform.LookAt(target);
    }
    void ShaderMask()
    {
        
        RaycastHit hit;
        var dir = Main.instance.GetChar().transform.position - this.transform.transform.position;
        dir.Normalize();
        if (Physics.Raycast(this.transform.transform.position, dir, out hit, 1000, _layermask_raycast_mask))
        {

            DebugCustom.Log("CameraThings", "Raycast Hit Element", hit.transform.gameObject.name);

            DebugCustom.Log("categoria de pepito", "elemento 1", 23);
            DebugCustom.Log("categoria de pepito", "elemento 12", 26);
            Main.instance.GetChar().Mask(!hit.transform.GetComponent<CharacterHead>());
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
    void SmoothDump() => transform.position = Vector3.SmoothDamp(transform.position, target.position, ref velocity, smooth);

    private void nextIndex()
    {
        //cambiar lo de poner el input. es solo para probar
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (index < myCameras.Count-1)
                index++;
            else
                index = 0;
            changeCameraconf(index);
        }
        transform.forward = Vector3.Lerp(transform.forward, myCameras[index].transform.forward, speedRot*Time.deltaTime);

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
    
    
}
