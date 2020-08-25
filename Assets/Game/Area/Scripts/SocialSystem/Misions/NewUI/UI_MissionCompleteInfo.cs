using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

///////////////////////////////////////////////////////////////////////////////////////
/*
    este solo va a ser un mostrador bobo
    pero aca se mostrará toda la informacion completa y detallada de la mision
    que acabamos de seleccionar con los botones
*/
///////////////////////////////////////////////////////////////////////////////////////
public class UI_MissionCompleteInfo : MonoBehaviour
{
    [SerializeField] Text mision_name = null;
    [SerializeField] Text description = null;

    [SerializeField] Text mision_state = null;//finalizado o en curso

    [SerializeField] Text items = null;// esto luego hacerle un sub panel con las cosas que hay que hacer
    [SerializeField] Text regions_to_enable = null;
    [SerializeField] Text recompensa = null; // esto luego hacerle un sub panel con los objetos, su imagen y descripcion

    public void ClearInfo()
    {
        mision_name.text = description.text = mision_state.text =
            items.text = regions_to_enable.text = recompensa.text = "";
    }

    public void SetInfo(Mision mision)
    {
        mision_name.text = mision.info.mision_name;
        description.text = mision.info.description;

        mision_state.text = mision.data.Completed ? "Finalizado" : "En curso";
        mision_state.color = mision.data.Completed ? Color.black : Color.grey;

        items.text = "Subtareas\n";

        foreach (var item in mision.data.MisionItems)
        {
            if (item.itemType == ItemMision.ItemType.one_objective_Bool)
            {
                string desc = item.Description;
                items.text += "<color=" + (item.IsCompleted ? "green" : "black") + "> " + desc + "</color> \n";
            }
            if (item.itemType == ItemMision.ItemType.multiple_int)
            {
                string desc = item.CurrentValue + " / " + item.MaxValue + " " + item.Description;
                items.text += "<color=" + (item.IsCompleted ? "green" : "black") + "> " + desc + "</color> \n";
            }
        }

        regions_to_enable.text = "";
        foreach (var place in mision.data.Regions)
        {
            regions_to_enable.text += place + "\n";
        }

        for (int i = 0; i < mision.data.Regions.Length; i++)
        {
            if (i != mision.data.Regions.Length - 1)
                regions_to_enable.text += mision.data.Regions[i] + "\n";
            else
                regions_to_enable.text += mision.data.Regions[i];
        }

        recompensa.text = "";
        for (int i = 0; i < mision.rewarding.items_rewarding.Length; i++)
        {
            if (i != mision.rewarding.items_rewarding.Length - 1)
                recompensa.text += mision.rewarding.items_rewarding[i].cant + " " + mision.rewarding.items_rewarding[i].item.name + "\n";
            else
                recompensa.text += mision.rewarding.items_rewarding[i].cant + " " + mision.rewarding.items_rewarding[i].item.name;
        }
    }
}
