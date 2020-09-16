using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ColorRandomizer))]
public class ColorRandomizerEditor : Editor
{
    ColorRandomizer inspector;

    private void OnEnable()
    {
        inspector = (ColorRandomizer)target;
 
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Space(2);

        if(GUILayout.Button("Bake Color"))
        {
            inspector.SetArrays();
        }

        Space(1);

        if (GUILayout.Button("Clear Color"))
        {
            inspector.ClearData();
        }
    }

    void Space(int NumSpace)
    {
        for (int i = 0; i < NumSpace; i++)
        {
            EditorGUILayout.Space();
        }
    }
}
