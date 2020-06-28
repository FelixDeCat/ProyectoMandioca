using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScriptEvent_WendigoDeath : MonoBehaviour
{
    public UnityEvent WendigoDeath;

    public void Execute()
    {
        WendigoDeath.Invoke();
    }
}
