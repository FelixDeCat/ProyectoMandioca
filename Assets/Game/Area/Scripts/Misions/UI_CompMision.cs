using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_CompMision : MonoBehaviour
{
    public Text Title;
    public Text Description;
    public Text Progesion;

    public void SetData(string title, string description, string progresion)
    {
        Title.text = title;
        Description.text = description;
        Progesion.text = progresion;
    }
}
