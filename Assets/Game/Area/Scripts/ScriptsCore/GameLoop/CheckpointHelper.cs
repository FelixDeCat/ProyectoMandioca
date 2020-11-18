using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CheckpointHelper : MonoBehaviour
{
    public bool execute;
    public LayerMask mask_hit_floor;

    void Update()
    {
        if (execute)
        {
            execute = false;

            var checkpoint = GetComponent<Checkpoint>();

            RaycastHit hit;

            if (Physics.Raycast(this.transform.position + this.transform.up * 10, this.transform.up * -1, out hit, 30, mask_hit_floor))
            {
                checkpoint.sceneName = hit.collider.gameObject.scene.name;
            }
        }
    }
}
