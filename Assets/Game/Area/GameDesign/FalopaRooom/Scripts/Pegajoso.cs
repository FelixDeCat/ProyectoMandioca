using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pegajoso : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<CharacterHead>())
        {
            collision.gameObject.GetComponent<CharacterHead>().transform.parent = this.transform;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.GetComponent<CharacterHead>())
        {
            collision.gameObject.GetComponent<CharacterHead>().transform.parent = null;
        }
    }
}
