using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Tools.EventClasses;
using UnityEngine.SceneManagement;
using Tools;

public class CharacterInput : MonoBehaviour
{
    private const string Load = "0_Load";
    private const string Menu = "Menu";

    public enum InputType { Joystick, Mouse, Other }
    public InputType input_type;

    JoystickBasicInput joystickhelper;

    [Header("WASD / L_STICK")]
    public UnityEvFloat LeftHorizontal;
    public UnityEvFloat LeftVertical;

    [Header("MOUSE_ROT / R_STICK")]
    public UnityEvFloat RightHorizontal;
    public UnityEvFloat RightVertical;

    public UnityEvFloat MouseScrollWheel;

    public UnityEvFloat ChangeWeapon;
    public UnityEvent Dash;

    [Header("Defense")]
    public UnityEvent OnBlock;
    public UnityEvent UpBlock;

    [Header("Attack")]
    public UnityEvent OnAttack;
    public UnityEvent OnAttackEnd;

    [Header("Interact")]
    public UnityEvent OnInteractBegin;
    public UnityEvent OnInteractEnd;
    public UnityEvent Back;

    [Header("DPAD PRESS")]
    public UnityEvent OnPress_Dpad_Up;
    public UnityEvent OnPress_Dpad_Down;
    public UnityEvent OnPress_Dpad_Left;
    public UnityEvent OnPress_Dpad_Right;

    [Header("DPAD RELEASE")]
    public UnityEvent OnRelease_Dpad_Up;
    public UnityEvent OnRelease_Dpad_Down;
    public UnityEvent OnRelease_Dpad_Left;
    public UnityEvent OnRelease_Dpad_Right;

    [Header("Test actives")]
    public UnityEvent OnUseActive;
    public UnityEvent SwitchActive;
    public UnityEvent OverTheSholder;
    public EventInt SendIndexAlphanumeric;

    public UnityEvent LockON;
    public UnityEvent NextON;

    public UnityEvent RTrigger;
    public UnityEvent RTrigger_Release;
    public UnityEvent LTrigger;
    public UnityEvent LTrigger_Release;

    public UnityEvent R_Stick_Central_Button;
    public UnityEvent L_Stick_Central_Button;

    public InputControl inputControlCheck;
    bool isJoystick;


    private void Awake() => ConfigureJoystickHelper();


    int currentAxisIndex = 0;
    public void ChangeCameraIndex(int index) => currentAxisIndex = index;

    private void Start()
    {
        
        LoadSceneHandler.instance.OnEndLoad += (x) =>
        {
            if (x != Menu && x != Load && inMenu)
                inMenu = false;
        };
    }
    public bool inMenu = true;
    public bool inCinematic = false;

