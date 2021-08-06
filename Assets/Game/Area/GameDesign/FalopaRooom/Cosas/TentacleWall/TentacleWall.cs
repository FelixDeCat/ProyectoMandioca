using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TentacleWall : EnemyBase
{
    [SerializeField] Animator _anim = null;
    [SerializeField] AnimEvent animEvent = null;
    [SerializeField] BoxCollider boxCol = null;
    [SerializeField] GameObject damageTrigger = null;
    [SerializeField] float rotSpeed = 10;

    CDModule cdModule;
    bool attacking;
    bool inRange = false;

    [SerializeField] int damage = 10;

    Transform characterT;
    Vector3 initDir;

    private void Start()
    {
        initDir = transform.forward;
        characterT = Main.instance.GetChar().Root;

        animEvent.Add_Callback("attack", () => damageTrigger.SetActive(true));
        animEvent.Add_Callback("end", () => gameObject.SetActive(false));
        animEvent.Add_Callback("finishAttack", ResetTentacleAttack);

        dmgData = GetComponent<DamageData>().SetDamage(damage).SetDamageInfo(DamageInfo.NonBlockAndParry).SetDamageType(Damagetype.Normal).SetKnockback(100);
        dmgData.Initialize(transform);

        cdModule = new CDModule();
    }

    void ResetTentacleAttack()
    {
        if (!attacking) return;

        attacking = false;
        damageTrigger.SetActive(false);
        boxCol.enabled = true;
    }

    public void CloseTentacles()
    {
        if (!gameObject.activeInHierarchy) return;

        _anim.Play("End");
    }

    public void AttackTentacles()
    {
        _anim.Play("Attack");
        attacking = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        var aux = other.GetComponent<CharacterHead>();

        if(aux != null)
        {
            boxCol.enabled = false;
            AttackTentacles();
        }
    }

    public void DoDamage()
    {
        Main.instance.GetChar().DamageReceiver().TakeDamage(dmgData);
        damageTrigger.SetActive(false);

        cdModule.AddCD("waitToAttackAgain", () => 
        {
            attacking = false;
            boxCol.enabled = true;
            
        }, 2);
        

        //attacking = false;
        //if(Vector3.Distance(transform.position, Main.instance.GetChar().Root.position) < distanceToAttack)
        //{
        //}
    }

    public void IsInRange(bool value) { inRange = value; }

    void LookAtPlayer()
    {
        if (attacking) return;
        if (!inRange)
        {
            if(transform.forward!= initDir)transform.forward += Vector3.Lerp(transform.forward, initDir, Time.fixedDeltaTime * rotSpeed);
            return;
        }

        var auxDir = (characterT.position - transform.position).normalized;
       
        transform.forward += Vector3.Lerp(transform.forward, auxDir, Time.fixedDeltaTime * rotSpeed);
    }

    protected override void OnPause()
    {
        base.OnPause();
        animator.speed = 0;
    }
    protected override void OnResume()
    {
        base.OnResume();
        animator.speed = 1;
    }

    protected override void OnReset() { }
    protected override void TakeDamageFeedback(DamageData data) { }
    protected override void Die(Vector3 dir) { }
    protected override bool IsDamage() { return true; }
    protected override void OnUpdateEntity() { }
    protected override void OnTurnOn() { }
    protected override void OnTurnOff() { }
    protected override void OnFixedUpdate() {  cdModule.UpdateCD(); LookAtPlayer(); }
 

}
