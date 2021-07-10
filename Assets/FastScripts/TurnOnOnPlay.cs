using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOnOnPlay : MonoBehaviour
{
    public GameObject parent;

    void Start()
    {
        parent.SetActive(true);
    }

}
