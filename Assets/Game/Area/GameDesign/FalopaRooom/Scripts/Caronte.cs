using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Caronte : MonoBehaviour
{

    public event Action OnDefeatCaronte;

    public void ReturnToLife()
    {
        OnDefeatCaronte?.Invoke();
    }
}
