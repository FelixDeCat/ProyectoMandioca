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
    [SerializeField] Text mision_name;
    [SerializeField] Text description;
    [SerializeField] Text subdescription;

    [SerializeField] Text mision_state;//finalizado o en curso

    [SerializeField] Text items;// esto luego hacerle un sub panel con las cosas que hay que hacer
    [SerializeField] Text regions_to_enable;
    [SerializeField] Text recompensa; // esto luego hacerle un sub panel con los objetos, su imagen y descripcion

    public void SetInfo(Mision mision)
    {
        mision_name.text = mision.info.mision_name;
        description.text = mision.info.description;
        subdescription.text = mision.info.subdescription;

        mision_state.text = mision.data.Completed ? "Finalizado" : "En curso";
        mision_state.color = mision.data.Completed ? Color.black : Color.grey;

        items.text = "Subtareas\n";

        foreach (var item in mision.data.MisionItems)
        {
            try
            {
                var auxbool = (BoolItemMision)item;
                if (auxbool != null)
                {
                    string desc = auxbool.Description;
                    items.text += "<color=" + (auxbool.IsCompleted ? "green" : "black") + "> " + desc + "</color> \n";
                }
            }
            catch (System.InvalidCastException ex) {  }

            try
            {
                var auxint = (IntItemMision)item;
                if (auxint != null)
                {
                    string desc = auxint.CurrentValue + " / " + auxint.MaxValue + " " + auxint.Description;
                    items.text += "<color=" + (auxint.IsCompleted ? "green" : "black") + "> " + desc + "</color> \n";
                }
            }
            catch (System.InvalidCastException ex) { }
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
