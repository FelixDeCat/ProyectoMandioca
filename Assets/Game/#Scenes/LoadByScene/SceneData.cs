using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SceneData", menuName = "Scenes/Streaming/SceneData", order = 1)]
public class SceneData : ScriptableObject
{
    public string[] scenes_to_reset;
    public SceneParameter[] sceneparam;
    public GameObject landmark;
    public GameObject gameplay;
    public GameObject low_detail;
    public GameObject medium_detail;
    public GameObject hight_detail;

    //EditorVariables
    [HideInInspector]
    public GameObject landmarkInScene;
    [HideInInspector]
    public GameObject gameplayInScene;
    [HideInInspector]
    public GameObject low_detailInScene;
    [HideInInspector]
    public GameObject medium_detailInScene;
    [HideInInspector]
    public GameObject hightDetailInScene;
    public enum Detail_Parameter
    {
        none,
        full_load,
        top_to_landmark,
        top_to_low,
        top_to_medium
    }
    [System.Serializable]
    public class SceneParameter
    {
        public string scene;
        public Detail_Parameter detail;
    }
}
