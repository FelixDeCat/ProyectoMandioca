using System;
using UnityEngine;

/*
A 0
B 1
X 2
Y 3
Left Bumper 4
Right Bumper 5
View (Back) 6
Menu (Start) 7
Left Stick Button 8
Right Stick Button 9
*/


public class JoystickBasicInput
{
    bool[] optimize = new bool[14];

    event Action accept, cancel, square, triangle, lbuttonDown, lbuttonUp, rbuttonDown, rbuttonUp, ltrigger, rtrigger, dpadLeft, dpadRight, dpadUp, dpadDown, back, start = delegate { };
    event Action<float> movehorizontal, movevertical, twisthorizontal, twistvertical = delegate { };

    bool b_accept, b_cancel, b_square, b_triangle,
        b_lbuttonDown, b_lbuttonUp, b_rbuttonDown,
        b_rbuttonUp, b_ltrigger, b_rtrigger, b_dpadLeft,
        b_dpadRight, b_dpadUp, b_dpadDown, b_back, b_start,
        b_movehorizontal, b_movevertical, b_twisthorizontal,
        b_twistvertical;

    ClampedAxisButton DPad_Horizontal = new ClampedAxisButton("XBOX360_DPadHorizontal");
    ClampedAxisButton DPad_Vertical = new ClampedAxisButton("XBOX360_DPadVertical");
    ClampedAxisButton Triggers = new ClampedAxisButton("XBOX360_Trigger");

    public JoystickBasicInput SUBSCRIBE_ACCEPT(Action a) { accept += a; b_accept = true; return this; }
    public JoystickBasicInput SUBSCRIBE_CANCEL(Action a) { cancel += a; b_cancel = true;  return this; }
    public JoystickBasicInput SUBSCRIBE_SQUARE_X(Action a) { square += a; b_square = true;  return this; }
    public JoystickBasicInput SUBSCRIBE_TRIANGLE_Y(Action a) { triangle += a; b_triangle = true;  return this; }
    public JoystickBasicInput SUBSCRIBE_LBUTTON_DOWN(Action a) { lbuttonDown += a; b_lbuttonDown = true; return this; }
    public JoystickBasicInput SUBSCRIBE_LBUTTON_UP(Action a) { lbuttonUp += a; b_lbuttonUp = true; return this; }
    public JoystickBasicInput SUBSCRIBE_RBUTTON_DOWN(Action a) { rbuttonDown += a; b_rbuttonDown = true;  return this; }
    public JoystickBasicInput SUBSCRIBE_RBUTTON_UP(Action a) { rbuttonUp += a; b_rbuttonUp = true; return this; }
    public JoystickBasicInput SUBSCRIBE_LTRIGGER(Action a) { Triggers.AddEvent_Negative(a); b_ltrigger = true; return this; }
    public JoystickBasicInput SUBSCRIBE_RTRIGGER(Action a) { Triggers.AddEvent_Positive(a); b_rtrigger = true; return this; }
    public JoystickBasicInput SUBSCRIBE_DPAD_LEFT(Action a) { DPad_Horizontal.AddEvent_Negative(a); b_dpadLeft = true; return this; }
    public JoystickBasicInput SUBSCRIBE_DPAD_RIGHT(Action a) { DPad_Horizontal.AddEvent_Positive(a); b_dpadRight = true; return this; }
    public JoystickBasicInput SUBSCRIBE_DPAD_UP(Action a) { DPad_Vertical.AddEvent_Positive(a); b_dpadUp = true; return this; }
    public JoystickBasicInput SUBSCRIBE_DPAD_DOWN(Action a) { DPad_Vertical.AddEvent_Negative(a); b_dpadDown = true; return this; }
    public JoystickBasicInput SUBSCRIBE_BACK(Action a) { back += a; b_back = true; return this; }
    public JoystickBasicInput SUBSCRIBE_START(Action a) { start += a; b_start = true;  return this; }
    public JoystickBasicInput SUBSCRIBE_MOVE_HORIZONTAL(Action<float> a) { movehorizontal += a; b_movehorizontal = true; return this; }
    public JoystickBasicInput SUBSCRIBE_MOVE_VERTICAL(Action<float> a) { movevertical += a; b_movevertical = true; return this; }
    public JoystickBasicInput SUBSCRIBE_TWIST_HORIZONTAL(Action<float> a) { twisthorizontal += a; b_twisthorizontal = true; return this; }
    public JoystickBasicInput SUBSCRIBE_TWIST_VERTICAL(Action<float> a) { twistvertical += a; b_twistvertical = true; return this; }

    public void Refresh()
    {
        if (b_accept) if (Input.GetButtonDown("XBOX360_Accept")) accept.Invoke();
        if (b_cancel) if (Input.GetButtonDown("XBOX360_Cancel")) cancel.Invoke();
        if (b_square) if (Input.GetButtonDown("XBOX360_Square")) square.Invoke();
        if (b_triangle) if (Input.GetButtonDown("XBOX360_Triangle")) triangle.Invoke();

        if (b_lbuttonDown) if (Input.GetButtonDown("XBOX360_LB")) lbuttonDown.Invoke();
        if (b_lbuttonUp) if (Input.GetButtonUp("XBOX360_LB")) lbuttonUp.Invoke();

        if (b_rbuttonDown) if (Input.GetButtonDown("XBOX360_RB")) rbuttonDown.Invoke();
        if (b_rbuttonUp) if (Input.GetButtonUp("XBOX360_RB")) rbuttonUp.Invoke();

        if (b_movehorizontal) movehorizontal.Invoke(Input.GetAxis("MoveHorizontal"));
        if (b_movevertical) movevertical.Invoke(Input.GetAxis("MoveVertical"));
        if (b_twisthorizontal) twisthorizontal.Invoke(Input.GetAxis("TwistHorizontal"));
        if (b_twistvertical) twistvertical.Invoke(Input.GetAxis("TwistVertical"));

        if (b_dpadLeft || b_dpadRight) DPad_Horizontal.Refresh();
        if (b_dpadDown || b_dpadUp) DPad_Vertical.Refresh();
        if (b_ltrigger || b_rtrigger) Triggers.Refresh();

        if (b_back) if (Input.GetButtonDown("XBOX360_Back")) {  back.Invoke(); }
        if (b_start) if (Input.GetButtonDown("XBOX360_Start")) { start.Invoke(); }
    }
}


public class ClampedAxisButton
{
    event Action PositiveEvent, NegativeEvent;

    bool active_positive, active_negative;
    bool positive, negative = true;
    string axis;

    public ClampedAxisButton(string axis) { this.axis = axis; }
    public void AddEvent_Positive(Action ev_positive) {  PositiveEvent += ev_positive; active_positive = true; positive = true; }
    public void AddEvent_Negative(Action ev_negative) { NegativeEvent += ev_negative; active_negative = true; negative = true; }

    public void Refresh()
    {
        if (Input.GetAxisRaw(axis) != 0)
        {
            if (Input.GetAxisRaw(axis) == 1)
            {
                if (!active_positive) return;
                if (positive)
                {
                    PositiveEvent.Invoke();
                    positive = false;
                    negative = true;
                }
            }
            else
            {
                if (!active_negative) return;
                if (negative)
                {
                    NegativeEvent.Invoke();
                    positive = true;
                    negative = false;
                }
            }
        }
        else
        {
            positive = true;
            negative = true;
        }
    }
}
