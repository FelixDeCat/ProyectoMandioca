using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Answer_ComboWombo : MonoBehaviour, IAnswerable
{
    public Answer Answer => new Answer(Gate.AND, () => Main.instance.GetChar().IsComboWomboActive );

    
}
