using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyedVersion : MonoBehaviour
{
    public Renderer[] render;

    float timer;

    [SerializeField] float maxTimer = 1;

    bool animate;
    Color mycolor;

    [SerializeField] bool useFade = true;
    public Rigidbody principalChild = null;

    public void BeginDestroy()
    {
        render = GetComponentsInChildren<Renderer>();
        if(useFade) mycolor = render[0].material.color;
        animate = true;
        timer = maxTimer;
    }

    public void ExplosionForce(Vector3 explosionPoint, float force, float torqueForce)
    {
        var childs = GetComponentsInChildren<Rigidbody>();

        if (principalChild)
        {
            foreach (var c in childs)
            {
                Vector3 aux;
                if (c != principalChild) aux = c.transform.position - principalChild.transform.position;
                else aux = c.transform.position - explosionPoint;
                c.AddForce(aux * force, ForceMode.VelocityChange);
                c.AddTorque(aux * torqueForce);
            }
        }
        else
        {
            foreach (var c in childs)
            {
                var aux = c.transform.position - explosionPoint;
                aux.Normalize();
                c.AddForce(aux * force, ForceMode.VelocityChange);
                c.AddTorque(aux * torqueForce);
            }
        }
    }

    private void Update()
    {
        if (animate)
        {
            if (timer > 0)
            {
                timer = timer - 0.3f * Time.deltaTime;
                foreach (var r in render) if (useFade) r.material.color = new Color(mycolor.r, mycolor.g, mycolor.b, timer);
            }
            else
            {
                animate = false;
                timer = 1;
                Destroy(this.gameObject);
            }
        }
    }
}
