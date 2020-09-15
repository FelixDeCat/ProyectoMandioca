using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CullingTest : MonoBehaviour
{
    public CullingGroup group;
    BoundingSphere[] spheres;

    private void Start()
    {
        group.targetCamera = Main.instance.GetMyCamera().MyCam;

        spheres = new BoundingSphere[1];
        spheres[0] = new BoundingSphere(Vector3.zero, 1f);
        group.SetBoundingSpheres(spheres);
        group.SetBoundingSphereCount(1);

        CullingGroup.StateChanged statechange = new CullingGroup.StateChanged(test);
        group.onStateChanged = statechange;

    }

    void test(CullingGroupEvent asd) => Debug.Log(asd);


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(Vector3.zero, 5f);

        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(Vector3.zero, 5f);
        for (int i = 0; i < spheres.Length; i++)
        {
            DebugCustom.Log("CullingTest", "spherePos", spheres[i].position);
            DebugCustom.Log("CullingTest", "sphereRad", spheres[i].radius);
            Gizmos.DrawSphere(spheres[i].position, spheres[i].radius);
        }
    }
}
