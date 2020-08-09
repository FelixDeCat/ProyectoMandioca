using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FallingPillar_AnimHelper : MonoBehaviour
{
    public UnityEvent e;

    public void StartFallingPillar()
    {
        GetComponent<Animator>().SetTrigger("FallingPillar");
    }

    public void FallingPillar()
    {
        e?.Invoke();
    }
}
