using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Tools.EventClasses;
using UnityEngine.SceneManagement;

public class CharacterInput : MonoBehaviour
{
    public enum InputType { Joystick, Mouse, Other }
    public InputType input_type;

    JoystickBasicInput joystickhelper;

    [Header("WASD / L_STICK")]
    public UnityEvFloat LeftHorizontal;
    public UnityEvFloat LeftVertical;

    [Header("MOUSE_ROT / R_STICK")]
    public UnityEvFloat RightHorizontal;
    public UnityEvFloat RightVertical;

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

    [Header("Test actives")]
    public UnityEvent OnDpad_Up;
    public UnityEvent OnDpad_Down;
    public UnityEvent OnDpad_Left;
    public UnityEvent OnDpad_Right;
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

    //DirConfig dirConfig = new DirConfig();
    //struct DirConfig
    //{
    //    public string H_name;
    //    public int H_Multiplier;
    //    public string V_name;
    //    public int V_Multiplier;
    //}

    private void Awake() => ConfigureJoystickHelper();

    //void ConfigureAxis()
    //{
    //    dirConfig.H_name = "Horizontal";
    //    dirConfig.H_Multiplier = 1;
    //    dirConfig.V_name = "Vertical";
    //    dirConfig.V_Multiplier = 1;
    //}
    //void ConfigureAxisRotations()
    //{
    //    dirConfig.H_name = "RightHorizontal";
    //    dirConfig.H_Multiplier = 1;
    //    dirConfig.V_name = "RightVertical";
    //    dirConfig.V_Multiplier = 1;     
    //}

    int currentAxisIndex = 0;
    public void ChangeCameraIndex(int index) => currentAxisIndex = index;

    private void Start()
    {
        DevelopTools.UI.Debug_UI_Tools.instance.CreateToogle("Input de Rotacion", true, ChangeRotation);
        LoadSceneHandler.instance.OnEndLoad += (x) =>
        {
            if (x != 12 && x != 0 && inMenu)
                inMenu = false;
        };
    }
    public bool inMenu = true;

