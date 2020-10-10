using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using System.Linq;

[ExecuteInEditMode]
public class LoadScenesInEditor : MonoBehaviour
{
    public bool BUTTON_LOAD;
    public bool BUTTON_UNLOAD;
    public string scenes;
    string[] catched;
    HashSet<Scene> hashscenes = new HashSet<Scene>();
    

    private void Update()
    {
        if (BUTTON_LOAD)
        {
            HashSet<string> strings = new HashSet<string>(hashscenes.Select(x => x.name));

            catched = scenes.Split(',');

            BUTTON_LOAD = false;

            for (int i = 0; i < catched.Length; i++)
            {
                if (!strings.Contains(catched[i]))
                {
                    string path = "Assets/Game/#Scenes/LoadByScene/Prefabs/" + catched[i] + "/" + catched[i] + ".unity";
                    hashscenes.Add(EditorSceneManager.OpenScene(path, OpenSceneMode.Additive));
                }
            }
        }
        if (BUTTON_UNLOAD)
        {
            BUTTON_UNLOAD = false;

            foreach (var s in hashscenes)
            {
                EditorSceneManager.CloseScene(s,true);
            }
            hashscenes.Clear();
        }
    }
}
