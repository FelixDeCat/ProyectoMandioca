using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Choice
{
    public string text;
    public int connectionID;

    public Choice(string t, int c)
    {
        text = t;
        connectionID = c;
    }
}
