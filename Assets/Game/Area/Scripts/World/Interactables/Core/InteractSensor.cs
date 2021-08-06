using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools.Extensions;
using System;
using System.Linq;

public class InteractSensor : MonoBehaviour
{
    #region static Instance
    public static InteractSensor instance;
    private void Awake() => instance = this;
    #endregion

    public HashSet<Interactable> interactables = new HashSet<Interactable>();
    Interactable current;

    Interactable most_close;
    public Interactable Most_Close { get { return most_close; } }
    [Header("Walking Entity")]
    [SerializeField] WalkingEntity collector = null;

    bool buttonPressing;
    bool isclose;

    private void Start()
    {
        Main.instance.eventManager.SubscribeToEvent(GameEvents.INTERACTABLES_INITIALIZE, AddInteractStart);
    }

    void AddInteractStart()
    {
        interactables = FindObjectsOfType<Interactable>().ToHashSet();
    }

    #region [EXPOSED] public Statics
    public static void Add_Interactable     (Interactable interactable)     => instance.AddInter(interactable);
    public static void Remove_Interactable  (Interactable interactable)     => instance.RemoveInter(interactable);
    public static Vector3 TargetPosition                                    => instance.transform.position;
    #endregion

    #region [LOGIC] Add & Remove
    void AddInter(Interactable interactable) => interactables.Add(interactable);
    void RemoveInter(Interactable interactable) => interactables.Remove(interactable);
    #endregion

    #region INPUT > DOWN & UP
    public Interactable OnInteractDown()
    {
        if (!can_interact) return null;

        calculate_fast_recollection = true;
        timerfastrec = 0;
        buttonPressing = true;
        if (isclose && most_close != null)
        {
            most_close.Execute(collector);
            most_close.BUTTON_PressDown();
            return most_close;
        }

        return null;
    }
    public void OnInteractUp()
    {
        if (most_close != null)
        {
            most_close.InterruptExecute();
            most_close.BUTTON_PressUp();
        }
        buttonPressing = false;
        calculate_fast_recollection = false;
        timerfastrec = 0;
    }
    #endregion

    public void Dissappear(Interactable interact)
    {
        //most_close.Exit();
        most_close = null;

        RemoveInter(interact);

        most_close = interactables.ReturnMostClose(transform.position, x => x.CanInteract);

        if (most_close != null)
        {
            if (most_close.InDistance())
            {
                if (!most_close.autoexecute)
                {
                    ContextualBarSimple.instance.Show();
                    ContextualBarSimple.instance.Set_Sprite_Button_Custom(InputImageDatabase.InputImageType.interact);
                }
                else ContextualBarSimple.instance.Hide();
                most_close.Enter(collector);
            }
            else
                ContextualBarSimple.instance.Hide();
        }
        else
        {
            ContextualBarSimple.instance.Hide();
        }
    }

    bool can_show_info = true;
    bool can_interact = true;

    public void CanInteract(bool val) => can_interact = val;


    private void Update()
    {
        if (!can_interact) return; //esto venia de los menues
        isclose = false;
        Update_CheckFastRecollection();
        Update_CooldownNextRecollection();

        //buscamos los interactuables
        //interactables = FindObjectsOfType<Interactable>().ToList();//hay que optimizar esto, es muy pesado un findobject en un Update
        List<Interactable> filtered = new List<Interactable>();
        List<Interactable> interactables_list = new List<Interactable>(interactables);
        for (int i = 0; i < interactables_list.Count; i++)
        {
            if (interactables_list[i].autoexecute && interactables_list[i].InDistance())
            {
                interactables_list[i].Execute(collector);
                can_show_info = true;
            }
            else
            {
                filtered.Add(interactables_list[i]);
            }
        }
        if (filtered.Count == 0) { return; }
        if (filtered.Count == 1) current = filtered[0];
        else current = filtered.ReturnMostClose(transform.position, x => x.CanInteract);//esto tambien se puede optimizar mas a delante con un return most close que busque por grupos

        //si no hay un most_close previo le asigno uno
        if (most_close == null)
        {
            most_close = current;
            return;
        }

        //si lo hay, pero mi current es mas cercano, le asigno current
        if (current != most_close)
        {
            most_close.Exit();
            most_close = current;
            can_show_info = true;
        }

        //hasta ahora ya estariamos al dia de cual es el que tengo que calcular
        //ahora...


        if (most_close.InDistance())
        {

            if(isExit && buttonPressing) most_close.Execute(collector);

            isclose = true;

            isExit = false;

            if (can_show_info)
            {
                most_close.Enter(collector);
                can_show_info = false;
            }

            if (most_close.autoexecute)
            {
                most_close.Execute(collector);
                can_show_info = true;
                WorldItemInfo.instance.Hide();
                return;
            }

            if (!cooldown_to_next_recollection && !most_close._withDelay)
            {
                if (most_close.autoexecute)
                {
                    cooldown_to_next_recollection = true;
                    most_close.Execute(collector);
                    can_show_info = true;
                    WorldItemInfo.instance.Hide();
                }
            }
        }
        else
        {
            if (isExit) return;
            isclose = false;
            most_close.Exit();
            WorldItemInfo.instance.Hide();
            can_show_info = true;
            isExit = true;
            //collector.GetComponent<CharacterHead>()?.UNITY_EVENT_OnInteractUp();
        }
    }

    bool isExit = true;
    [Header("Fast Recollection")]
    [Range(0f, 1f)]
    public float time_to_fast_recollection = 0.5f;
    bool calculate_fast_recollection = false;
    [SerializeField] float timerfastrec = 0;
    void Update_CheckFastRecollection()
    {
        if (calculate_fast_recollection)
        {
            if (timerfastrec < time_to_fast_recollection)
            {
                timerfastrec = timerfastrec + 1 * Time.deltaTime;
            }
        }
        else
        {
            timerfastrec = 0;
        }
    }
    [Header("Cooldown")]
    [Range(0f, 0.5f)]
    public float cooldownrecolect = 0.1f;
    public bool cooldown_to_next_recollection = true;
    float timer_cooldown = 0;
    void Update_CooldownNextRecollection()
    {
        if (cooldown_to_next_recollection)
        {
            if (timer_cooldown < cooldownrecolect)
            {
                timer_cooldown = timer_cooldown + 1 * Time.deltaTime;
            }
            else
            {
                cooldown_to_next_recollection = false;
                timer_cooldown = 0;
            }
        }
    }

}

internal static class InteractExtensions
{
    internal static bool InDistance(this Interactable i) { return Vector3.Distance(i.Position, InteractSensor.TargetPosition) < i.distancetoInteract; }
}