    private void Update()
    {
        if (inMenu) return;

        //var HorizontalAux = Input.GetAxis(dirConfig.H_name);
        //var VerticalAux = Input.GetAxis(dirConfig.V_name);
        //var HMult = dirConfig.H_Multiplier;
        //var VMult = dirConfig.V_Multiplier;

        //LeftHorizontal.Invoke( HorizontalAux * HMult);
        //LeftVertical.Invoke(VerticalAux  * VMult);

        LeftHorizontal.Invoke(Input.GetAxis("Horizontal"));
        LeftVertical.Invoke(Input.GetAxis("Vertical"));
        if (Mathf.Abs(Input.GetAxis(InputControl.HORIZONTAL)) > 0 || Mathf.Abs(Input.GetAxis(InputControl.VERTICAL)) > 0)
        {
            LeftHorizontal.Invoke(Input.GetAxis(InputControl.HORIZONTAL));
            LeftVertical.Invoke(Input.GetAxis(InputControl.VERTICAL));
        }
        
        if (input_type == InputType.Joystick) JoystickInputs();
        else if (input_type == InputType.Mouse) MouseInputs();
        if (Input.GetButtonDown("Dash")) Dash.Invoke();

        if (Input.GetButtonDown("Block")) OnBlock.Invoke();
        if (Input.GetButtonUp("Block")) UpBlock.Invoke();
        
        if (Input.GetButtonDown("Attack")) OnAttack.Invoke();
        if (Input.GetButtonUp("Attack")) OnAttackEnd.Invoke();

        if (Input.GetButtonDown("Interact")) OnInteractBegin.Invoke();
        if (Input.GetButtonUp("Interact")) OnInteractEnd.Invoke();

        //porque le manda un flotante??
        ChangeWeapon.Invoke(Input.GetAxis("XBOX360_DPadHorizontal"));

        if (Input.GetKeyDown(KeyCode.Alpha1)) EV_DPAD_UP();
        if (Input.GetKeyDown(KeyCode.Alpha2)) EV_DPAD_LEFT();
        if (Input.GetKeyDown(KeyCode.Alpha3)) EV_DPAD_DOWN();
        if (Input.GetKeyDown(KeyCode.Alpha4)) EV_DPAD_RIGHT();
        if (Input.GetKeyDown(KeyCode.K)) OverTheSholder.Invoke();
        if (Input.GetButtonDown("Back")) Back.Invoke();

        if (Input.GetButtonDown("Skill")) OnUseActive.Invoke();

        if (Input.GetButtonDown("LockOn")) LockON.Invoke();

        if (Input.GetKeyDown(KeyCode.Alpha1)) R_Stick_Central_Button.Invoke();
        if (Input.GetKeyDown(KeyCode.Alpha2)) L_Stick_Central_Button.Invoke();

        if (Input.GetButtonDown("SwitchActive")) SwitchActive.Invoke();

        if (Input.GetButtonDown("Pause")) PauseManager.Instance.Pause();

        if (Input.GetKeyDown(KeyCode.E)) RTrigger.Invoke();
        if (Input.GetKeyDown(KeyCode.Q)) LTrigger.Invoke();

        if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl)) SwitchActive.Invoke();

            RefreshHelper();

        if (!isJoystick) {
            if (inputControlCheck.GetInputState() == InputControl.eInputState.Controler) 
            {
                input_type = InputType.Joystick;
                isJoystick = true;

                InputImageDatabase.instance.ChangeInput(InputImageDatabase.InputImageCode.joystick);
                Main.instance.eventManager.TriggerEvent(GameEvents.CHANGE_INPUT, "Joystick");
                SendMessage(isJoystick);
            }
        }
        else {
            if (inputControlCheck.GetInputState() == InputControl.eInputState.MouseKeyboard) 
            {
                input_type = InputType.Mouse;
                isJoystick = false;

                
                InputImageDatabase.instance.ChangeInput(InputImageDatabase.InputImageCode.mouse);
                Main.instance.eventManager.TriggerEvent(GameEvents.CHANGE_INPUT, "Mouse");
                SendMessage(isJoystick);
            }
        }
    }

    public JoystickMessage joystickMessage;

    public void SendMessage(bool _isJoystick)
    {
        joystickMessage.Open();
        joystickMessage.Message(_isJoystick);
    }

    public void MouseInputs()
    {
        //Vector2 positionOnScreen = Camera.main.WorldToViewportPoint(transform.position);
        //Vector2 mouseOnScreen = (Vector2)Camera.main.ScreenToViewportPoint(Input.mousePosition);
        //Vector2 mousePos = (mouseOnScreen - positionOnScreen).normalized;
        //RightHorizontal.Invoke(mousePos.x);
        //RightVertical.Invoke(mousePos.y);

        RightHorizontal.Invoke(Input.GetAxis("Mouse X"));
        RightVertical.Invoke(Input.GetAxis("Mouse Y"));

    }

    public void JoystickInputs()
    {
        //var HorizontalAux = Input.GetAxis(dirConfigsRotation[currentAxisIndex].H_name);
        //var VerticalAux = Input.GetAxis(dirConfigsRotation[currentAxisIndex].V_name);
        //var HMult = dirConfigsRotation[currentAxisIndex].H_Multiplier;
        //var VMult = dirConfigsRotation[currentAxisIndex].V_Multiplier;

        //RightHorizontal.Invoke(HorizontalAux * HMult);
        //RightVertical.Invoke(VerticalAux * VMult);
        RightHorizontal.Invoke(Input.GetAxis("RightHorizontal"));
        RightVertical.Invoke(Input.GetAxis("RightVertical"));

    }

    /// <summary>
    /// su es true usa mouse
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public string ChangeRotation(bool value)
    {
        if (value)
        {
            input_type = InputType.Mouse;
            return "teclado y raton detectado";
        }
        else
        {
            input_type = InputType.Joystick;
            return "joystick detectado";
        }
    }

    

    #region JoystickHelper
    void ConfigureJoystickHelper()
    {
        joystickhelper = new JoystickBasicInput();
        joystickhelper
            .SUBSCRIBE_DPAD_UP(EV_DPAD_UP)
            .SUBSCRIBE_DPAD_DOWN(EV_DPAD_DOWN)
            .SUBSCRIBE_DPAD_RIGHT(EV_DPAD_RIGHT)
            .SUBSCRIBE_DPAD_LEFT(EV_DPAD_LEFT)
            .SUBSCRIBE_LTRIGGER(EV_DPAD_RTRIGGER)
            .SUBSCRIBE_RTRIGGER(EV_DPAD_LTRIGGER)
            .SUBSCRIBE_LTRIGGER_RELEASE(EV_DPAD_RTRIGGER_RELEASE)
            .SUBSCRIBE_RTRIGGER_RELEASE(EV_DPAD_LTRIGGER_RELEASE)
            .SUBSCRIBE_R_STICK_BTN_CENTRAL(R_Stick_Central_Button.Invoke)
            .SUBSCRIBE_L_STICK_BTN_CENTRAL(L_Stick_Central_Button.Invoke)
            ;
    }
    void RefreshHelper() => joystickhelper.Refresh();
    void EV_DPAD_UP() { OnDpad_Up.Invoke(); }
    void EV_DPAD_DOWN() { OnDpad_Down.Invoke(); }
    void EV_DPAD_LEFT() { OnDpad_Left.Invoke(); }
    void EV_DPAD_RIGHT() { OnDpad_Right.Invoke(); }
    void EV_DPAD_LTRIGGER() { LTrigger.Invoke(); }
    void EV_DPAD_LTRIGGER_RELEASE() { LTrigger_Release.Invoke(); }
    void EV_DPAD_RTRIGGER() { RTrigger.Invoke(); }
    void EV_DPAD_RTRIGGER_RELEASE() { RTrigger_Release.Invoke(); }

    #endregion


    [System.Serializable]
    public class UnityEvFloat : UnityEvent<float> { }
}


