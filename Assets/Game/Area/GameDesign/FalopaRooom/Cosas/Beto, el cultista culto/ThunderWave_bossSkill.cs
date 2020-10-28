using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GOAP;
using Tools.Extensions;

public class ThunderWave_bossSkill : GOAP_Skills_Base
{
    [SerializeField] TotemFeedback totemFeedback = new TotemFeedback();

    Ente _ent;
    DamageData dmgData;
    Animator _anim;
    [SerializeField] int damage = 15;
    [SerializeField] int knock = 200;
    [SerializeField] float radious = 5f;
    [SerializeField] float chargeTime = 5f;
    [SerializeField] float timeBtwEndPartAndExplode = 2f;
    [SerializeField] LayerMask hitMask;

    protected override void OnEndSkill()
    {
        //_ent.OnSkillAction -= ThuderWave;
        //No me gusta que este aca esto. Esto tiene que ser automatico de otro lado
        WorldState.instance.valoresBool["OwnerGetDamage"] = false;
        _ent.canBeInterrupted = true;
        _anim.SetTrigger("finishSkill");
    }

    protected override void OnExecute()
    {
        _ent.canBeInterrupted = false;
        _anim.Play("StartCastOrb");
        totemFeedback.StartChargeFeedback(() => StartCoroutine(TimeBtwEndPartAndExplode()));
        //_ent.OnSkillAction += ThuderWave;
    }

    protected override void OnFixedUpdate()
    {

    }

    IEnumerator TimeBtwEndPartAndExplode()
    {
        yield return new WaitForSeconds(timeBtwEndPartAndExplode);

        ThunderWave();
    }

    void ThunderWave()
    {
        var affectedPO  = Extensions.FindInRadius<PlayObject>(owner, radious);
        hitMask = ~hitMask;


        for (int i = 0; i < affectedPO.Count; i++)
        {
            DamageReceiver obj = affectedPO[i].GetComponent<DamageReceiver>();

            if (obj == null) continue;

            obj.TakeDamage(dmgData);


            //raycast que no quiere funcar
            //Vector3 objDir = ((obj.transform.position + Vector3.up) - (_ent.Root().position + Vector3.up)).normalized;

            //RaycastHit hit;
            //// Does the ray intersect any objects excluding the player layer
            //if (Physics.Raycast(_ent.Root().position + Vector3.up, objDir, out hit, radious, hitMask))
            //{
            //    if(hit.transform.GetComponent<DamageReceiver>() != null)
            //    {
            //        obj.TakeDamage(dmgData);
            //        Debug.Log("Did Hit");
            //    }
            //    else
            //    {
            //        Debug.Log("Entro en true pero no pego");
            //    }

                
            //}
            //else
            //{
            //    //Debug.Log("el hit ESDSS " + hit.transform.name);
            //    Debug.Log("Did not Hit");
            //}

        }

        EndSkill();
    }

    protected override void OnInitialize()
    {
        _ent = owner.GetComponent<Ente>();
        _anim = owner.GetComponentInChildren<Animator>();
        dmgData = GetComponent<DamageData>().SetDamage(damage).SetDamageInfo(DamageInfo.NonParry).SetKnockback(knock);
        totemFeedback.Initialize(StartCoroutine);
    }

    protected override void OnPause()
    {

    }

    protected override void OnResume()
    {

    }

    protected override void OnTurnOff()
    {

    }

    protected override void OnTurnOn()
    {

    }

    protected override void OnUpdate()
    {

    }

    protected override void OnInterruptSkill()
    {
        
    }

    void OnDrawGizmosSelected()
    {
        // Display the explosion radius when selected
        Gizmos.color = new Color(1, 1, 0, 0.75F);
        Gizmos.DrawSphere(transform.position, radious);

        if(_ent)
            Gizmos.DrawLine(_ent.Root().position + Vector3.up, Main.instance.GetChar().Root.position + Vector3.up);
    }
}
