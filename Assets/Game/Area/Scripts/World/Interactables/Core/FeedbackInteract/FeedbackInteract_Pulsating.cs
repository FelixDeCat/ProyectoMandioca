using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedbackInteract_Pulsating : FeedbackInteractBase
{
    public Transform toscale;
    Vector3 startscale = Vector3.zero;
    Vector3 finalscale = Vector3.zero;

    public bool own;
    public Vector3 cant_to_scale = new Vector3(0.5f, 0.5f, 0.5f);
    bool anim;
    PingPongLerp pingPong;
    [SerializeField] float cant_speed_ping_pong_lerp = 2;
    private void Awake()
    {
        pingPong = new PingPongLerp();

        if (own) toscale = this.transform;

        startscale = toscale.transform.localScale;
        finalscale = new Vector3(
            startscale.x + cant_to_scale.x,
            startscale.y + cant_to_scale.y,
            startscale.z + cant_to_scale.z);

        pingPong.Configure(ToAnimate, true);
    }

    void ToAnimate(float val)
    {
        toscale.transform.localScale = Vector3.Lerp(startscale, finalscale,val);
    }

    protected override void OnShow() 
    {
        pingPong.Play(cant_speed_ping_pong_lerp);
        anim = true;
    }
    protected override void OnHide() 
    {
        pingPong.Stop();
        anim = false;
        toscale.transform.localScale = startscale;
    }
    protected override void On_Condicional_Update()
    {
        if (anim)
        {
            pingPong.Updatear();
        }
    }

    protected override void On_Permanent_Update() { }
}
