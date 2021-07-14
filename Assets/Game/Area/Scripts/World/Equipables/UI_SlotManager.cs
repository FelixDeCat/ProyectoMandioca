using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_SlotManager : MonoBehaviour
{
    public static UI_SlotManager instance;
    private void Awake() => instance = this;

    public UI_CurrentItem[] ui_items;

    Dictionary<SpotType, UI_CurrentItem> reg = new Dictionary<SpotType, UI_CurrentItem>();

    bool activesOn;
    bool itemsOn;

    [SerializeField] GameObject[] activesThing = new GameObject[0];
    [SerializeField] GameObject[] itemThings = new GameObject[0];

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

        for (int i = 0; i < activesThing.Length; i++)
            activesThing[i].SetActive(false);
        for (int i = 0; i < itemThings.Length; i++)
            itemThings[i].SetActive(false);
    }

    public UI_CurrentItem GetSlotBySpot(SpotType spot) => reg.ContainsKey(spot) ? reg[spot] : null;

    public void SetSpotBlock(SpotType spot, bool b)
    {
        if (reg.ContainsKey(spot))
        {
            reg[spot].SetBlock(b);
        }
    }

    public void TryOnActiveSlot()
    {
        if (activesOn) return;
        activesOn = true;
        for (int i = 0; i < activesThing.Length; i++)
            activesThing[i].SetActive(true);
    }

    public void TryOnItemSlot()
    {
        if (itemsOn) return;
        itemsOn = true;
        for (int i = 0; i < itemThings.Length; i++)
            itemThings[i].SetActive(true);
    }
}