    private void Update()
    {
        if (!isJoystick)
        {
            if (inputControlCheck.GetInputState() == InputControl.eInputState.Controler)
            {
                input_type = InputType.Joystick;
                isJoystick = true;

                InputImageDatabase.instance.ChangeInput(InputImageDatabase.InputImageCode.joystick);
                Main.instance.eventManager.TriggerEvent(GameEvents.CHANGE_INPUT, "Joystick");
                SendMessage(isJoystick);
            }
        }
        else
        {
            if (inputControlCheck.GetInputState() == InputControl.eInputState.MouseKeyboard)
            {
                input_type = InputType.Mouse;
                isJoystick = false;


                InputImageDatabase.instance.ChangeInput(InputImageDatabase.InputImageCode.mouse);
                Main.instance.eventManager.TriggerEvent(GameEvents.CHANGE_INPUT, "Mouse");
                SendMessage(isJoystick);
            }
        }

        if (inMenu)
        {
            return;
        }

        LeftHorizontal.Invoke(Input.GetAxis("Horizontal"));
        LeftVertical.Invoke(Input.GetAxis("Vertical"));

        if (Mathf.Abs(Input.GetAxis(InputControl.HORIZONTAL)) > 0 || Mathf.Abs(Input.GetAxis(InputControl.VERTICAL)) > 0)
        {
            LeftHorizontal.Invoke(Input.GetAxis(InputControl.HORIZONTAL));
            LeftVertical.Invoke(Input.GetAxis(InputControl.VERTICAL));
        }

        if (input_type == InputType.Joystick) JoystickInputs();
        else if (input_type == InputType.Mouse) { MouseInputs(); ScrollWheel(); }
        if (Input.GetButtonDown("Dash")) Dash.Invoke();

        if (Input.GetButtonDown("Block")) OnBlock.Invoke();
        if (Input.GetButtonUp("Block")) UpBlock.Invoke();
        
        if (Input.GetButtonDown("Attack")) OnAttack.Invoke();
        if (Input.GetButtonUp("Attack")) OnAttackEnd.Invoke();

        if (Input.GetButtonDown("Interact")) OnInteractBegin.Invoke();
        if (Input.GetButtonUp("Interact")) OnInteractEnd.Invoke();
        if (Input.GetButtonDown("OpenInventory")) Back.Invoke();

        //porque le manda un flotante??
        ChangeWeapon.Invoke(Input.GetAxis("XBOX360_DPadHorizontal"));

        if (Input.GetKeyDown(KeyCode.Alpha1)) EV_PRESS_DPAD_UP();
        if (Input.GetKeyDown(KeyCode.Alpha4)) EV_PRESS_DPAD_LEFT();
        if (Input.GetKeyDown(KeyCode.Alpha3)) EV_PRESS_DPAD_DOWN();
        if (Input.GetKeyDown(KeyCode.Alpha2)) EV_PRESS_DPAD_RIGHT();

        if (Input.GetKeyUp(KeyCode.Alpha1)) EV_RELEASE_DPAD_UP();
        if (Input.GetKeyUp(KeyCode.Alpha4)) EV_RELEASE_DPAD_LEFT();
        if (Input.GetKeyUp(KeyCode.Alpha3)) EV_RELEASE_DPAD_DOWN();
        if (Input.GetKeyUp(KeyCode.Alpha2)) EV_RELEASE_DPAD_RIGHT();

        if (Input.GetKeyDown(KeyCode.K)) OverTheSholder.Invoke();

        if (Input.GetButtonDown("Skill")) OnUseActive.Invoke();

        if (Input.GetButtonDown("LockOn")) LockON.Invoke();

        if (Input.GetKeyDown(KeyCode.Alpha1)) R_Stick_Central_Button.Invoke();

        if (Input.GetButtonDown("SwitchActive")) SwitchActive.Invoke();

        if (Input.GetButtonDown("Pause")) PauseManager.Instance.PauseHud();

        if (Input.GetKeyDown(KeyCode.E)) RTrigger.Invoke();
        if (Input.GetKeyDown(KeyCode.Q)) LTrigger.Invoke();
        if (Input.GetKeyUp(KeyCode.E)) RTrigger_Release.Invoke();
        if (Input.GetKeyUp(KeyCode.Q)) LTrigger_Release.Invoke();

        if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl)) SwitchActive.Invoke();

            RefreshHelper();
    }

    public void MouseInputs()
    {
        RightHorizontal.Invoke(Input.GetAxis("Mouse X"));
        RightVertical.Invoke(Input.GetAxis("Mouse Y"));

    }
    public void JoystickInputs()
    {
        RightHorizontal.Invoke(Input.GetAxis("RightHorizontal"));
        RightVertical.Invoke(Input.GetAxis("RightVertical"));
    }

    public void ScrollWheel() => MouseScrollWheel.Invoke(Input.GetAxis("Mouse ScrollWheel"));

    #region Change Input & Message
    public JoystickMessage joystickMessage;
    public void SendMessage(bool _isJoystick)
    {
        joystickMessage.Open();
        joystickMessage.Message(_isJoystick);
    }
    //public string ChangeRotation(bool value)
    //{
    //    if (value)
    //    {
    //        input_type = InputType.Mouse;
    //        return "teclado y raton detectado";
    //    }
    //    else
    //    {
    //        input_type = InputType.Joystick;
    //        return "joystick detectado";
    //    }
    //}
    #endregion



    #region JoystickHelper
    void ConfigureJoystickHelper()
    {
        joystickhelper = new JoystickBasicInput();
        joystickhelper

            .SUBSCRIBE_PRESS_DPAD_UP(EV_PRESS_DPAD_UP)
            .SUBSCRIBE_PRESS_DPAD_DOWN(EV_PRESS_DPAD_DOWN)
            .SUBSCRIBE_PRESS_DPAD_RIGHT(EV_PRESS_DPAD_RIGHT)
            .SUBSCRIBE_PRESS_DPAD_LEFT(EV_PRESS_DPAD_LEFT)

            .SUBSCRIBE_RELEASE_DPAD_UP(EV_RELEASE_DPAD_UP)
            .SUBSCRIBE_RELEASE_DPAD_DOWN(EV_RELEASE_DPAD_DOWN)
            .SUBSCRIBE_RELEASE_DPAD_RIGHT(EV_RELEASE_DPAD_RIGHT)
            .SUBSCRIBE_RELEASE_DPAD_LEFT(EV_RELEASE_DPAD_LEFT)

            .SUBSCRIBE_PRESS_LTRIGGER(EV_DPAD_RTRIGGER)
            .SUBSCRIBE_PRESS_RTRIGGER(EV_DPAD_LTRIGGER)
            .SUBSCRIBE_RELEASE_LTRIGGER(EV_DPAD_RTRIGGER_RELEASE)
            .SUBSCRIBE_RELEASE_RTRIGGER(EV_DPAD_LTRIGGER_RELEASE)
            .SUBSCRIBE_R_STICK_BTN_CENTRAL(R_Stick_Central_Button.Invoke)
            .SUBSCRIBE_L_STICK_BTN_CENTRAL(L_Stick_Central_Button.Invoke)
            ;
    }
    void RefreshHelper() => joystickhelper.Refresh();
    void EV_PRESS_DPAD_UP() { OnPress_Dpad_Up.Invoke(); }
    void EV_PRESS_DPAD_DOWN() { OnPress_Dpad_Down.Invoke(); }
    void EV_PRESS_DPAD_LEFT() { OnPress_Dpad_Left.Invoke(); }
    void EV_PRESS_DPAD_RIGHT() { OnPress_Dpad_Right.Invoke(); }
    void EV_RELEASE_DPAD_UP() { OnRelease_Dpad_Up.Invoke(); }
    void EV_RELEASE_DPAD_DOWN() { OnRelease_Dpad_Down.Invoke(); }
    void EV_RELEASE_DPAD_LEFT() { OnRelease_Dpad_Left.Invoke(); }
    void EV_RELEASE_DPAD_RIGHT() { OnRelease_Dpad_Right.Invoke(); }
    void EV_DPAD_LTRIGGER() { LTrigger.Invoke(); }
    void EV_DPAD_LTRIGGER_RELEASE() { LTrigger_Release.Invoke(); }
    void EV_DPAD_RTRIGGER() { RTrigger.Invoke(); }
    void EV_DPAD_RTRIGGER_RELEASE() { RTrigger_Release.Invoke(); }
    #endregion


    [System.Serializable]
    public class UnityEvFloat : UnityEvent<float> { }
}
