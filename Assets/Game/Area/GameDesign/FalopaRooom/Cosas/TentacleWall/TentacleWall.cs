using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TentacleWall : EnemyBase
{
    [SerializeField] Animator _anim;
    [SerializeField] AnimEvent animEvent;
    [SerializeField] BoxCollider boxCol;
    [SerializeField] GameObject damageTrigger;
    [SerializeField] float rotSpeed;

    CDModule cdModule;
    bool attacking;
    bool inRange = false;

    [SerializeField] int damage;
    [SerializeField] float distanceToAttack;

    Transform characterT;

    private void Start()
    {
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


    private void Update()
    {
        //Debug.Log(canupdate);
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
        if (!inRange) return;

        var auxDir = (characterT.position - transform.position).normalized;
       
        transform.forward += Vector3.Lerp(transform.forward, auxDir, Time.fixedDeltaTime * rotSpeed);
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
