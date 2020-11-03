using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools.Extensions;
using System;
using System.Linq;

public class InteractSensor : MonoBehaviour
{
    public bool isclose;

    List<Interactable> interactables = new List<Interactable>();
    public List<Interactable> Interacts { get { return interactables; } }
    Interactable current;

    Interactable most_close;
    public Interactable Most_Close { get { return most_close; } }
    [Header("Walking Entity")]
    [SerializeField] WalkingEntity collector = null;

    bool buttonPressing;

    private void Start()
    {
        Main.instance.eventManager.SubscribeToEvent(GameEvents.INTERACTABLES_INITIALIZE, AddInteractStart);
        Main.instance.eventManager.SubscribeToEvent(GameEvents.DELETE_INTERACTABLE, RemoveToInteractables);
        Main.instance.eventManager.SubscribeToEvent(GameEvents.ADD_INTERACTABLE, AddOneInteract);
    }

    void AddInteractStart()
    {
        interactables = FindObjectsOfType<Interactable>().ToList();
    }

    void AddOneInteract(params object[] interact)
    {
        Interactable _interact = (Interactable)interact[0];
        if (!interactables.Contains(_interact)) interactables.Add(_interact);
    }

    public void RemoveToInteractables(params object[] interact)
    {
        Interactable _interact = (Interactable)interact[0];
        if (interactables.Contains(_interact)) interactables.Remove(_interact);
    }

    public Interactable OnInteractDown()
    {
        if (!can_interact) return null;

        calculate_fast_recollection = true;
        timerfastrec = 0;
        buttonPressing = true;
        if (isclose && most_close != null)
        {
            most_close.Execute(collector);
            return most_close;
        }

        return null;
    }
    public void OnInteractUp()
    {
        if (most_close != null)
        {
            most_close.InterruptExecute();
        }
        buttonPressing = false;
        calculate_fast_recollection = false;
        timerfastrec = 0;
    }

    public void Dissappear()
    {
        most_close.Exit();
        if (interactables.Contains(most_close)) interactables.Remove(most_close);
        most_close = null;
        most_close = interactables.ReturnMostClose(transform.position);
        if (most_close != null && I_Have_Good_Distace_To_Interact()) return;
        ContextualBarSimple.instance.Hide();
        WorldItemInfo.instance.Hide();
    }

    bool can_show_info = true;
    bool can_interact = true;

    public void CanInteract(bool val) => can_interact = val;


    private void Update()
    {
        if (!can_interact) return;
        isclose = false;
        Update_CheckFastRecollection();
        Update_CooldownNextRecollection();

        //buscamos los interactuables
        //interactables = FindObjectsOfType<Interactable>().ToList();//hay que optimizar esto, es muy pesado un findobject en un Update
        List<Interactable> filtered = new List<Interactable>();

        for (int i = 0; i < interactables.Count; i++)
        {
            if (interactables[i].autoexecute && Vector3.Distance(interactables[i].transform.position, transform.position) < interactables[i].distancetoInteract)
            {
                interactables[i].Execute(collector);
                can_show_info = true;
            }
            else
            {
                filtered.Add(interactables[i]);
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


        if (I_Have_Good_Distace_To_Interact())
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
    [SerializeField] bool can_fast_recollecion = false;
    [SerializeField] float timerfastrec = 0;
    void Update_CheckFastRecollection()
    {
        if (calculate_fast_recollection)
        {
            if (timerfastrec < time_to_fast_recollection)
            {
                timerfastrec = timerfastrec + 1 * Time.deltaTime;
            }
            else
            {
                can_fast_recollecion = true;
            }
        }
        else
        {
            can_fast_recollecion = false;
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

    bool I_Have_Good_Distace_To_Interact() { return Vector3.Distance(most_close.transform.position, transform.position) < most_close.distancetoInteract; }


}
