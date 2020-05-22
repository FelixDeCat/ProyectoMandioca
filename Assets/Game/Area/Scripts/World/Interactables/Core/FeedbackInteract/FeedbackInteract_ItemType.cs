using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedbackItem : FeedbackInteractBase
{
    public SpriteRenderer sp;
    public Transform endpoint;
    public LineRenderer lr;

    private void Awake()
    {
        sp = GetComponentInChildren<SpriteRenderer>();
        if (sp) sp.color = new Color(0, 0, 0, 0);

        lr.SetPosition(0, new Vector3(transform.position.x, initialY, transform.position.z));
        lr.SetPosition(1, new Vector3(transform.position.x, initialY, transform.position.z));
        lr.enabled = false;
    }

    bool animate;
    float timer;
    float posy;
    float initialY = 1;

    protected override void OnShow()
    {
        sp.color = new Color(1, 1, 1, 1);
        animate = true;
        timer = 0;
        lr.enabled = true;
        lr.SetPosition(0, new Vector3(transform.position.x, initialY, transform.position.z));
        lr.SetPosition(1, new Vector3(transform.position.x, initialY, transform.position.z));
    }

    protected override void OnHide()
    {
        sp.color = new Color(0, 0, 0, 0);
        animate = false;
        timer = 0;
        lr.SetPosition(1, new Vector3(transform.position.x, initialY, transform.position.z));
        lr.enabled = false;
    }

    protected override void On_Condicional_Update()
    {
        if (animate)
        {
            if (timer < 1)
            {
                timer = timer + 5 * Time.deltaTime;
                posy = Mathf.Lerp(initialY, endpoint.localPosition.y, timer);

                lr.SetPosition(1, new Vector3(transform.position.x, posy, transform.position.z));
            }
            else
            {
                animate = false;
                timer = 0;
            }
        }
    }

    protected override void On_Permanent_Update() { }
}
