using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ShieldFalling_AnimHelper : MonoBehaviour
{
    public UnityEvent e;

    public void StartFallingShield()
    {
        GetComponent<Animator>().SetTrigger("FallingShield");

        
    }

    public void FallingFloor()
    {
        e?.Invoke();
    }


}
