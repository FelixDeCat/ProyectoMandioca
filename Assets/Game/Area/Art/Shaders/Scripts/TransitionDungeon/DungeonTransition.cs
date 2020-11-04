using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class DungeonTransition : MonoBehaviour
{

    public PostProcessVolume firstZonePP;
    public PostProcessVolume jacintaPP;

   

    public void Enter()
    {
        firstZonePP.weight = 0;
        jacintaPP.weight = 0;

    }

    public void Exit()
    {
        firstZonePP.weight = 1;
        jacintaPP.weight = 1;

    }
}
