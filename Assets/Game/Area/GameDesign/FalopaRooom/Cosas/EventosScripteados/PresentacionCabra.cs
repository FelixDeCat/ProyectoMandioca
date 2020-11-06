using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresentacionCabra : MonoBehaviour
{
    [SerializeField] Transform root;
    [SerializeField] DamageReceiver dmgreceiver;
    [SerializeField] DamageData cabraDmg;
    [SerializeField] ChangeToTarget changeToTarget;
    [SerializeField] EventDestructible cercaRota;

    public float speed;
    public Transform destPos;

    bool active;


    private void Start()
    {
        cabraDmg.GetComponent<DamageData>();

        dmgreceiver.Initialize(dmgreceiver.transform, dmgreceiver.GetComponent<Rigidbody>(), dmgreceiver.GetComponent<_Base_Life_System>());

        dmgreceiver.AddTakeDamage(OnTakeDamage);

        dmgreceiver.GetComponent<NPC_Anims>().Play_Idle("");
    }

    void OnTakeDamage(DamageData dmg)
    {
        cercaRota?.BreakYourselfBaby();
        changeToTarget.ChangeTarget();
        dmgreceiver.GetComponent<NPC_Anims>().StopRunDesesperate("");
        dmgreceiver.GetComponent<NPC_Anims>().PlayDeath("");
    }

    public void StartRunning()
    {
        active = true;
        dmgreceiver.GetComponent<NPC_Anims>().StartRunDesesperate("");
    }

    private void Update()
    {
        if (!active) return;

        var pos = (destPos.position - dmgreceiver.transform.position).normalized;

        root.transform.position += pos * speed * Time.deltaTime;
    }
}
