using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputImageDatabase : MonoBehaviour
{
    public enum InputImageCode { joystick, mouse }
    SpriteDataBaseInput current;
    public SpriteDataBaseInput Joystick;
    public SpriteDataBaseInput MouseKeyboard;

    public SpriteDataBaseInput GetCurrentInputDataBase() { return current; }
    public void ChangeInput(InputImageCode inputImageCode)
    {
        if (inputImageCode == InputImageCode.joystick) current = Joystick;
        if (inputImageCode == InputImageCode.mouse) current = MouseKeyboard;
    }
}

[System.Serializable]
public class SpriteDataBaseInput
{
    public Sprite move;
    public Sprite rotate;
    public Sprite parry;
    public Sprite normal_attack;
    public Sprite interact;
    public Sprite selectSkill;
    public Sprite useSkill;
    public Sprite evade;
}
