using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JacintaQueCamina : PistonWithSteps
{
    [SerializeField] Transform _rootRot = null;
    [SerializeField] Animator _anim = null;

    public void StartWalkingAnim()
    {
        _anim.SetTrigger("walk");
    }

    public override void Move()
    {
        _root.transform.position += _root.forward * speed * Time.fixedDeltaTime;
        _rootRot.LookAt(nodes[currentNode]);
    }
}
