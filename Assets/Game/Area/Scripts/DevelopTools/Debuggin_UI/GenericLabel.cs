using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GenericLabel : MonoBehaviour
{
    Image myImage;
    Text myText;
    TextMeshProUGUI txtmesh;

    private void Start()
    {
        myImage = GetComponent<Image>();
        if (myImage == null) myImage.GetComponentInChildren<Image>();
        myText = GetComponentInChildren<Text>();
        txtmesh = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SetText(string text, Color? color = null)
    {
        myImage.color = color ?? Color.white;
        if(myText) myText.text = text;
        if (txtmesh) txtmesh.text = text;

    }
}
