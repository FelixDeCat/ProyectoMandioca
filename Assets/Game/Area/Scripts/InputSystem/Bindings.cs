using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bindings : MonoBehaviour
{
    CustomInput customInput;

    private void Start()
    {
        customInput = new CustomInput();
        customInput.Initialize();
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
