using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_EquipableBase : MonoBehaviour
{
    public Image img;
    public Item itemRef;
    public SpotType spotType;

    public void Refresh(Item itmRef)
    {
        itemRef = itmRef;
        img.sprite = itemRef.img;
    }
}
