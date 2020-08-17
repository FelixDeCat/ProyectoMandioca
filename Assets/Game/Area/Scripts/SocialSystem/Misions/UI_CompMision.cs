using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_CompMision : MonoBehaviour
{
    public TextMeshProUGUI Title;
    public TextMeshProUGUI Progesion;

    public void SetData(string title, string progresion)
    {
        Title.text = title;
        Progesion.text = progresion;
    }
}
