using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bindings : MonoBehaviour
{
    CustomInput customInput;

    private void Start()
    {
        customInput = new CustomInput();

        customInput.SubscribeMeTo(GameActions.Right_Hand, InputEventAction.Down, ExampleBinding);
    }

    void ExampleBinding(float axis)
    {

    }
}
