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
