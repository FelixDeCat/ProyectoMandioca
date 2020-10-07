using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.SceneManagement;

[CustomEditor(typeof(LocalSceneHandler))]
public class LocalSceneHandlerEditor : Editor
{/*
    LocalSceneHandler _handler;
    string sceneName;
    GameObject _landmark { get => _handler.SceneData.landmarkInScene; set => _handler.SceneData.landmarkInScene = value; }
    GameObject _gameplay { get => _handler.SceneData.gameplayInScene; set => _handler.SceneData.gameplayInScene = value; }
    GameObject _lowdetail { get => _handler.SceneData.low_detailInScene; set => _handler.SceneData.low_detailInScene = value; }
    GameObject _mediumDetail { get => _handler.SceneData.medium_detailInScene; set => _handler.SceneData.medium_detailInScene = value; }
    GameObject _highDetail { get => _handler.SceneData.hightDetailInScene; set => _handler.SceneData.hightDetailInScene = value; }
    void OnEnable()
    {
        _handler = (LocalSceneHandler)target;
        sceneName = SceneManager.GetActiveScene().name;

    }
    public override void OnInspectorGUI()
    {
        //Checkea si hay un scriptableobject en el espacio
        if (!_handler.SceneData)
        {
            if (GUILayout.Button("Create Scriptable Object!"))
            {
                //TODO Crear scriptable object donde deberia
                Debug.Log("jsdfjsfjd");
            }
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

            _landmark = EditorGUILayout.ObjectField("Landmark:", _landmark, typeof(GameObject), false) as GameObject;
            _gameplay = EditorGUILayout.ObjectField("Lowdetail:", _gameplay, typeof(GameObject), false) as GameObject;
            _lowdetail = EditorGUILayout.ObjectField("Lowdetail:", _lowdetail, typeof(GameObject), false) as GameObject;
            _mediumDetail = EditorGUILayout.ObjectField("MediumDetail:", _mediumDetail, typeof(GameObject), false) as GameObject;
            _highDetail = EditorGUILayout.ObjectField("HighDetail:", _highDetail, typeof(GameObject), false) as GameObject;

            EditorGUI.EndDisabledGroup();

            //Debug. Te limpia las referencias para que despues puedas cargarlo de nuevo

            //Boton de carga
            if (GUILayout.Button("Load Scene"))
            {
                //Landmark
                _landmark = Instantiate(_handler.SceneData.landmark);
                _landmark.name = _handler.SceneData.landmark.name;

                //Gameplay
                _gameplay = Instantiate(_handler.SceneData.gameplay);
                _gameplay.name = _handler.SceneData.gameplay.name;

                //Low Detail
                _lowdetail = Instantiate(_handler.SceneData.low_detail);
                _lowdetail.name = _handler.SceneData.low_detail.name;

                //Medium Detail
                _mediumDetail = Instantiate(_handler.SceneData.medium_detail);
                _mediumDetail.name = _handler.SceneData.medium_detail.name;

                //HighDetail
                _highDetail = Instantiate(_handler.SceneData.hight_detail);
                _highDetail.name = _handler.SceneData.hight_detail.name;

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
    */
}
