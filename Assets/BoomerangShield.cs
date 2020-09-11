using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerangShield : MonoBehaviour
{
    ///////////////////////////////////////
    //  USE
    ///////////////////////////////////////
    public void OnPress()
    {
        Debug.Log("ON PRESS");
    }
    public void OnStopUse()
    {
        Debug.Log("ON RELEASE");
    }

    ///////////////////////////////////////
    //  EQUIP
    ///////////////////////////////////////
    public void Equip()
    {
        Debug.Log("EQUIP");
    }
    public void UnEQuip()
    {
        Debug.Log("UNEQUIP");
    }


    ///////////////////////////////////////
    //  EXECUTE SKILL
    ///////////////////////////////////////
    public void OnExecute()
    {
        Debug.Log("EXECUTE");
    }

    private void Update()
    {
        
    }
}

