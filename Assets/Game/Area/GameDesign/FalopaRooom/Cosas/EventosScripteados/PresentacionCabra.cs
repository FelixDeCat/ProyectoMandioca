using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresentacionCabra : MonoBehaviour
{
    [SerializeField] Transform root;
    [SerializeField] DamageReceiver dmgreceiver;
    [SerializeField] DamageData cabraDmg;

    public float speed;
    public Transform destPos;

    bool active = true;


    private void Start()
    {
        cabraDmg.GetComponent<DamageData>();

        dmgreceiver.Initialize(dmgreceiver.transform, dmgreceiver.GetComponent<Rigidbody>(), dmgreceiver.GetComponent<_Base_Life_System>());

        dmgreceiver.AddTakeDamage((cabraDmg) => dmgreceiver.GetComponent<NPC_Anims>().PlayDeath(""));

        dmgreceiver.GetComponent<NPC_Anims>().StartRunDesesperate("");
    }

    private void Update()
    {
        if (!active) return;

        var pos = (destPos.position - dmgreceiver.transform.position).normalized;

        root.transform.position += pos * speed * Time.deltaTime;
    }
}
