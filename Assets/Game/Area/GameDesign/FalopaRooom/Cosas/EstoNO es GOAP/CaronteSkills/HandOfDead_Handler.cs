using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools.Extensions;

public class HandOfDead_Handler : MonoBehaviour
{
    [SerializeField] float speed = 5;
    [SerializeField] Transform _root = null;
    public Transform Root => _root;
    [SerializeField] float _lifeTime = 4;

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

        //StartCoroutine(CheckIfDoorIsClose());
    }

    public void GrabPlayer()
    {
        if (Main.instance.GetChar().DamageReceiver().TakeDamage(dmgDATA) == Attack_Result.inmune)
            return;

        OnGrabPlayer?.Invoke(this);
        CaronteEvent.instance.TurnOffCarontePP();
        Destroy(gameObject);
    }

    private void Update()
    {
        _count += Time.deltaTime;

        if (_count >= _lifeTime)
        {
            _count = 0;
            //CheckIfDoorIsClose();
            OnReachedDestination?.Invoke(this);
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        _root.transform.position += _dir * speed * Time.fixedDeltaTime; 
    }

    //IEnumerator CheckIfDoorIsClose()
    //{
    //    while(true)
    //    {
    //        var d = Extensions.FindInRadius<CaronteExitDoor>(_root.position, 10);

    //        if (d.Count > 0)
    //        {
    //            d[0].HandHit();
    //            OnReachedDestination?.Invoke(this);
    //            Destroy(gameObject);
    //        }

    //        yield return new WaitForSeconds(.3f);
    //    }
        
    //}
}
