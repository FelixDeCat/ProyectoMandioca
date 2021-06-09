using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMode : MonoBehaviour
{
    [SerializeField] Camera[] UICameras = new Camera[0];
    bool dissapeared;

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.J))
            DissappearUI();
    }

    void DissappearUI()
    {
        for (int i = 0; i < UICameras.Length; i++)
        {
            UICameras[i].enabled = dissapeared;
        }
        dissapeared = !dissapeared;
    }
}
