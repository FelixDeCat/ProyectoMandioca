using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerangShield : MonoBehaviour
{
    public void OnUse()
    {
        Debug.Log("ON PRESS");
    }
    public void OnExecute()
    {
        Debug.Log("EXECUTE");
    }
    public void OnStopUse()
    {
        Debug.Log("ON RELEASE");
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

