using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxTransition : MonoBehaviour
{

    public Material skybox;

    [Range(0,1)]
    public float NigthValue;

    private void Update()
    {
        SetNigth();
    }

    void SetNigth()
    {
        NigthValue += Time.deltaTime*.1f;
        skybox.SetFloat("_Night", NigthValue);
        NigthValue = Mathf.Clamp(NigthValue, 0, 1);
    }

    void SetDay()
    {

    }

    

}
