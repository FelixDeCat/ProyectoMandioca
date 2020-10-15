using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PPTransition : MonoBehaviour
{
    public PostProcessVolume volumeEntry;
    public PostProcessVolume volumeExit;

    public float transitionSpeed;

    public void Transition()
    {
       // while (volumeEntry.weight<1 && volumeExit.weight>0)
      //  {
            volumeEntry.weight = Time.deltaTime + transitionSpeed;
            volumeExit.weight = Time.deltaTime - transitionSpeed;
     //   }
       

    }

}
