using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

public class RagdollComponent : MonoBehaviour
{
    [SerializeField] Collider[] myCollider = new Collider[1];
    [SerializeField] Rigidbody myRigidbody = null;
    [SerializeField] Animator anim = null;
    [SerializeField] float explosionForce = 40;
    [SerializeField] Collider principalBone;
    Action callback_end;

    Bone[] myBones;
    Vector3[] transformpositions;

    private void Awake()
    {
        myBones = GetComponentsInChildren<Bone>();
        Ragdoll(false, Vector3.zero);
    }

    public void ActivateRagdoll(Vector3 direction_to_fall, Action _callback_end)
    {
        callback_end = _callback_end;
        Ragdoll(true, direction_to_fall);
        Invoke("ShutDownBones", 5f);
    }

    void ShutDownBones()
    {
        DesactiveBones();
        Invoke("DisableWendigo", 3f);
    }
    void DisableWendigo() => callback_end.Invoke();


    public void Ragdoll(bool active, Vector3 dir)
    {
        for (int i = 0; i < myCollider.Length; i++)
        {
            myCollider[i].enabled = !active;
        }

        myRigidbody.isKinematic = active;
        myRigidbody.detectCollisions = !active;
        anim.enabled = !active;

        for (int i = 0; i < myBones.Length; i++)
        {
            myBones[i].GetComponent<Rigidbody>().isKinematic = !active;
            myBones[i].GetComponent<Rigidbody>().detectCollisions = active;
            myBones[i].GetComponent<Collider>().enabled = active;
        }

        if (active)
        {
            Vector3 temp = dir * explosionForce;

            principalBone.GetComponent<Rigidbody>().AddForce(temp, ForceMode.Impulse);
        }
    }

    public void DesactiveBones()
    {
        for (int i = 0; i < myBones.Length; i++)
        {
            myBones[i].GetComponent<Collider>().enabled = false;
        }
    }
}
