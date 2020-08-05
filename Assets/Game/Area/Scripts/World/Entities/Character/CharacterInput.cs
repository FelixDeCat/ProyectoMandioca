using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Tools.EventClasses;

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

    public EventInt SendIndexAlphanumeric;

    public UnityEvent LockON;
    public UnityEvent NextON;

    public UnityEvent RTrigger;
    public UnityEvent LTrigger;

    public InputControl inputControlCheck;
    bool isJoystick;

    DirConfig[] dirConfigs = new DirConfig[4];
    DirConfig[] dirConfigsRotation = new DirConfig[4];
    struct DirConfig
    {
        public string H_name;
        public int H_Multiplier;
        public string V_name;
        public int V_Multiplier;
    }

    private void Awake() => ConfigureJoystickHelper();

    void ConfigureAxis()
    {
        //arriba abajo
        DirConfig down = new DirConfig();
        down.H_name = "Horizontal";
        down.H_Multiplier = 1;
        down.V_name = "Vertical";
        down.V_Multiplier = 1;
        dirConfigs[0] = down;

        DirConfig up = new DirConfig();
        up.H_name = "Horizontal";
        up.H_Multiplier = -1;
        up.V_name = "Vertical";
        up.V_Multiplier = -1;
        dirConfigs[2] = up;

        //derecha izquierda
        DirConfig right = new DirConfig();
        right.H_name = "Vertical";
        right.H_Multiplier = -1;
        right.V_name = "Horizontal";
        right.V_Multiplier = 1;
        dirConfigs[1] = right;

        DirConfig left = new DirConfig();
        left.H_name = "Vertical";
        left.H_Multiplier = 1;
        left.V_name = "Horizontal";
        left.V_Multiplier = -1;
        dirConfigs[3] = left;
    }
    void ConfigureAxisRotations()
    {
        //arriba abajo
        DirConfig down = new DirConfig();
        down.H_name = "RightHorizontal";
        down.H_Multiplier = 1;
        down.V_name = "RightVertical";
        down.V_Multiplier = 1;
        dirConfigsRotation[0] = down;

        DirConfig up = new DirConfig();
        up.H_name = "RightHorizontal";
        up.H_Multiplier = -1;
        up.V_name = "RightVertical";
        up.V_Multiplier = -1;
        dirConfigsRotation[2] = up;

        //derecha izquierda
        DirConfig right = new DirConfig();
        right.H_name = "RightVertical";
        right.H_Multiplier = -1;
        right.V_name = "RightHorizontal";
        right.V_Multiplier = 1;
        dirConfigsRotation[1] = right;

        DirConfig left = new DirConfig();
        left.H_name = "RightVertical";
        left.H_Multiplier = 1;
        left.V_name = "RightHorizontal";
        left.V_Multiplier = -1;
        dirConfigsRotation[3] = left;
    }

    int currentAxisIndex = 0;
    public void ChangeCameraIndex(int index) => currentAxisIndex = index;

    private void Start()
    {
        DevelopTools.UI.Debug_UI_Tools.instance.CreateToogle("Input de Rotacion", false, ChangeRotation);
        ConfigureAxis();
        ConfigureAxisRotations();
    }
    private void Update()
    {
        Debug.Log("HORIZONTAL NAME: " + dirConfigs[currentAxisIndex].H_name);

        var HorizontalAux = Input.GetAxis(dirConfigs[currentAxisIndex].H_name);
        var VerticalAux = Input.GetAxis(dirConfigs[currentAxisIndex].V_name);
        var HMult = dirConfigs[currentAxisIndex].H_Multiplier;
        var VMult = dirConfigs[currentAxisIndex].V_Multiplier;

        LeftHorizontal.Invoke( HorizontalAux * HMult);
        LeftVertical.Invoke(VerticalAux  * VMult);

        //LeftHorizontal.Invoke(Input.GetAxis("Horizontal"));
        //LeftVertical.Invoke(Input.GetAxis("Vertical"));

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

        if (Input.GetButtonDown("Back")) Back.Invoke();

        if (Input.GetButtonDown("Skill")) OnUseActive.Invoke();

        if (Input.GetButtonDown("LockOn")) LockON.Invoke();

        if (Input.GetKeyDown(KeyCode.Alpha1)) SendIndexAlphanumeric.Invoke(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SendIndexAlphanumeric.Invoke(1);

        if (Input.GetButtonDown("SwitchActive")) SwitchActive.Invoke();

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
        Vector2 positionOnScreen = Camera.main.WorldToViewportPoint(transform.position);
        Vector2 mouseOnScreen = (Vector2)Camera.main.ScreenToViewportPoint(Input.mousePosition);
        Vector2 mousePos = (mouseOnScreen - positionOnScreen).normalized;
        RightHorizontal.Invoke(mousePos.x);
        RightVertical.Invoke(mousePos.y);
    }

    public void JoystickInputs()
    {
        var HorizontalAux = Input.GetAxis(dirConfigsRotation[currentAxisIndex].H_name);
        var VerticalAux = Input.GetAxis(dirConfigsRotation[currentAxisIndex].V_name);
        var HMult = dirConfigsRotation[currentAxisIndex].H_Multiplier;
        var VMult = dirConfigsRotation[currentAxisIndex].V_Multiplier;

        RightHorizontal.Invoke(HorizontalAux * HMult);
        RightVertical.Invoke(VerticalAux * VMult);
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
            .SUBSCRIBE_LTRIGGER(EV_DPAD_LTRIGGER)
            .SUBSCRIBE_RTRIGGER(EV_DPAD_RTRIGGER);
    }
    void RefreshHelper() => joystickhelper.Refresh();
    void EV_DPAD_UP() {  }
    void EV_DPAD_DOWN() {  }
    void EV_DPAD_LEFT() { OnDpad_Left.Invoke(); }
    void EV_DPAD_RIGHT() { OnDpad_Right.Invoke(); }
    void EV_DPAD_LTRIGGER() { LTrigger.Invoke(); }
    void EV_DPAD_RTRIGGER() { RTrigger.Invoke(); }

    #endregion


    [System.Serializable]
    public class UnityEvFloat : UnityEvent<float> { }
}


