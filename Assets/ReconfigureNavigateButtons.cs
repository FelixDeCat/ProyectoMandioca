using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReconfigureNavigateButtons : MonoBehaviour
{
    public Selectable[] selectables;

    public bool clampOnVerticalConfig = true;

    public void Reconfigure()
    {
        selectables = GetComponentsInChildren<Selectable>(false);

        if (clampOnVerticalConfig)
        {
            if (selectables.Length == 1)
            {
                Navigation nav = new Navigation();
                nav.mode = Navigation.Mode.None;
                selectables[0].navigation = nav;
            }
            else
            {
                for (int i = 0; i < selectables.Length; i++)
                {
                    Navigation nav = new Navigation();
                    nav.mode = Navigation.Mode.Explicit;
                    if (i > 0 && i < selectables.Length-1)
                    {
                        nav.selectOnUp = selectables[i - 1];
                        nav.selectOnDown = selectables[i + 1];
                    }
                    else
                    {
                        if (i <= 0)
                        {
                            nav.selectOnUp = selectables[selectables.Length - 1];
                            nav.selectOnDown = selectables[1];
                        }
                        else if (i >= selectables.Length - 1)
                        {
                            nav.selectOnUp = selectables[selectables.Length - 2];
                            nav.selectOnDown = selectables[0];
                        }
                    }
                    selectables[i].navigation = nav;
                }
            }
        }
    }
}
