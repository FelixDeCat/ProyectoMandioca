using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedbackOneShot : FeedbackBase
{
    public ParticleSystem[] parts;

    pepito[] arra = new pepito[2];

    protected override void OnPlayFeedback()
    {
        for (int i = 0; i < parts.Length; i++)
        {
            parts[i].Play();


        }

        arra[0] = new jorgito();
        arra[1] = new jorgito();

        var aux = arra[0].GetType();

        
    }

    class jorgito : pepito
    {

    }

    class pepito
    {

    }
}
