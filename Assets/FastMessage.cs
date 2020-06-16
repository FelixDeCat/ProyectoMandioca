using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FastMessage : UI_Base
{
    public static FastMessage instance;
    public Text txt_to_message;
    public void Awake() => instance = this;
    bool anim;
    float timer;
    float time_to_close;

    public void Print(string message, float time)
    {
        Open();
        time_to_close = time;
        anim = true;
        txt_to_message.text = message;
    }

    protected override void OnUpdate()
    {
        if (anim)
        {
            if (timer < time_to_close)
            {
                timer = timer + 1 * Time.deltaTime;
            }
            else
            {
                anim = false;
                timer = 0;
                Close();
            }
        }
    }

    public override void Refresh() { }
    protected override void OnAwake() { }
    protected override void OnEndCloseAnimation() { }
    protected override void OnEndOpenAnimation() { }
    protected override void OnStart() { }
   
}
