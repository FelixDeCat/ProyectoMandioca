using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DebugText : MonoBehaviour
{
    public TextMeshPro txttmp;
    public TextMesh txttm;
    public Text txtUI;

    public void Print(string s)
    {
                //Debug.Log(s);
        if (txttmp != null) txttmp.text = s;
        if (txttm != null) txttm.text = s;
        if(txtUI != null) txtUI.text = s;
    }
}
