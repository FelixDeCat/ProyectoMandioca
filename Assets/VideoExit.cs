using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.Events;

public class VideoExit : MonoBehaviour
{
    public VideoPlayer vid;
    public UnityEvent OnEnd;
    public 
    void Start()
    {
        vid.loopPointReached += EndReached;
    }

    // Update is called once per frame
    void EndReached(VideoPlayer vp)
    {
        OnEnd.Invoke();
    }
}
