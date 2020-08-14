using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimPalanca : MonoBehaviour
{
    PingPongLerp pingpong;

    private void Awake()
    {
        pingpong = new PingPongLerp();
        pingpong.Configure(Animate, false);
    }

    private void Update()
    {
        pingpong.Updatear();
    }

    void Animate(float val)
    {

    }

    public void Anim()
    {
        pingpong.Play(1);
    }
}
