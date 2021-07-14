using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class FastInventory : UI_Base, IPauseable
{
    public static FastInventory instance;

    public System.Action OnOpenInventory = delegate { };
    public Dictionary<int, ItemInInventory> inventory = new Dictionary<int, ItemInInventory>();

    public RectTransform parent_inventoryObjects;
    public GameObject model;

    [SerializeField] List<Item> allItems = new List<Item>();
    Dictionary<int, int> itemsInInventory = new Dictionary<int, int>();
    List<UI_fastItem> itemUI = new List<UI_fastItem>();

    [SerializeField] ItemDescController controller = null;
    [SerializeField] ItemMesseage messeage = null;

    [SerializeField] Tutorial_Dispatcher inventoryTuto = null;

    private void Awake()
    {
        instance = this;
        for (int i = 0; i < allItems.Count; i++)
        {
            var gameobject = Instantiate(model, parent_inventoryObjects).GetComponentInChildren<UI_fastItem>();
            gameobject.photo.sprite = allItems[i].img;
            gameobject.SetInactive();
            itemsInInventory.Add(allItems[i].id, i);
            itemUI.Add(gameobject);
        }
    }

    bool begintimer;
    float timer;

    public bool Have(ItemInInventory[] col)
    {
        for (int i = 0; i < col.Length; i++)
        {
            int id = col[i].item.id;
            if (!inventory.ContainsKey(id) || inventory[id].cant <= 0) return false;
            var my_cant = inventory[id].cant;
            var requested_cant = col[i].cant;

            if (my_cant < requested_cant)
            {
                return false;
            }
        }
        return true;
    }

    public void Add(Item item, int cant = 1)
    {
        string mess = "";
        if (inventory.Count <= 0) inventoryTuto.DispatchTutorialWithDelay(1);
        if (!inventory.ContainsKey(item.id))
        {
            ItemInInventory newSlot = new ItemInInventory(item, cant);
            inventory.Add(item.id, newSlot);
            itemUI[itemsInInventory[item.id]].SetActive(item.consumible);
            mess = "¡Nuevo! ";
        }
        else
        {
            inventory[item.id].cant += cant;
        }

        if (item.equipable)
        {
            EquipedManager.instance.EquipItem(item, cant);
        }

        itemUI[itemsInInventory[item.id]].SetCant(inventory[item.id].cant);
        if(item.consumible) mess = mess + "Agarraste " + cant + " " + item.name;
        else mess = mess + "Agarraste " + item.name;

        EquipedManager.instance.RefreshBlocks();

        messeage.OpenMesseage(item.img, mess);
    }
    public void Remove(Item item, int cant = 1)
    {
        if (inventory.ContainsKey(item.id))
        {
            inventory[item.id].cant -= cant;
            itemUI[itemsInInventory[item.id]].SetCant(inventory[item.id].cant);
        }
    }

    public void SetUI(Item item, int cant)
    {
        if (inventory.ContainsKey(item.id))
        {
            inventory[item.id].cant = cant;
            itemUI[itemsInInventory[item.id]].SetCant(inventory[item.id].cant);
        }
    }

    public bool Remove(ItemInInventory[] col)
    {
        for (int i = 0; i < col.Length; i++)
        {
            if (!inventory.ContainsKey(col[i].item.id)) return false;

            var cant_to_remove = col[i].cant;
            var my_cant = inventory[col[i].item.id].cant;
            var itm = col[i].item;

            if (my_cant >= cant_to_remove)
            {
                Remove(itm, cant_to_remove);
            }
            else
            {
                return false;
            }
        }
        return true;
    }

    int currentSelection;
    bool pause;
    public bool IsOpen { get; private set; }

    public void RefreshScreen()
    {
        OnOpenInventory();
        Open();
        IsOpen = true;
        currentSelection = 0;
        ChangeItemSelect(0);
        Main.instance.GetChar().getInput.inMenu = true;
    }

    public void CloseScreen()
    {
        anim.Close();
        isActive = false;
        IsOpen = false;
        itemUI[currentSelection].DeselectItem();
        currentSelection = 0;
        begintimer = false;
        timer = 0;
        Main.instance.GetChar().getInput.inMenu = false;
    }

    void ChangeItemSelect(int newItemSelect)
    {
        begintimer = true;
        itemUI[currentSelection].DeselectItem();
        currentSelection = newItemSelect;
        itemUI[currentSelection].SelectItem();

        if (inventory.ContainsKey(allItems[currentSelection].id))
        {
            controller.SetItem(inventory[allItems[currentSelection].id]);
        }
        else
        {
            controller.SetUnknownItem(allItems[currentSelection].img);
        }
    }

    protected override void OnAwake() { }
    protected override void OnStart()
    {
        PauseManager.Instance.AddToPause(this);
    }
    protected override void OnEndOpenAnimation() { }
    protected override void OnEndCloseAnimation() { }
    protected override void OnUpdate() 
    {
        if (pause) return;

        if (begintimer)
        {
            timer += Time.deltaTime;
            if (timer > 0.3f)
            {
                begintimer = false;
                timer = 0;
            }
            return;
        }

        if (IsOpen)
        {
            float vertical = Input.GetAxis("Vertical");
            float horizontal = Input.GetAxis("Horizontal");
            int dir = 0;
            if (horizontal > 0.5f || horizontal< -0.5f)
            {
                dir = horizontal > 0 ? 1 : -1;
            }
            else if (vertical > 0.5f || vertical < -0.5f)
            {
                dir = vertical > 0 ? -4 : 4;
            }

            if (dir != 0 && dir + currentSelection >= 0 && dir + currentSelection < itemUI.Count)
                ChangeItemSelect(currentSelection + dir);

            if (Input.GetButtonDown("OpenInventory") && canPress) StartCoroutine(WaitCoroutine());
            else if (Input.GetButtonDown("Back") && canPress) StartCoroutine(WaitCoroutine());

            if (!canPress) canPress = true;
        }

    }

    IEnumerator WaitCoroutine()
    {
        yield return new WaitForEndOfFrame();
        Main.instance.GetChar().getInput.Back.Invoke();

    }

    bool canPress;
    public override void Refresh() { }

    public void Pause()
    {
        pause = true;
    }

    public void Resume()
    {
        pause = false;
    }
}
