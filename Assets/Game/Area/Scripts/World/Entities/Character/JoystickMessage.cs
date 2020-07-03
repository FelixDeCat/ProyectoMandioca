using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoystickMessage : UI_Base
{
    public Image img;
    public Text txt;

    public Sprite joystick;
    public Sprite keyboard;

    bool anim_joystick;
    float timer;

    public void Message(bool isjoystick)
    {
        anim_joystick = true;
        timer = 0;

        if (isjoystick)
        {
            txt.text = "Joystick";
            img.sprite = joystick;
        }
        else
        {
            txt.text = "Mouse & Keyboard";
            img.sprite = keyboard;
        }
    }

    private void Update()
    {
        if (anim_joystick)
        {
            if (timer < 5)
            {
                timer = timer + 1 * Time.deltaTime;
            }
            else
            {
                anim_joystick = false;
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
    protected override void OnUpdate() { }
}
