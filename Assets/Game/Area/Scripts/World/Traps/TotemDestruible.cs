using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TotemDestruible : MonoBehaviour
{
    Collider col;
    Rigidbody rb;

    private void Start()
    {
        col = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
    }

    public void DropPiece(Vector3 dir, float force)
    {
        transform.SetParent(null);
        col.enabled = true;
        rb.isKinematic = false;

        rb.AddForce(dir * force, ForceMode.Impulse);;

        StartCoroutine(DestroyCoroutine());
    }

    IEnumerator DestroyCoroutine()
    {
        yield return new WaitForSeconds(3);

        col.enabled = false;

        yield return new WaitForSeconds(1);

        Destroy(this.gameObject);
    }
}
