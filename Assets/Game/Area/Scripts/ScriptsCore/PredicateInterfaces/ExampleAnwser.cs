using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class ExampleAnwser : MonoBehaviour, IAnswerable
{
    public Answer Answer => new Answer(Gate.AND, () => Main.instance.GetChar().IsComboWomboActive);
}
