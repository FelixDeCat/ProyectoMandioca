using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandOfDead_Handler : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] Transform _root;
    [SerializeField] float _lifeTime;

    DamageData dmgDATA;

    public event Action<HandOfDead_Handler> OnGrabPlayer;
    public event Action<HandOfDead_Handler> OnReachedDestination;

    Vector3 _dir;
   public float _count;

    public void Initialize(Transform from, Vector3 to,  int damage)
    {
        dmgDATA = GetComponent<DamageData>().SetDamage(damage).SetDamageType(Damagetype.Explosion);
        _dir = to;
        _root.position = from.position;
        _root.forward = new Vector3(to.x, 0, to.z);
       
    }

    public void GrabPlayer()
    {
        OnGrabPlayer?.Invoke(this);
        Main.instance.GetChar().DamageReceiver().TakeDamage(dmgDATA);
        CaronteEvent.instance.TurnOffCarontePP();
        Destroy(gameObject);
    }

    private void Update()
    {
        _count += Time.deltaTime;

        if (_count >= _lifeTime)
        {
            //No me salio
            OnReachedDestination?.Invoke(this);
            Destroy(gameObject);
        }

    }

    private void FixedUpdate()
    {
        _root.transform.position += _dir * speed * Time.fixedDeltaTime; 
    }
}
