using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerOfPiston : MonoBehaviour
{
    public Transform parentToParent;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<CharacterHead>())
        {
            other.gameObject.GetComponent<CharacterHead>().transform.parent = parentToParent.transform;
        }
    }
    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.GetComponent<CharacterHead>() && collision.gameObject.GetComponent<CharacterHead>().transform.parent == parentToParent.transform)
        {
            collision.gameObject.GetComponent<CharacterHead>().transform.parent = collision.gameObject.GetComponent<CharacterHead>().MyParent;
        }
    }
}
