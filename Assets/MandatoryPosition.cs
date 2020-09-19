using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class MandatoryPosition : MonoBehaviour
{
    Vector3 position;
    Vector3 rotation;
    public bool Locked = false;
    public bool capture = false;

    /// <summary>
    /// como se usa? le das a capture primero, asi se guarda la posicion, luego lo lockeas
    /// </summary>
    private void Capture()
    {
        position = this.transform.position;
        rotation = this.transform.eulerAngles;
    }
    void Update()
    {
        if (capture)
        {
            capture = false;
            Capture();
        }
        if (Locked)
        {
            this.transform.position = position;
            this.transform.eulerAngles = rotation;
        }
    }
}
