using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DevelopTools;

public class UI_CraftBar : MonoBehaviour
{
    [SerializeField] ItemRequirements requirements = null;
    [SerializeField] ItemReceiverWithItem item_to_Receive = null;

    ItemInInventory[] catch_requirements;
    Item item;
    int cant;


    public RectTransform parent;
    public GameObject model_ui_arrow;
    public UI_fastItem model_ui_item;

    List<GameObject> objs = new List<GameObject>();
    GameObject arrow;

    private void Start()
    {
        catch_requirements = requirements.Items_Require;
        item = item_to_Receive.Item;
        cant = item_to_Receive.Cant;
    }

    public void RefreshCraftBar()
    {
        //limpieza
        for (int i = 0; i < objs.Count; i++)
        {
            Destroy(objs[i]);
        }
        objs.Clear();

        //requisitos
        for (int i = 0; i < catch_requirements.Length; i++)
        {
            UI_fastItem ui = Instantiate(model_ui_item,parent);
            ui.photo.sprite = catch_requirements[i].item.img;
            ui.txt.text = catch_requirements[i].cant.ToString();
            objs.Add(ui.gameObject);
        }

        //flechita
        objs.Add(Instantiate(model_ui_arrow,parent));

        //item a craftear
        var aux = Instantiate(model_ui_item, parent);
        aux.photo.sprite = item.img;
        aux.txt.text = cant.ToString();
        objs.Add(aux.gameObject);
    }
}
