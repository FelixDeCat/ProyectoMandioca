using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnoSuperCheat : MonoBehaviour
{
    [SerializeField] GameObject magno_face;
    [SerializeField] GameObject original;

    private void Start()
    {
        magno_face.SetActive(false);
        original.SetActive(true);
    }

    public string ToogleMagnoFace(bool val)
    {
        if (val)
        {
            magno_face.SetActive(true);
            original.SetActive(false);
            return "ents magnificados";
        }
        else
        {
            magno_face.SetActive(false);
            original.SetActive(true);
            return "ents normales";
        }
        
    }
}
