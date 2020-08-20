using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DevelopTools.UI;

public class CameraRotate : MonoBehaviour
{
    [Header("General Settings")]
    [SerializeField] bool UseBezier = false;
    [SerializeField] public List<BezierPoint> bezierPoints;
    [SerializeField] float sliderTime = 1;

    //[SerializeField] float _speedToReturn;
    //[SerializeField] float _speedAwayFromMesh;
    [SerializeField] float minDistance = 2.5f;

    [Header("Horizontal")]
    [SerializeField] GameObject rotatorX;
    [SerializeField] float sensitivityHorizontal;

    float _distance;
    CharacterHead myChar;

    [SerializeField] LayerMask _mask;
    public bool colliding;
    float raycastDist;

    [Header("Vertical")]
    [SerializeField] float sensitivityVertical;
    [SerializeField] float clampYUp;
    [SerializeField] float clampYDown;
    Vector3 initialVector;

    [Header("DebugOptions")]
    [SerializeField] float minHorSens;
    [SerializeField] float maxHorSens;
    [SerializeField] float minVertSens;
    [SerializeField] float maxVertSens;

    [SerializeField] Transform camConfig;

    Vector3 offsetVec = new Vector3(0, 1f, 0);
    

    private void Start()
    {
        myChar = Main.instance.GetChar();

        raycastDist = Vector3.Distance(rotatorX.transform.position, myChar.transform.position + offsetVec);
     
        initialVector = transform.position - ( myChar.transform.position + offsetVec);
        initialVector.x = 0;

        Debug_UI_Tools.instance.CreateSlider("HorSens", sensitivityHorizontal, minHorSens, maxHorSens, ChangeSensitivityHor);
        Debug_UI_Tools.instance.CreateSlider("VertSens", sensitivityVertical, minVertSens, maxVertSens, ChangeSensitivityVer);
        Debug_UI_Tools.instance.CreateToogle("Invert Horizontal", false, InvertAxisHor);
        Debug_UI_Tools.instance.CreateToogle("Invert Vertical", false, InvertAxisVert);
    }

    private void Update()
    {
        if (!UseBezier)
        {
            RaycastHit hit;
            Vector3 direction = rotatorX.transform.position - (myChar.transform.position + offsetVec);
            if (Physics.Raycast(myChar.transform.position + offsetVec, direction, out hit, raycastDist, _mask))
            {
                if (hit.distance > minDistance)
                {
                    Vector3 dir = hit.point - direction.normalized;
                    camConfig.position = dir;
                    return;
                }
            }
            else if (Vector3.Distance(myChar.transform.position, camConfig.position) < raycastDist)
            {
                camConfig.transform.position = rotatorX.transform.position;
            }
        }
        else
        {
            rotatorX.transform.position = GetPointOnBezierCurve(bezierPoints[0], bezierPoints[1]);
            rotatorX.transform.rotation = bezierPoints[0].transform.rotation;
        }
    }

    string ChangeSensitivityHor(float val)
    {
        sensitivityHorizontal = val;
        return val.ToString();
    }
    string ChangeSensitivityVer(float val)
    {
        sensitivityVertical = val;
        return val.ToString();
    }
    int vertAxis = 1;
    string InvertAxisVert(bool val)
    {
        if (val) vertAxis = -1;
        else vertAxis = 1;
        return "";
    }
    int horAxis = 1;
    string InvertAxisHor(bool val)
    {
        if (val) horAxis = -1;
        else horAxis = 1;
        return "";
    }


    public void RotateHorizontal(float axis)
    {
        if (UseBezier) return;
        rotatorX.transform.RotateAround(myChar.transform.position, Vector3.up, axis * sensitivityHorizontal * Time.deltaTime * horAxis);
    }
    public void RotateVertical(float vertical)
    {
        if (UseBezier) return;
        float rotateDegrees = 0f;

        rotateDegrees += vertical * vertAxis;
        
        Vector3 currentVector = transform.position - (myChar.transform.position + offsetVec);
        currentVector.x = 0;
        float angleBetween = Vector3.Angle(initialVector, currentVector) * (Vector3.Cross(initialVector, currentVector).x > 0 ? 1 : -1);
        float newAngle = Mathf.Clamp(angleBetween + rotateDegrees, -clampYDown, clampYUp);
        rotateDegrees = newAngle - angleBetween;

        transform.RotateAround(myChar.transform.position, Vector3.right, rotateDegrees * sensitivityVertical * Time.deltaTime);

        rotatorX.transform.RotateAround(myChar.transform.position, rotatorX.transform.right, rotateDegrees * sensitivityVertical * Time.deltaTime);
    }
       
    public void RotateHorizontalByBezier(float axis)
    {
        if (!UseBezier) return;
        foreach (var item in bezierPoints)
        {
            item.transform.RotateAround(myChar.transform.position, Vector3.up, axis * sensitivityHorizontal * Time.deltaTime * horAxis);
        }
    }

    public void RotateVerticalByBezier(float vertical)
    {
        if (!UseBezier) return;
        sliderTime = Mathf.Clamp(sliderTime + vertical /100, 0, 1);
    }

    Vector3 GetPointOnBezierCurve(BezierPoint ini, BezierPoint final)
    {
        Vector3 a = Vector3.Lerp(ini.transform.position, ini.son.transform.position, sliderTime);
        Vector3 b = Vector3.Lerp(ini.son.transform.position, final.son.transform.position, sliderTime);
        Vector3 c = Vector3.Lerp(final.son.transform.position, final.transform.position, sliderTime);

        Vector3 d = Vector3.Lerp(a, b, sliderTime);
        Vector3 e = Vector3.Lerp(b, c, sliderTime);
        Vector3 pointOnCurve = Vector3.Lerp(d, e, sliderTime);

        return pointOnCurve;
    }
}
