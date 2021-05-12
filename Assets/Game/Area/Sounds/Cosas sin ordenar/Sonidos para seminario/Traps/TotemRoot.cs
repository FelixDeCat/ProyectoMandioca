using System.Collections;
using UnityEngine;

public class TotemRoot : Totem
{
    [SerializeField] private float range = 20;
    [SerializeField] bool squaredRange = false;
    [SerializeField] Vector3 squareRange = Vector3.zero;

    [SerializeField] private float effectDuration = 10;

    [SerializeField] ParticleSystem onRootParticles = null;
    [SerializeField] CombatArea spawneablePosition = null;
    [SerializeField] EffectName effectOwner = EffectName.OnRoot;

    [SerializeField] AnimEvent animEvent = null;
    [SerializeField] Collider col = null;

    CharacterHead myChar;

    protected override void OnInitialize()
    {
        base.OnInitialize();

        ParticlesManager.Instance.GetParticlePool(onRootParticles.name, onRootParticles);

        animEvent.Add_Callback("TeleportAnim", TeleportForAnim);
    }

    protected override void OnTurnOn()
    {
        base.OnTurnOn();
        myChar = Main.instance.GetChar();
        StartCoroutine(CheckDistance());
        dmgReceiver.AddInvulnerability(Damagetype.All);
        animator.enabled = true;
    }

    protected override void OnTurnOff()
    {
        base.OnTurnOff();
        animator.ResetTrigger("Teleport");
        animator.Play("idle");
        animator.enabled = false;
        col.enabled = true;
    }

    IEnumerator CheckDistance()
    {
        while (true)
        {
            Vector3 aux = transform.InverseTransformPoint(myChar.transform.position);

            if (!onUpdate && (!squaredRange && Vector3.Distance(myChar.transform.position, transform.position) <= range) ||
                squaredRange && (Mathf.Abs(aux.x) <= squareRange.x && Mathf.Abs(aux.y) <= squareRange.y && Mathf.Abs(aux.z) <= squareRange.z))
                OnTotemEnter();
            else if(onUpdate && (!squaredRange && Vector3.Distance(myChar.transform.position, transform.position) > range) ||
            squaredRange && (Mathf.Abs(aux.x) > squareRange.x || Mathf.Abs(aux.y) > squareRange.y || Mathf.Abs(aux.z) > squareRange.z))
                OnTotemExit();
            yield return new WaitForSeconds(0.5f);
        }
    }

    protected override void UpdateTotem()
    {
        base.UpdateTotem();
    }

    void StunChar()
    {
        myChar.GetComponent<EffectReceiver>().TakeEffect(effectOwner, effectDuration);

        ParticlesManager.Instance.PlayParticle(onRootParticles.name, myChar.transform.position);
    }

    protected override void InternalStunOver()
    {
        base.InternalStunOver();
        dmgReceiver.AddInvulnerability(Damagetype.All);
        Teleport();
    }

    void Teleport()
    {
        col.enabled = false;
        animator.SetTrigger("Teleport");
    }

    void TeleportForAnim()
    {
        Vector2 aux;
        if (spawneablePosition.isCircle)
        {
            float ang = Random.Range(0, 360);

            Vector3 pos;
            pos.x = spawneablePosition.transform.position.x + spawneablePosition.circleRadius * Random.value * Mathf.Sin(ang * Mathf.Deg2Rad);
            pos.y = spawneablePosition.transform.position.y;
            pos.z = spawneablePosition.transform.position.z + spawneablePosition.circleRadius * Random.value * Mathf.Cos(ang * Mathf.Deg2Rad);

            transform.position = pos;
        }
        else
        {
            aux.x = -(spawneablePosition.cubeArea.x / 2) + spawneablePosition.cubeArea.x * Random.value;
            aux.y = -(spawneablePosition.cubeArea.y / 2) + spawneablePosition.cubeArea.y * Random.value;
            transform.position = spawneablePosition.transform.position + new Vector3(aux.x, 0, aux.y);
        }
        col.enabled = true;
    }

    protected override void InternalTakeDamage()
    {
        if (stuned)
        {
            TakeDamage(); return;
        }
        InterruptCast();
        Teleport();
    }

    protected override void InmuneFeedback(DamageData data)
    {
        base.InmuneFeedback(data);
        InterruptCast();
        Teleport();
    }

    protected override void InternalGetStunned()
    {
        base.InternalGetStunned();
        dmgReceiver.RemoveInvulnerability(Damagetype.All);
        animator.SetTrigger("Stunned");
    }

    private void OnDrawGizmosSelected()
    {
        if (!squaredRange)
            Gizmos.DrawWireSphere(transform.position, range);
        else
            Gizmos.DrawWireCube(transform.position, squareRange * 2);
    }

    protected override bool InternalCondition()
    {
        return true;
    }

    protected override void InternalEndCast()
    {
        feedback.StartGoToFeedback(myChar.transform, (x) => StunChar());
    }
}
