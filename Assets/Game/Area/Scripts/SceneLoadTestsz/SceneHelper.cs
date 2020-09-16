using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
namespace SM {
    //El que maneja todo en la escena
    public class SceneHelper : CharacterHead{
        [SerializeField]
        public SceneVault handler;
    }

    //El que guarda todo lo de la escena
    [CreateAssetMenu(fileName = "newVault", menuName = "SceneVault", order = 6)]
    public class SceneVault : ScriptableObject {
        public string sceneName;
        public GameObject terrain, rocks, trees, landmarks, details;
        public bool isLoaded;
        
    }

    /*
    [CustomEditor(typeof(SceneHelper))]
    public class SceneHelperEditor : Editor {
        SerializedProperty handler;

        void OnEnable() {
            // Setup the SerializedProperties.
            handler = serializedObject.FindProperty("handler");

        }
        public override void OnInspectorGUI() {
            EditorGUILayout.PropertyField(handler, new GUIContent("handler"));

        }

    }
    */
}