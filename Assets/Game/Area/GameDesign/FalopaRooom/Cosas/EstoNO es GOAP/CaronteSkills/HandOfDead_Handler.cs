using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandOfDead_Handler : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] Transform _root;

    DamageData dmgDATA;

    public event Action<HandOfDead_Handler> OnGrabPlayer;
    public event Action<HandOfDead_Handler> OnReachedDestination;

    Vector3 _dir;

    public void Initialize(Transform from, Vector3 to,  int damage)
    {
        dmgDATA = GetComponent<DamageData>().SetDamage(damage).SetDamageType(Damagetype.Explosion);
        _dir = to;
        _root.position = from.position;
        _root.rotation = from.rotation;

        
    }

    public void GrabPlayer()
    {
        OnGrabPlayer?.Invoke(this);
        Main.instance.GetChar().DamageReceiver().TakeDamage(dmgDATA);
        CaronteEvent.instance.TurnOffCarontePP();
    }

    private void FixedUpdate()
    {
        _root.transform.position += _dir * speed * Time.fixedDeltaTime; 
    }
}
