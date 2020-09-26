using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TotemDestruible : MonoBehaviour
{
    Renderer render;
    [SerializeField] GameObject prefabToDestruible = null;

    private void Start()
    {
        render = GetComponent<Renderer>();
    }

    public void OnReset() => render.enabled = true;

    public void DropPiece(Vector3 dir, float force)
    {
        render.enabled = false;

        var newObject = Instantiate(prefabToDestruible);
        newObject.transform.SetPositionAndRotation(transform.position, transform.rotation);
        newObject.GetComponent<Rigidbody>()?.AddForce(dir * force, ForceMode.Impulse);;
    }
}
