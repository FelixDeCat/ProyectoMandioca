using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerOfPiston : MonoBehaviour
{
    Piston _myPiston;
    public void getPiston(Piston piston)
    {
        _myPiston = piston;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<CharacterHead>())
        {
            _myPiston.active = true;
        }
    }
}
