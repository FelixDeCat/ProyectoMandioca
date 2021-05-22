using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(tests))]
public class Test : Editor
{

    tests myScript;

    void OnEnable()
    {
        myScript = (tests)target;
    }


    
    void OnSceneGUI()
    {
        Handles.BeginGUI();

        Rect rect = new Rect(10, 10, 100, 50);

        if (GUI.Button(rect, "Techos"))
        {
            myScript.Switch();
        }

        //if (Event.current.type == EventType.MouseDown && rect.Contains(Event.current.mousePosition))
        //{
        //    /*
        //    Selection.activeGameObject = myScript.Switch();
        //    */

            
        //}

        Handles.EndGUI();

        //SceneView.RepaintAll();
    }
}

