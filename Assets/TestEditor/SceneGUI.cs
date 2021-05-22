using UnityEditor;
using UnityEngine;

public class SceneGUI : EditorWindow
{
    [MenuItem("Window/Scene GUI/Enable")]
    public static void Enable()
    {
        SceneView.onSceneGUIDelegate += OnScene;
        Debug.Log("Scene GUI : Enabled");
    }

    [MenuItem("Window/Scene GUI/Disable")]
    public static void Disable()
    {
        SceneView.onSceneGUIDelegate -= OnScene;
        Debug.Log("Scene GUI : Disabled");
    }

    private static void OnScene(SceneView sceneview)
    {
        Handles.BeginGUI();

        Rect rect = new Rect(10, 10, 100, 50);

        if (GUI.Button(rect,"Press Me"))
            Debug.Log("Got it to work.");

        Handles.EndGUI();
    }
}