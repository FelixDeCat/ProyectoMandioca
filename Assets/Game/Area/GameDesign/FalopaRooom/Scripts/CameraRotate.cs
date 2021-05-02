using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DevelopTools.UI;
using Tools.Extensions;

public class CameraRotate : MonoBehaviour
{
    [Header("General Settings")]
    [SerializeField] bool UseBezier = false;
    [SerializeField] bool IgnoreCollisionsBezier = true;

    [SerializeField] public List<BezierPoint> bezierPoints = new List<BezierPoint>();
    [SerializeField] float sliderTime = 1;

    [SerializeField] Transform lookAtTrans = null;

    [SerializeField] float minDistance = 2.5f;

    [SerializeField] Transform posToCheckpoint = null;

    [Header("Horizontal")]
    [SerializeField] GameObject rotatorX = null;
    [SerializeField] float sensitivityHorizontal = 0.5f;
    CharacterHead myChar;

    [SerializeField] LayerMask _mask = 0 << 21;
    public bool colliding;
    float raycastDist;

    [Header("Vertical")]
    [SerializeField] float sensitivityVertical = 0.5f;
    [SerializeField] float clampYUp = 0.1f;
    [SerializeField] float clampYDown = 0;
    Vector3 initialVector;

    [Header("Vertical Tilt")]
    [SerializeField] float maxHeight;
    [SerializeField] float minHeight;
    [SerializeField] float comebackSpeed;
    bool isTilting;
    float tiltLerp;

    [SerializeField] Transform camConfig = null;

    [SerializeField] Vector3 offsetVec = new Vector3(0, 2f, 0);
    float zoomDist;

    [HideInInspector] public bool forceStop = false;

    private void Start()
    {
        myChar = Main.instance.GetChar();

        raycastDist = Vector3.Distance(rotatorX.transform.position, myChar.transform.position + offsetVec);
        zoomDist = raycastDist;

        initialVector = transform.position - (myChar.transform.position + offsetVec);
        initialVector.x = 0;
    }

    float prevDist = 0f;

    private void Update()
    {
        if (!isTilting && Mathf.Abs(tiltLerp) > 0.1f)
        {
            if (tiltLerp > 0)
                tiltLerp -= Time.deltaTime * comebackSpeed;
            else if (tiltLerp < 0)
                tiltLerp += Time.deltaTime * comebackSpeed;
        }
    }


