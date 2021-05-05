using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemDescController : MonoBehaviour
{
    [SerializeField] Image sprite = null;
    [SerializeField] TextMeshProUGUI title = null;
    [SerializeField] TextMeshProUGUI description = null;
    [SerializeField] TextMeshProUGUI ammount = null;

    public void SetItem(ItemInInventory item)
    {
        sprite.sprite = item.item.img;
        sprite.color = Color.white;
        title.text = item.item.name;
        description.text = item.item.description;
        ammount.gameObject.SetActive(true);
        ammount.text = item.cant.ToString();
    }

    public void SetUnknownItem(Sprite _sprite)
    {
        sprite.sprite = _sprite;
        sprite.color = Color.black;
        title.text = "???";
        description.text = "Aún no se descubrió este objeto.";
        ammount.gameObject.SetActive(false);
    }
}
