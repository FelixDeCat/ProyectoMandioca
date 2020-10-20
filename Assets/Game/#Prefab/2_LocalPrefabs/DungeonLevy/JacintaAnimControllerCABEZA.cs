using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JacintaAnimControllerCABEZA : MonoBehaviour
{
    [SerializeField]
    Animator myanim = null;
    void Start()
    {
        myanim.SetBool("Crying", true);
    }
}
