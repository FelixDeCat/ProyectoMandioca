using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_MisionManager : MonoBehaviour
{
    public List<UI_CompMision> ui_misions;

    public Transform parent;
    public GameObject model;

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
            GameObject go = GameObject.Instantiate(model);
            go.transform.SetParent(parent);
            go.transform.localScale = new Vector3(1, 1, 1);
            go.transform.localPosition = new Vector3(0, 0, 0);

            var m = go.GetComponent<UI_CompMision>();
            m.SetData(misions[i].mision_name,
                misions[i].description,
                misions[i].subdescription);

            ui_misions.Add(m);
        }
    }
}
