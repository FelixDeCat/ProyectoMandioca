using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Gold : MonoBehaviour
{
    [SerializeField] Text txt_gold_cant;

    public void SetGold(int cant)
    {
        txt_gold_cant.text = cant.ToString();
    }
}
