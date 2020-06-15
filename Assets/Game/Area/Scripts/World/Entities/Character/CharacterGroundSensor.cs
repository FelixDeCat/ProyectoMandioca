using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterGroundSensor : MonoBehaviour
{
    [SerializeField] LayerMask ground;
    bool isGronded;
   // public bool IsGrounded => isGronded;

    public float radius;

    public bool IsGrounded()
    {
        var cols = Physics.OverlapSphere(this.transform.position, radius, ground.value);
        foreach (var c in cols) return true;
        return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(this.transform.position, radius);
    }

    public void OnTriggerEnter(Collider other)
    {
        if ((1 << other.gameObject.layer & ground) != 0)
        {
            DebugCustom.Log("GroundSensor", "Sensor", true);
            isGronded = true;
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if ((1 << other.gameObject.layer & ground) != 0)
        {
            DebugCustom.Log("GroundSensor", "Sensor", false);
            isGronded = false;
        }
    }
}
