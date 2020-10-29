using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEditor;

[ExecuteInEditMode]
public class FastRename : MonoBehaviour
{
    public bool Execute = true;

    public GameObject LD;
    public GameObject MD;
    public GameObject HD;
    public GameObject GamePlay;
    public GameObject Landmark;

    public string NameScene;

    private void Update()
    {
        if (Execute)
        {
            NameScene = EditorSceneManager.GetActiveScene().name;
            GamePlay.gameObject.name = NameScene + "_0_Gameplay";
            LD.gameObject.name = NameScene + "_1_LD";
            MD.gameObject.name = NameScene + "_2_MD";
            HD.gameObject.name = NameScene + "_3_HD";
            Landmark.gameObject.name = NameScene + "_LandMark";

            var proxys = GetComponentsInChildren<EnemyProxyManager>();
            for (int i = 0; i < proxys.Length; i++)
            {
                proxys[i].sceneName = NameScene;
            }

            PrefabUtility.UnpackPrefabInstance(this.gameObject, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);

            GamePlay.transform.parent = null;
            LD.transform.parent = null;
            MD.transform.parent = null;
            HD.transform.parent = null;
            Landmark.transform.parent = null;

            Landmark.transform.SetAsLastSibling();
            GamePlay.transform.SetAsLastSibling();
            LD.transform.SetAsLastSibling();
            MD.transform.SetAsLastSibling();
            HD.transform.SetAsLastSibling();

            Execute = false;

            FindObjectOfType<LocalSceneHandler>().gameObject.name = this.gameObject.scene.name;
            EditorSceneManager.MarkSceneDirty(this.gameObject.scene);

            DestroyImmediate(this.gameObject);
            EditorSceneManager.SaveOpenScenes();
        }
    }
}
