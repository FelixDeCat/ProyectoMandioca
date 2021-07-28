using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] Portal conection = null;
    [SerializeField] RenderTexture portalRender = null;
    [SerializeField] MeshRenderer myMesh = null;
    [SerializeField] LayerMask transportable = 1 << 8;
    [SerializeField] ParticleSystem particle = null;
    float cd;
    bool canTP;

    private void Start()
    {
        myMesh.material.SetTexture("_PortalRender", portalRender);
    }
    void Update()
    {
        if (canTP)
        {
            cd += Time.deltaTime;

            if (cd >= 0.2f)
            {
                canTP = false;
                cd = 0;
            }
        }
    }

    void TPToPortal(Transform tpObject, Vector3 diff)
    {
        canTP = true;
        tpObject.position = myMesh.transform.position + diff;


        if (tpObject.GetComponent<CharacterHead>())
            tpObject.GetComponent<CharacterHead>().Root.forward = new Vector3(myMesh.transform.up.x, 0, myMesh.transform.up.z);
        else if (tpObject.GetComponent<BashingRock>())
            tpObject.GetComponent<BashingRock>().myDir = myMesh.transform.up;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (canTP) return;
        if ((1 << other.gameObject.layer & transportable) != 0)
        {
            if(conection == null)
            {
                if (other.GetComponent<BashingRock>()) { particle.Play(); other.transform.position = Vector3.zero; }
                return;
            }
            conection.TPToPortal(other.transform, other.transform.position - myMesh.transform.position);
            particle.Play();
        }
    }
}
