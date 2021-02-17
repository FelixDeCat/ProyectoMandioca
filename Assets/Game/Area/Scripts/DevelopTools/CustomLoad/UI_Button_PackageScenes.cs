using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

public class UI_Button_PackageScenes : MonoBehaviour
{
    Action<HashSet<string>> callbackOnPress = delegate { };
    string[] scenes;

    [SerializeField] TextMeshProUGUI txt_name = null;
    Button myButton;

    public void Configure(string _name, Action<HashSet<string>> _callbackOnPress,  params string[] _scenes)
    {
        myButton = GetComponent<Button>();
        myButton.onClick.AddListener(OnPress);
        txt_name.text = _name;
        callbackOnPress = _callbackOnPress;
        scenes = _scenes;
    }

    void OnPress() => callbackOnPress.Invoke(new HashSet<string>(scenes));
}
