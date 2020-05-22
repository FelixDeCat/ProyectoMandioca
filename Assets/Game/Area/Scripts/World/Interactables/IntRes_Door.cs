using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntRes_Door : Interact_Receptor
{
    public bool open;
    bool anim;

    float timer;
    public float TimeAnim = 1f;

    public Transform door;
    public Transform pos1;
    public Transform pos2;

    public LineRenderer ln;

    public override void Execute()
    {
        if (anim) return;
        open = !open;
        anim = true;
        timer = 0;
    }

    public void Close()
    {
        open = false;
        anim = true;
        timer = 0;
    }
    public void Open()
    {
        open = true;
        anim = true;
        timer = 0;
    }

    private void Update()
    {
        if (anim)
        {
            if (open)
            {
                if (timer < TimeAnim)
                {
                    timer = timer + 1 * Time.deltaTime;
                    door.position = Vector3.Lerp(pos1.position, pos2.position, timer);
                }
                else
                {
                    if(ln != null) ln.material.SetColor("_Color", Color.green);
                    anim = false;
                    timer = 0;
                }
            }
            else
            {
                if (timer < TimeAnim)
                {
                    timer = timer + 1 * Time.deltaTime;
                    door.position = Vector3.Lerp(pos2.position, pos1.position, timer);
                }
                else
                {
                    if (ln != null) ln.material.SetColor("_Color", Color.red);
                    anim = false;
                    timer = 0;
                }
            }
        }
    }
}
