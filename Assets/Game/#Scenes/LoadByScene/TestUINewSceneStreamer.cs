using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TestUINewSceneStreamer : MonoBehaviour
{
    public NewSceneStreamer streamer;
    public TextMeshProUGUI currentScene;
    public TextMeshProUGUI t_loaded;
    public TextMeshProUGUI t_loading;
    private void Update()
    {
        currentScene.text = streamer.currentScene;
        Print(streamer.loaded, t_loaded);
        Print(streamer.loading, t_loading);
    }
    public void Print(HashSet<string> hash, TextMeshProUGUI txt)
    {
        txt.text = "";
        foreach (var h in hash)
        {
            txt.text += h + "  ";
        }
    }
}
