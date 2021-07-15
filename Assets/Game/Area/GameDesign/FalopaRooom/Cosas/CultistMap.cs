using System.Collections;
using System.Collections.Generic;
using Tools.EventClasses;
using UnityEngine;
using UnityEngine.UI;

public class CultistMap : MonoBehaviour
{
    [SerializeField] Sprite mapImage = null;

    public EventCounterPredicate predicate;

    bool isOpen = false;

    public void OpenMap()
    {
        Debug.Log("lelelele");
        Main.instance.gameUiController.OpenCustomImage(mapImage);
    }

    public void CloseMap()
    {
        Main.instance.gameUiController.CloseCustomImage();
    }

    public void ToggleMap()
    {
        if (isOpen)
        {
            Main.instance.gameUiController.CloseCustomImage();
            isOpen = false;
        }
        else
        {
            if(Main.instance.gameUiController.OpenCustomImage(mapImage))
                isOpen = true;
        }
    }

    public bool Pred()
    {

        return true;
    }
}
