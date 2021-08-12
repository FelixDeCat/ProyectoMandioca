using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class outro : MonoBehaviour
{
    public void PlayOutro()
    {
        VideoCamera.Play("outro", OnEnd);
            
    }

    void OnEnd()
    {

    }
}
