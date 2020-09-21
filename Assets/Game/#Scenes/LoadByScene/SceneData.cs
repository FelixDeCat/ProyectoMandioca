using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SceneData", menuName = "Scenes/Streaming/SceneData", order = 1)]
public class SceneData : ScriptableObject
{
    public string[] scenes_to_view;
}
