using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveIgnoreFallDamage : MonoBehaviour
{
    public void ActiveIgnore() => Main.instance.GetChar().GetCharMove().ignoreFallDamage = true;
}
