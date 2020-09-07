using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_SlotManager : MonoBehaviour
{
    public static UI_SlotManager instance;
    private void Awake() => instance = this;

    public UI_CurrentItem[] ui_items;

    Dictionary<SpotType, UI_CurrentItem> reg = new Dictionary<SpotType, UI_CurrentItem>();


    private void Start()
    {
        ui_items = GetComponentsInChildren<UI_CurrentItem>();

        for (int i = 0; i < ui_items.Length; i++)
        {
            if (!reg.ContainsKey(ui_items[i].spot_to_represent))
            {
                reg.Add(ui_items[i].spot_to_represent, ui_items[i]);
            }
        }  
    }

    public UI_CurrentItem GetSlotBySpot(SpotType spot) => reg.ContainsKey(spot) ? reg[spot] : null;
}
