using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestoyedPart : MonoBehaviour
{
    void Start() => StartCoroutine(DestroyCoroutine());

    IEnumerator DestroyCoroutine()
    {
        yield return new WaitForSeconds(3);

        GetComponent<Collider>().enabled = false;

        yield return new WaitForSeconds(1);

        Destroy(this.gameObject);
    }
}
