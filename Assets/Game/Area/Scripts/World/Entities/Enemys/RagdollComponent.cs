using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RagdollComponent : MonoBehaviour
{
    [SerializeField] Collider[] myCollider = new Collider[1];
    [SerializeField] Rigidbody myRigidbody = null;
    [SerializeField] Animator anim = null;
    [SerializeField] float explosionForce = 40;
    [SerializeField] Collider principalBone;

    Bone[] myBones;

    private void Awake()
    {
        myBones = GetComponentsInChildren<Bone>();
        Ragdoll(false, Vector3.zero);
    }

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

        if(active)
            principalBone.GetComponent<Rigidbody>().AddForce((dir.normalized + transform.up) * explosionForce, ForceMode.Impulse);
    }

    public void DesactiveBones()
    {
        for (int i = 0; i < myBones.Length; i++)
        {
            myBones[i].GetComponent<Collider>().enabled = false;
        }
    }
}
