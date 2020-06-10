using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowablePoolsManager : MonoBehaviour
{
    public static ThrowablePoolsManager instance;
    private void Awake() => instance = this;
    
    public Dictionary<string, PoolThrowable> pools = new Dictionary<string, PoolThrowable>();

    public void Throw(string s)
    {

    }

}
