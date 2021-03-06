﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastTravell : MonoBehaviour
{
    [SerializeField]CustomCamera _myCamera = null;
     public Transform _myTransform;
    bool _inFastTravell;
    [SerializeField] float _speedOfTravell = 2;
    [SerializeField] LayerMask _mask = 0;
    bool DoneTheReturn;

    void Update()
    {
        if (_inFastTravell)
            _goToFastTravell();
        else
            _ReturnFastTravell();
    }

    public void _OnClickOnFastTravell()
    {
        if (_inFastTravell)
            _inFastTravell = false;
        else
            _inFastTravell = true;
    }
    void _goToFastTravell()
    {
        DoneTheReturn = false;
        _myCamera.active = false;
        _myCamera.transform.position = Vector3.Lerp(_myCamera.transform.position, _myTransform.position, Time.deltaTime * _speedOfTravell);
        _myCamera.transform.forward = Vector3.Lerp(_myCamera.transform.forward, Vector3.down, Time.deltaTime * _speedOfTravell);
        Vector3 mousePos = Input.mousePosition;
       
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(mousePos);
            RaycastHit hit;
           
            if (Physics.Raycast(ray,out hit,10000,_mask))
            {
                if (hit.collider.gameObject.layer == 21)
                {
                    Main.instance.GetChar().GetCharMove().StopDamageFall();
                    Main.instance.GetChar().transform.position = hit.point;
                    _inFastTravell = false;

                }
            }
        }
        

    }
    void _ReturnFastTravell()
    {
        if (!DoneTheReturn)
        {
            _myCamera.ChangeToDefaultCamera();
            DoneTheReturn = true;
        }
        
       
    }
}
