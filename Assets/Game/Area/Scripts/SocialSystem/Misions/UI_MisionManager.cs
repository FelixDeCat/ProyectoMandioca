using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_MisionManager : MonoBehaviour
{
    public List<UI_CompMision> ui_misions;
    public RectTransform parent;
    public UI_CompMision model;
    bool mostrar;

    public void Awake()
    {
        mostrar = true;
    }

    public void MostrarMisiones()
    {
        if (mostrar)
        {
            parent.localScale = new Vector3(1, 0, 1);
            mostrar = false;
        }
        else
        {
            parent.localScale = new Vector3(1, 1, 1);
            mostrar = true;
        }
    }

    public void RefreshUIMisions(List<Mision> misions)
    {
        for (int i = 0; i < ui_misions.Count; i++)
        {
            Destroy(ui_misions[i].gameObject);
        }
        ui_misions.Clear();

        for (int i = 0; i < misions.Count; i++)
        {
            UI_CompMision m = GameObject.Instantiate(model, parent);
            m.SetData(
                misions[i].info.mision_name,
                misions[i].data.ItemsCompleteString());

            ui_misions.Add(m);
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(parent);


    }
}
