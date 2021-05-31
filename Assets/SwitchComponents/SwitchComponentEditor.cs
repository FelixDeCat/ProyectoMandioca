#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SwitchComponent))]
public class SwitchComponentEditor : Editor
{
    public SwitchComponentType type = SwitchComponentType.Both;

    SwitchComponent obj;

    SerializedProperty UE_OnTurnOn;
    SerializedProperty UE_OnTurnOff;
    SerializedProperty UE_OnFade;
    GUIStyle style;

    void OnEnable()
    {
        obj = (SwitchComponent)target;

        UE_OnTurnOn = serializedObject.FindProperty("EV_Turn_On");
        UE_OnTurnOff = serializedObject.FindProperty("EV_Turn_Off");
        UE_OnFade = serializedObject.FindProperty("EV_OnFade");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        #region GUI Styles
        GUIStyle title_Style = new GUIStyle();
        title_Style.fontStyle = FontStyle.Bold;
        title_Style.font = Font.CreateDynamicFontFromOSFont("Calibri", 20);
        title_Style.alignment = TextAnchor.MiddleCenter;

        GUIStyle description_Style = new GUIStyle();
        description_Style.fontStyle = FontStyle.Italic;
        description_Style.font = Font.CreateDynamicFontFromOSFont("Arial", 15);
        description_Style.alignment = TextAnchor.MiddleCenter;

        GUIStyle item_style = new GUIStyle();
        item_style.fontStyle = FontStyle.Bold;
        item_style.font = Font.CreateDynamicFontFromOSFont("Consolas", 20);
        item_style.alignment = TextAnchor.MiddleCenter;

        GUIStyleState pressed = new GUIStyleState();
        pressed.textColor = Color.green;
        GUIStyleState normal = new GUIStyleState();
        pressed.textColor = Color.black;
        GUIStyleState hover = new GUIStyleState();
        pressed.textColor = Color.blue;

        #endregion
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Usar Objeto con \"ISwitcheable\""))
        {
            obj.UseASwitcheableAuxiliars = true;
        }
        if (GUILayout.Button("Usar Unity Events"))
        {
            obj.UseASwitcheableAuxiliars = false;
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space();
        EditorGUILayout.Space();

        if (obj.UseASwitcheableAuxiliars)
        {
            EditorGUILayout.LabelField("----- ISwitcheables -----", title_Style, GUILayout.Height(40));
            EditorGUILayout.Space();
            if (GUILayout.Button("Buscar y agregar"))
            {
                var comps = obj.GetComponents<Switcheable>();

                obj.switcheables = new Switcheable[comps.Length];
                for (int i = 0; i < obj.switcheables.Length; i++)
                {
                    obj.switcheables[i] = comps[i];
                }

                serializedObject.ApplyModifiedProperties();
            }
            EditorGUILayout.Space();

            if (obj.switcheables == null)
            {
                EditorGUILayout.LabelField("No se encontró nada, asegurate que\n  el script Attacheado implemente \"ISwitcheable\"", description_Style, GUILayout.Height(40));
                return;
            }
            if (obj.switcheables.Length > 0)
            {
                EditorGUILayout.LabelField("Objetos encontrados", description_Style, GUILayout.Height(20));
                for (int i = 0; i < obj.switcheables.Length; i++)
                {
                    EditorGUILayout.LabelField(i + ". " + obj.switcheables[i].ToString(), item_style, GUILayout.Height(20));
                }
            }
            else
            {
                EditorGUILayout.LabelField("No se encontró nada, asegurate que\n  el script Attacheado implemente \"ISwitcheable\"", description_Style, GUILayout.Height(40));
            }
        }
        else
        {
            EditorGUILayout.LabelField("----- Unity Events -----", title_Style, GUILayout.Height(40));

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Ambos"))
            {
                obj.type = SwitchComponentType.Both;
            }
            if (GUILayout.Button("Switch"))
            {
                obj.type = SwitchComponentType.Switch;
            }
            if (GUILayout.Button("Fade"))
            {
                obj.type = SwitchComponentType.Fade;
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();

            EditorGUILayout.BeginVertical();
            switch (obj.type)
            {
                case SwitchComponentType.Switch:
                    EditorGUILayout.LabelField("SWITCH", title_Style, GUILayout.Height(20));
                    EditorGUILayout.LabelField("Si solo activas y desactivas cosas podés usar esto \n directamente sobre el objeto", description_Style, GUILayout.Height(40));
                    EditorGUILayout.Space();
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.PropertyField(UE_OnTurnOn);
                    EditorGUILayout.PropertyField(UE_OnTurnOff);
                    EditorGUILayout.EndHorizontal();
                    break;
                case SwitchComponentType.Fade:
                    EditorGUILayout.LabelField("FADE", title_Style, GUILayout.Height(20));
                    EditorGUILayout.LabelField("Necesita una clase auxiliar que tenga una funcion \n con parametro (float) [0] apagado [1] prendido", description_Style, GUILayout.Height(40));
                    EditorGUILayout.Space();
                    EditorGUILayout.PropertyField(UE_OnFade);
                    break;
                case SwitchComponentType.Both:
                    EditorGUILayout.LabelField("AMBOS", title_Style, GUILayout.Height(20));
                    EditorGUILayout.LabelField("[SWITCH] Si solo activas y desactivas cosas podés usar esto \n directamente sobre el objeto", description_Style, GUILayout.Height(40));
                    EditorGUILayout.LabelField("[FADE] Necesita una clase auxiliar que tenga una funcion \n con parametro (float) [0] apagado [1] prendido", description_Style, GUILayout.Height(40));
                    EditorGUILayout.Space();
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.PropertyField(UE_OnTurnOn);
                    EditorGUILayout.PropertyField(UE_OnTurnOff);
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.PropertyField(UE_OnFade);
                    break;
            }
            EditorGUILayout.EndVertical();
        }

        serializedObject.ApplyModifiedProperties();
    }
}
#endif