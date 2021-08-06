using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EntusiastaVillager : MonoBehaviour
{
    [SerializeField] Transform[] waypoints = new Transform[0];
    [SerializeField] Animator anim = null;
    [SerializeField] AssassinRay ray = null;
    [SerializeField] UnityEvent[] reachToWaypoint = new UnityEvent[0];
    [SerializeField] UnityEvent endThunderAnim = null;

    [SerializeField] float speed = 5;

    int currentWaypoint;
    int targetWaypoint;
    bool move;

    private void Start()
    {
        anim.GetComponent<AnimEvent>().Add_Callback("FinishAbility", () => { endThunderAnim.Invoke(); Debug.Log("la puta madre"); } );
        anim.GetComponent<AnimEvent>().Add_Callback("SpawnOrb", ray.Attack);
    }

    public void GoToWaypoint(int waypoint)
    {
        targetWaypoint = waypoint;
        move = true;
        anim.SetBool("Move", true);
    }

    public void UpdateInteractable() => Main.instance.eventManager.TriggerEvent(GameEvents.INTERACTABLES_INITIALIZE);

    private void Update()
    {
        if (move)
        {
            Vector3 dirToWaypoint = (waypoints[currentWaypoint + 1].position - transform.position).normalized;
            transform.forward = dirToWaypoint;

            transform.position += transform.forward * speed * Time.deltaTime;

            if (Vector3.Distance(transform.position, waypoints[currentWaypoint + 1].position) <= 0.2f)
            {
                currentWaypoint += 1;
                if(currentWaypoint == targetWaypoint)
                {
                    move = false;
                    anim.SetBool("Move", false);
                    reachToWaypoint[currentWaypoint].Invoke();
                    transform.forward = waypoints[currentWaypoint].forward;
                }
            }
        }
    }
}
