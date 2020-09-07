using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerangShield : MonoBehaviour
{
    public void OnUse()
    {
        Debug.Log("USE");
    }
    public void OnStopUse()
    {
        Debug.Log("Stop USE");
    }
    public void Equip()
    {
        Debug.Log("EQUIP");
    }
    public void UnEQuip()
    {
        Debug.Log("UNEQUIP");
    }
}

