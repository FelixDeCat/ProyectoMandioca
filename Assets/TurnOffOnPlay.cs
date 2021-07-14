using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOffOnPlay : MonoBehaviour
{
    private void Start()
    {
        Invoke("TurnOff", 0.1f);
    }

    void TurnOff()
    {
        this.gameObject.SetActive(false);
    }
}