    private void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Y)) { Cursor.lockState = CursorLockMode.Locked; Cursor.visible = false; }
        if (Input.GetKeyDown(KeyCode.U)) { Cursor.lockState = CursorLockMode.None; Cursor.visible = true; }

        RaycastHit hit;
        Vector3 direction;

        if (!UseBezier)
        {
            direction = rotatorX.transform.position - (myChar.transform.position + offsetVec);
            if (Physics.Raycast(myChar.transform.position + offsetVec, direction, out hit, raycastDist, _mask))
            {
                colliding = true;
                if (hit.distance > minDistance)
                {
                    Vector3 dir = hit.point - direction.normalized;
                    camConfig.position = dir;
                    return;
                }
            }
            else if (Vector3.Distance(myChar.transform.position, camConfig.position) < raycastDist)
            {
                colliding = false;
                camConfig.transform.position = rotatorX.transform.position;
            }
        }
        else
        {
            direction = Extensions.GetPointOnBezierCurve(bezierPoints[0], bezierPoints[1], sliderTime) - (myChar.transform.position + offsetVec);
            float dist = direction.magnitude;
            if (!IgnoreCollisionsBezier && Physics.Raycast(myChar.transform.position + offsetVec, direction, out hit, dist, _mask))
            {
                colliding = true;
                float distance = hit.distance;
                if (distance > minDistance)
                {
                    Vector3 dir = hit.point - direction.normalized;
                    camConfig.position = dir;
                    camConfig.transform.rotation = bezierPoints[0].transform.rotation;
                }
                else if (distance <= minDistance && prevDist < distance)
                {
                    Vector3 dir = hit.point - direction.normalized;
                    camConfig.position = dir;
                    camConfig.transform.rotation = bezierPoints[0].transform.rotation;
                    prevDist = hit.distance;
                }
            }
            else
            {
                colliding = false;
                camConfig.transform.position = Extensions.GetPointOnBezierCurve(bezierPoints[0], bezierPoints[1], sliderTime);
                camConfig.transform.rotation = bezierPoints[0].transform.rotation;
            }
        }

        Vector3 newPos;

        if (tiltLerp > 0)
        {
            newPos = Vector3.Lerp(myChar.transform.position + offsetVec, myChar.transform.position + offsetVec + new Vector3(0, maxHeight, 0), tiltLerp);
            lookAtTrans.transform.position = new Vector3(lookAtTrans.transform.position.x, newPos.y, lookAtTrans.transform.position.z);
        }
        else
        {
            newPos = Vector3.Lerp(myChar.transform.position + offsetVec, myChar.transform.position + offsetVec - new Vector3(0, maxHeight, 0), Mathf.Abs(tiltLerp));
            lookAtTrans.transform.position = new Vector3(lookAtTrans.transform.position.x, newPos.y, lookAtTrans.transform.position.z);
        }

        Vector3 newDir;
        RaycastHit hitnew;
        newDir = Vector3.Lerp(bezierPoints[1].transform.position, bezierPoints[0].transform.position, 0.5f + tiltLerp / 2f) - lookAtTrans.position;

        if (Physics.Raycast(lookAtTrans.position , newDir, out hitnew, newDir.magnitude, _mask))
        {
            colliding = true;

            Vector3 distToChar = lookAtTrans.position - myChar.transform.position;
            distToChar.y = 0;
            Vector3 distToPoint = lookAtTrans.position - hitnew.point;
            distToPoint.y = 0;

            if (distToChar.magnitude > distToPoint.magnitude) return;

            if (hitnew.distance > minDistance )
            {
                Vector3 dir = hitnew.point - newDir.normalized;
                camConfig.position = dir;
                camConfig.transform.rotation = bezierPoints[0].transform.rotation;
                return;
            }
        }

        colliding = false;

        camConfig.transform.rotation = bezierPoints[0].transform.rotation;
        camConfig.transform.position = Vector3.Lerp(bezierPoints[1].transform.position, bezierPoints[0].transform.position, 0.5f + tiltLerp / 2f);


    }
    float timer;
    public string ChangeSensitivityHor(float val)
    {
        sensitivityHorizontal = val;
        return val.ToString();
    }
    public string ChangeSensitivityVer(float val)
    {
        sensitivityVertical = val;
        return val.ToString();
    }
    int vertAxis = 1;
    public string InvertAxisVert(bool val)
    {
        if (val) vertAxis = -1;
        else vertAxis = 1;
        return "";
    }
    int horAxis = 1;
    public string InvertAxisHor(bool val)
    {
        if (val) horAxis = -1;
        else horAxis = 1;

        return "";
    }

    public void MoveCamBehindChar()
    {
        var dir = Main.instance.GetChar().GetCharMove().GetRotatorDirection();
        Vector3 dirAux = rotatorX.transform.position - (myChar.transform.position + offsetVec);
        Debug.Log(dirAux);
        //rotatorX.transform.position = (myChar.transform.position + offsetVec) -dir * dirAux.x;
    }

    public Vector2 zoomLimits;
    public void Zoom(float axis)
    {
        if (UseBezier || colliding) return;
        Vector3 dir = rotatorX.transform.position - (myChar.transform.position + offsetVec);
        Vector3 aux = rotatorX.transform.position + dir * axis;
        float dist = Vector3.Distance(aux, myChar.transform.position + offsetVec);
        if (dist < zoomLimits.x || dist > zoomLimits.y) return;

        rotatorX.transform.position += dir * axis;
    }

    public void RotateHorizontal(float axis)
    {
        if (UseBezier || forceStop) return;
        rotatorX.transform.RotateAround(myChar.transform.position, Vector3.up, axis * sensitivityHorizontal * Time.deltaTime * horAxis);
        //lookAtTrans.transform.RotateAround(myChar.transform.position, Vector3.up, axis * sensitivityHorizontal * Time.deltaTime * horAxis);

        Vector3 dir = myChar.transform.position - rotatorX.transform.position;
        dir.y = 0;

        lookAtTrans.transform.position = myChar.transform.position + dir.normalized;
    }

    public void RotateVertical(float vertical)
    {
        if (UseBezier || forceStop) return;
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
        if (!UseBezier || forceStop) return;
        foreach (var item in bezierPoints)
        {
            item.transform.RotateAround(myChar.transform.position, Vector3.up, axis * sensitivityHorizontal * Time.deltaTime * horAxis);
        }
        Vector3 pos = myChar.transform.position - (bezierPoints[0].transform.position - myChar.transform.position).normalized;

        lookAtTrans.transform.position = new Vector3(pos.x, lookAtTrans.transform.position.y, pos.z);
    }

    public void RotateVerticalByBezier(float vertical)
    {
        if (!UseBezier || forceStop) return;
        sliderTime = Mathf.Clamp(sliderTime + (vertical * vertAxis) / 100, 0, 1);
    }

    public void TiltVertical(float vertical)
    {
        if (vertical == 0) { isTilting = false; return; }

        isTilting = true;
        tiltLerp = Mathf.Clamp(tiltLerp + vertical * sensitivityVertical * Time.deltaTime * vertAxis, -1, 1f);
    }

    public void CameraStartPosition()
    {
        Vector3 distToChar = new Vector3(myChar.transform.position.x - posToCheckpoint.position.x, 0, myChar.transform.position.z - posToCheckpoint.position.z);
        foreach (var item in bezierPoints)
        {
            float angle = Vector3.Angle(distToChar, new Vector3(myChar.transform.position.x - item.transform.position.x, 0, myChar.transform.position.z - item.transform.position.z));
            item.transform.RotateAround(myChar.transform.position, Vector3.up, -angle);
        }
    }
}
