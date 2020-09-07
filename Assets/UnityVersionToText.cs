using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnityVersionToText : MonoBehaviour
{
    TextMeshProUGUI txtmesh;
    Text textUI;

    void Start()
    {
        txtmesh = GetComponent<TextMeshProUGUI>();
        textUI = GetComponent<Text>();

        if (txtmesh != null) txtmesh.text = "Version " + Application.version;
        if (textUI != null) textUI.text = "Version " + Application.version;
    }

}
