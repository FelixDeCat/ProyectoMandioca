using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationPositionCapture : MonoBehaviour
{
    public Transform root_to_capture;
    Vector3 pos_captured;
    public void CapturePosition() => pos_captured = root_to_capture.transform.position;
    public Vector3 GetPosition() => pos_captured;

}
