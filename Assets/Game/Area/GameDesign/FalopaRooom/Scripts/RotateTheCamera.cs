using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTheCamera : MonoBehaviour
{

    public GameObject target;
    bool _returnToPos;
    [SerializeField] float _speed;
    [SerializeField] float _speedToReturn;
    private float PosY;

    private void Start()
    {
        PosY = transform.localPosition.y;
    }

    private void Update()
    {
        if (_returnToPos)
            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(transform.localPosition.x, PosY, transform.localPosition.z), _speedToReturn * Time.deltaTime);
    }
    public void RotationOfCamera(float axis)
    {
        transform.RotateAround(target.transform.position, Vector3.up, axis *_speed* Time.deltaTime);
    }
    public void ReturnToPos(bool result)
    {
        _returnToPos = result;
    }
}
