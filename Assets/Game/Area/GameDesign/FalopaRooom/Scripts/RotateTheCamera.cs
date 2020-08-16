using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTheCamera : MonoBehaviour
{

    public GameObject target;
    float _distance;
    Transform _playerTrans;
    bool _returnToPos;
    [SerializeField] float _speed;
    [SerializeField] float _speedToReturn;
    [SerializeField] float _speedAwayFromMesh;
    private float PosY;
    [SerializeField] LayerMask _mask;
    [SerializeField]float _range;
    public bool colliding;

    private void Start()
    {
        _distance = Vector3.Distance(transform.position, target.transform.position);
        _playerTrans = Main.instance.GetChar().transform;
        PosY = transform.localPosition.y;
        _range+= Vector3.Distance(transform.position, _playerTrans.position);
    }

    private void Update()
    {
        //if (_returnToPos)
        //    transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(transform.localPosition.x, PosY, transform.localPosition.z), _speedToReturn * Time.deltaTime);
         if(!colliding) ShootRaycast();
    }
    public void RotationOfCamera(float axis)
    {
        transform.RotateAround(target.transform.position, Vector3.up, axis *_speed* Time.deltaTime);
    }
    //public void ReturnToPos(bool result)
    //{
    //    _returnToPos = result;
    //}

    public void MovementFromMesh(float speed)
    {
        Vector3 direction = (target.transform.position - transform.position).normalized;
        if(Vector3.Distance(target.transform.position, transform.position) > 0.5f)
            transform.position += direction * speed * Time.deltaTime;
    }

    public void ReturnToPos(float speed)
    {
        float currentDistance= Vector3.Distance(transform.position, target.transform.position);
        if (currentDistance >= _distance)
            return;
        Vector3 direction = (transform.position - target.transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;
    }

    public void ShootRaycast()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position,((_playerTrans.position-transform.position).normalized),out hit, _range, _mask))
        {
            if (hit.collider.gameObject.GetComponent<CharacterHead>())
            {
                ReturnToPos(_speedToReturn);

            }
            else
            {
                MovementFromMesh(_speedAwayFromMesh);
            }
        }
    }
}
