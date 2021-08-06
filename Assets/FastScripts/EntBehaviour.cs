using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EntBehaviour : MonoBehaviour
{
    [SerializeField] DamageReceiver[] myTargets = new DamageReceiver[0];
    [SerializeField] Transform[] waypoints = new Transform[0];
    [SerializeField] Animator anim = null;
    [SerializeField] UnityEvent[] reachToWaypoint = new UnityEvent[0];
    [SerializeField] UnityEvent endThunderAnim = null;

    [SerializeField] float speed = 5;

    int currentWaypoint;
    public int targetWaypoint;
    bool move;

    private void Start()
    {
        anim.GetComponent<AnimEvent>().Add_Callback("FinishPiña", () => { endThunderAnim.Invoke(); Debug.Log("finisheo piña"); });
        anim.GetComponent<AnimEvent>().Add_Callback("DealDamage", Attack);
    }

    public void GoToWaypoint(int waypoint)
    {
        targetWaypoint = waypoint;
        move = true;
        anim.SetBool("Move", true);
    }

    void Attack()
    {
        for (int i = 0; i < myTargets.Length; i++)
            myTargets[i].GetComponent<PropDestructible>().DestroyDestructible();
    }

    public void StopMove() => move = false;

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
                if (currentWaypoint == targetWaypoint)
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
