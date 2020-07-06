using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bindings : MonoBehaviour
{
    CustomInput customInput;

    private void Start()
    {
        customInput = new CustomInput();
        customInput.ConfigureInput(GameActions.Right_Hand, UnityJoystickInputNames.BUTTON_A);
        customInput.SubscribeMeTo(GameActions.Right_Hand, new BindingConfig(Attack,InputEventAction.Down));
    }

    void Attack()
    {
        Debug.Log("ATTACK");
    }

    private void Update()
    {
        customInput.Refresh();
    }
}
