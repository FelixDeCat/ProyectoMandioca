using DevelopTools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Debug_Interactables : SingleObjectPool<GenericLabel>
{
    public InteractSensor sensor;
    public RectTransform parentRect;
    public GenericLabel label;
    public TextMeshProUGUI text;

    private void Update()
    {
        var interacts = sensor.Interacts;
        if (interacts == null) return;
        text.text = "";

        for (int i = 0; i < interacts.Count; i++)
        {
            if (interacts[i] == null) return;

            if (interacts[i].Equals(sensor.Most_Close))
            {
                text.text += "<color=\"red\">" + interacts[i].gameObject.name + "</color> \n";
            }
            else
            {
                text.text += "<color=\"white\">" + interacts[i].gameObject.name + "</color> \n";
            }
        }
    }

    //public ScreamItemWorld GetScream()
    //{
    //    var obj = Get();
    //    obj.to_collect.AddListener(Main.instance.GetChar().CollectScream);
    //    if (!obj.myPool)
    //        obj.myPool = this;
    //    return obj;
    //}

    //public void ReturnScream(ScreamItemWorld item)
    //{
    //    item.to_collect.RemoveListener(Main.instance.GetChar().CollectScream);
    //    ReturnToPool(item);
    //}

    //public void StartPool(int iniAmmount)
    //{
    //    // AddObject(iniAmmount);
    //}
}
