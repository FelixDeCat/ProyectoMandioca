using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Replacement : MonoBehaviour
{

    public Shader XRayShader;

    bool _xRayActive = false;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!_xRayActive && Input.GetKeyUp(KeyCode.Space))
        {
            GetComponent<Camera>().enabled = true;

            ///Los tags tienen que estar igualados en el Shader del objeto, y el Shader que va a reemplazar

            GetComponent<Camera>().SetReplacementShader(XRayShader, "Custom");
            _xRayActive = true;
        }

        else if (_xRayActive && Input.GetKeyUp(KeyCode.Space))
        {
            GetComponent<Camera>().ResetReplacementShader();
            GetComponent<Camera>().enabled = false;
            _xRayActive = false;
        }
    }
}
