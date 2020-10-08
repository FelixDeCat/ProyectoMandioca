using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.SceneManagement;

[CustomEditor(typeof(LocalSceneHandler))]
public class LocalSceneHandlerEditor : Editor
{
    LocalSceneHandler _handler;
    string sceneName;
    SceneData _data { get => _handler.SceneData; set => _handler.SceneData = value; }
    GameObject _landmark { get => _handler.SceneData.landmarkInScene; set => _handler.SceneData.landmarkInScene = value; }
    GameObject _gameplay { get => _handler.SceneData.gameplayInScene; set => _handler.SceneData.gameplayInScene = value; }
    GameObject _lowdetail { get => _handler.SceneData.low_detailInScene; set => _handler.SceneData.low_detailInScene = value; }
    GameObject _mediumDetail { get => _handler.SceneData.medium_detailInScene; set => _handler.SceneData.medium_detailInScene = value; }
    GameObject _highDetail { get => _handler.SceneData.hightDetailInScene; set => _handler.SceneData.hightDetailInScene = value; }
    void OnEnable()
    {
        _handler = (LocalSceneHandler)target;
        sceneName = _handler.gameObject.scene.name;

    }
    public override void OnInspectorGUI()
    {

        //Checkea si hay un scriptableobject en el espacio
        if (!_handler.SceneData)
        {
            _data = EditorGUILayout.ObjectField("my data:", _data, typeof(SceneData), false) as SceneData;
        }
        else
        {
            //checkea nombre de escena y del gameobject
            if (sceneName == _handler.gameObject.name)
            {
                Debug.Log("checked");
            }
            else
            {
                _handler.gameObject.name = sceneName;
            }

            //Forma de ver si las variables cargaron. Eso solo visual
            EditorGUI.BeginDisabledGroup(true);

            _data = EditorGUILayout.ObjectField("my data:", _data, typeof(SceneData), false) as SceneData;

            EditorGUILayout.Space();

            _landmark = EditorGUILayout.ObjectField("Landmark:", _landmark, typeof(GameObject), false) as GameObject;
            _gameplay = EditorGUILayout.ObjectField("Lowdetail:", _gameplay, typeof(GameObject), false) as GameObject;
            _lowdetail = EditorGUILayout.ObjectField("Lowdetail:", _lowdetail, typeof(GameObject), false) as GameObject;
            _mediumDetail = EditorGUILayout.ObjectField("MediumDetail:", _mediumDetail, typeof(GameObject), false) as GameObject;
            _highDetail = EditorGUILayout.ObjectField("HighDetail:", _highDetail, typeof(GameObject), false) as GameObject;

            EditorGUI.EndDisabledGroup();

            //Debug. Te limpia las referencias para que despues puedas cargarlo de nuevo
            if (GUILayout.Button("Reset Variables"))
            {

            }
            //Boton de carga
            if (GUILayout.Button("Load Scene"))
            {
                string path;
                Scene currentScene = _handler.gameObject.scene;
                //Landmark
                path = AssetDatabase.GetAssetPath(_handler.SceneData.landmark);
                _landmark = PrefabUtility.InstantiatePrefab(_handler.SceneData.landmark, currentScene) as GameObject;
                PrefabUtility.UnpackPrefabInstance(_landmark, PrefabUnpackMode.OutermostRoot, InteractionMode.AutomatedAction);

                //Gameplay
                path = AssetDatabase.GetAssetPath(_handler.SceneData.gameplay);
                _gameplay = PrefabUtility.InstantiatePrefab(_handler.SceneData.gameplay, currentScene) as GameObject;
                PrefabUtility.UnpackPrefabInstance(_gameplay, PrefabUnpackMode.OutermostRoot, InteractionMode.AutomatedAction);

                //Low Detail
                path = AssetDatabase.GetAssetPath(_handler.SceneData.low_detail);
                _lowdetail = PrefabUtility.InstantiatePrefab(_handler.SceneData.low_detail, currentScene) as GameObject;
                PrefabUtility.UnpackPrefabInstance(_lowdetail, PrefabUnpackMode.OutermostRoot, InteractionMode.AutomatedAction);

                //Medium Detail
                path = AssetDatabase.GetAssetPath(_handler.SceneData.medium_detail);
                _mediumDetail = PrefabUtility.InstantiatePrefab(_handler.SceneData.medium_detail, currentScene) as GameObject;
                PrefabUtility.UnpackPrefabInstance(_mediumDetail, PrefabUnpackMode.OutermostRoot, InteractionMode.AutomatedAction);

                //HighDetail
                path = AssetDatabase.GetAssetPath(_handler.SceneData.hight_detail);
                _highDetail = PrefabUtility.InstantiatePrefab(_handler.SceneData.hight_detail, currentScene) as GameObject;
                PrefabUtility.UnpackPrefabInstance(_highDetail, PrefabUnpackMode.OutermostRoot, InteractionMode.AutomatedAction);

            }

            //Boton de guardado y descarga. Solo se borra si guardo
            if (GUILayout.Button("Unload"))
            {
                bool success;
                if (_landmark)
                {
                    var path = AssetDatabase.GetAssetPath(_handler.SceneData.landmark);
                    PrefabUtility.SaveAsPrefabAsset(_landmark, path, out success);
                    if (success == true)
                    {
                        DestroyImmediate(_landmark);
                    }
                    else
                    {
                        Debug.LogWarning("Scene " + sceneName + " failed to save the Landmark! Try again :c");
                    }
                }
                if (_gameplay)
                {
                    var path = AssetDatabase.GetAssetPath(_handler.SceneData.gameplay);
                    PrefabUtility.SaveAsPrefabAsset(_gameplay, path, out success);
                    if (success == true)
                    {
                        DestroyImmediate(_gameplay);
                    }
                    else
                    {
                        Debug.LogWarning("Scene " + sceneName + " failed to save the Gameplay! Try again :c");
                    }
                }
                if (_lowdetail)
                {
                    var path = AssetDatabase.GetAssetPath(_handler.SceneData.low_detail);
                    PrefabUtility.SaveAsPrefabAsset(_lowdetail, path, out success);
                    if (success == true)
                    {
                        DestroyImmediate(_lowdetail);
                    }
                    else
                    {
                        Debug.LogWarning("Scene " + sceneName + " failed to save the Low Detail! Try again :c");
                    }
                }
                if (_mediumDetail)
                {
                    var path = AssetDatabase.GetAssetPath(_handler.SceneData.medium_detail);
                    PrefabUtility.SaveAsPrefabAsset(_mediumDetail, path, out success);
                    if (success == true)
                    {
                        DestroyImmediate(_mediumDetail);
                    }
                    else
                    {
                        Debug.LogWarning("Scene " + sceneName + " failed to save the Medium Detail! Try again :c");
                    }
                }
                if (_highDetail)
                {
                    var path = AssetDatabase.GetAssetPath(_handler.SceneData.hight_detail);
                    PrefabUtility.SaveAsPrefabAsset(_highDetail, path, out success);
                    if (success == true)
                    {
                        DestroyImmediate(_highDetail);
                    }
                    else
                    {
                        Debug.LogWarning("Scene " + sceneName + " failed to save the High Detail! Try again :c");
                    }
                }

            }
        }
    }
    public void ResetVariables()
    {
        _landmark = null;
        _gameplay = null;
        _lowdetail = null;
        _mediumDetail = null;
        _highDetail = null;

    }
}
