using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;
using System;

public class Fly_module : MonoBehaviour
{
    Rigidbody _rb;
    Ente _ent;

    public Vector3 _dest_pos;
    public Waypoint wp;
    Vector3 _dir;

    bool canUpdate = false;

    public event Action OnFinishMovement; 

    float speedScaler;

    [SerializeField] ParticleSystem Feedback = null;

    public void Init()
    {

        _rb = GetComponent<Rigidbody>();
        _ent = GetComponent<Ente>();
    }

    public void GainMagicFly()
    {
        if (_ent.heightLevel == 1)
        {
            OnFinishMovement?.Invoke();
            return;
        }

        Feedback.Play();

        canUpdate = true;
        _rb.useGravity = false;
        wp = Navigation.instance.NearestTo(_ent.Root().position, 1);
        _dest_pos = wp.transform.position;
        _dir = (_dest_pos - _rb.position).normalized;
        speedScaler = 2;
    }

    void ReachedDest()
    {
        if(_ent.heightLevel == 0)
        {
            //_rb.constraints = flyingConstrains;
            _ent.heightLevel = 1;
            WorldState.instance.valoresBool["OnGround"] = false;
        }
        else
        {
            _ent.heightLevel = 0;
            WorldState.instance.valoresBool["OnGround"] = true;
        }

        speedScaler = 1;
        canUpdate = false;
        OnFinishMovement?.Invoke();
    }

    public void LoseMagicFly()
    {

        if (_ent.heightLevel == 0)
        {
            OnFinishMovement?.Invoke();
            return;
        }

        Feedback.Stop();

        canUpdate = true;

        //_rb.constraints = defaultConstrains;
        _rb.useGravity = true;
        wp = Navigation.instance.NearestTo(_ent.Root().position, 0);
        _dest_pos = wp.transform.position;
        _dir = (_dest_pos - _rb.position).normalized;
        speedScaler = 4;
    }

    void FixedUpdate()
    {
        if (!canUpdate) return;

        if (_dest_pos != Vector3.zero)
        {
            _rb.velocity += _dir * _ent.speed * speedScaler * Time.fixedDeltaTime;
            return;
        }
    }

    void Update()
    {
        if (!canUpdate) return;

        if (Vector3.Distance(_dest_pos, _rb.position) <= .8f)
        {
            _rb.position = _dest_pos;
            ReachedDest();
        }
    }
}
