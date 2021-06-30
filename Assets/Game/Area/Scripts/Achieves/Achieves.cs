using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Achieves", menuName = "Create Achieve")]
public class Achieves : ScriptableObject
{
    public string ID = "";
    public string title = "";
    public string description = "";
    public string blockDescription = "";
    public Sprite achiveImg = null;
}
