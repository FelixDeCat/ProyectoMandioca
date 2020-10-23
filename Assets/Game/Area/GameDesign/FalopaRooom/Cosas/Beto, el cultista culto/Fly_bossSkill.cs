using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;

public class Fly_bossSkill : GOAP_Skills_Base
{
    Rigidbody _rb;
    Ente _ent;

    public Vector3 _dest_pos;
    public Waypoint wp;
    Vector3 _dir;

    RigidbodyConstraints defaultConstrains;
    RigidbodyConstraints flyingConstrains;
    
    float speedScaler;

    protected override void OnEndSkill()
    {
        if (wp.heighLevel == 0)
        {
            _rb.constraints = defaultConstrains;
        }
        else
        {
            _rb.constraints = flyingConstrains;
        }


        Off();
    }

    protected override void OnExecute()
    {

        _rb = owner.GetComponent<Rigidbody>();
        _ent = owner.GetComponent<Ente>();

        defaultConstrains = _rb.constraints;

        if (_ent.heightLevel == 0)
        {
            GoUp();
        }
        else
        {
            GoDown();
        }

        On();
    }


    protected override void OnFixedUpdate()
    {
        if (_dest_pos != Vector3.zero)
        {
            _rb.velocity += _dir * _ent.speed * speedScaler * Time.fixedDeltaTime;
            return;
        }


    }

    public void GoUp()
    {
        wp = Navigation.instance.NearestTo(_ent.Root().position, 1);
        _dest_pos = wp.transform.position;
        _dir = (_dest_pos - _rb.position).normalized;
        _ent.heightLevel = 1;
        speedScaler = 2;
        WorldState.instance.values["OnGround"] = false;
    }

    public void GoDown()
    {
        wp = Navigation.instance.NearestTo(_ent.Root().position, 0);
        _dest_pos = wp.transform.position;
        _dir = (_dest_pos - _rb.position).normalized;
        _ent.heightLevel = 0;
        speedScaler = 4;
        WorldState.instance.values["OnGround"] = true;
    }

    protected override void OnInitialize()
    {

        flyingConstrains = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY;
    }

    protected override void OnPause()
    {
        
    }

    protected override void OnResume()
    {
        
    }

    protected override void OnTurnOff()
    {
        
    }

    protected override void OnTurnOn()
    {
        
    }

    protected override void OnUpdate()
    {
        if(Vector3.Distance(_dest_pos, _rb.position) <= .8f)
        {
            _rb.position = _dest_pos;
            EndSkill();
        }
    }

    //private void OnDrawGizmos()
    //{
    //    if (_dest_pos != null)
    //        Gizmos.DrawSphere(_dest_pos, .5f);
    //}
}
