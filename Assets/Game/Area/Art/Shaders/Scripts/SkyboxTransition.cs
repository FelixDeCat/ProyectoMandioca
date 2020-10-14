using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxTransition : MonoBehaviour
{

    public Material skybox;

    [Range(0,1)]
    public float NigthValue;

    private void Awake()
    {
        skybox.SetFloat("_Night", 0);
    }

   

    float SetNigth()
    {
        if (NigthValue == 1)
            return NigthValue;

        NigthValue += Time.deltaTime*.01f;
        skybox.SetFloat("_Night", NigthValue);
        NigthValue = Mathf.Clamp(NigthValue, 0, 1);

        return SetNigth();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<CharacterHead>())
        {
            SetNigth();

           
        }
    }



}
