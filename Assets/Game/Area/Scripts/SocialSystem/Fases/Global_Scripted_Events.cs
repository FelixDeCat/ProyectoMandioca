using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Global_Scripted_Events : MonoBehaviour
{
    public static Global_Scripted_Events instance;
    private void Awake() { instance = this; }
}
