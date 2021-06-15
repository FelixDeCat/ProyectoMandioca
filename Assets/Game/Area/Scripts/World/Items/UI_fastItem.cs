using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_fastItem : MonoBehaviour
{
    public Image photo;
    public TextMeshProUGUI txt;
    [SerializeField] Animator anim = null;
    [SerializeField] GameObject selectCrown = null;

    public void SetInactive()
    {
        photo.color = Color.black;
        txt.gameObject.SetActive(false);
    }

    public void SetActive(bool isConsumible = true)
    {
        photo.color = Color.white;
        txt.gameObject.SetActive(isConsumible);
    }

    public void SetCant(int cant)
    {
        txt.text = cant.ToString();
    }

    public void SelectItem()
    {
        anim.Play("ItemEntry");
        selectCrown.SetActive(true);

    }

    public void DeselectItem()
    {
        anim.Play("ItemExit");
        selectCrown.SetActive(false);
    }
}
