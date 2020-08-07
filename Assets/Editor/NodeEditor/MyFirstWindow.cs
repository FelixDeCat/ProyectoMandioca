using UnityEditor;
using UnityEngine;

public class MyFirstWindow : EditorWindow
{

    string editsabestring;
    Color color;

    [MenuItem("MyWindowsTest/MyFirstWindow")]
    public static void OpenWindow()
    {
        GetWindow<MyFirstWindow>("MiPrimerVentana");
    }

    private void OnGUI()
    {
        GUILayout.Label("este es un label", EditorStyles.boldLabel);

        editsabestring = EditorGUILayout.TextField(editsabestring, editsabestring);

        EditorGUILayout.Space();

        color = EditorGUILayout.ColorField(color);

        if (GUILayout.Button("El boton"))
        {
            if (EditorUtility.DisplayDialog("Color Object selection", "¿Queres colorear la seleccion?", "Bueno dale", "Nehhh"))
            {
                EditorUtility.DisplayDialog("MENSAJOTE", "se acepto", "Ok bro");
                foreach (var o in Selection.gameObjects)
                {
                    o.GetComponent<Renderer>().material.color = color;
                }
            }
            else 
            {
                EditorUtility.DisplayDialog("MENSAJOTE", "se cancelo", "Ok bro");
            }
        }
    }
}
