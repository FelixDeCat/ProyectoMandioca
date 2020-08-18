using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealExecute : MonoBehaviour
{
    public int healval;

    public void Heal()
    {
        Main.instance.GetChar().Life.Heal(healval);
    }
}
