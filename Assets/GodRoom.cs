using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodRoom : MonoBehaviour
{
    InteractableTeleport teleport;
    private void Awake()
    {
        teleport = GetComponent<InteractableTeleport>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.G) && Input.GetKeyDown(KeyCode.R))
        {
            teleport.Execute(Main.instance.GetChar());
        }
    }
}
