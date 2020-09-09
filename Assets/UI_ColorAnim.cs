using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ColorAnim : MonoBehaviour
{
    public Image img;
    public bool playOnAwake = true;
    PingPongLerp pingpong;

    public Color color1;
    public Color color2;


    private void Start()
    {
        pingpong = new PingPongLerp();
        pingpong.Configure(OnAnim, true);
        if (playOnAwake) Play();
    }

    void OnAnim(float val)
    {
        img.color = Color.Lerp(color1, color2, val);
    }

    public void Play()
    {
        pingpong.Play(1f);
    }

    private void Update()
    {
        pingpong.Updatear();
    }

}
