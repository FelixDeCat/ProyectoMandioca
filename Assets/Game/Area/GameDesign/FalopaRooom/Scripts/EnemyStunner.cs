using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStunner : WalkingEntity
{
    [SerializeField] private float TimeBetweenCasts;

    [SerializeField] private float range;

    [SerializeField] private float stunDuration;
    [SerializeField] private float castTime;

    [SerializeField] CombatArea spawneablePosition;
    CombatArea area;

    CastingBar castingBar;
    EnemyLifeSystem life;
    DamageReceiver damageReceiver;
    Rigidbody rb;

    float castTimer = 0f;
    float durationTimer = 0f;

    CharacterHead myChar;

    void Start()
    {
        OnInitialize();
    }
    private void Update()
    {
        if (isCasting && Vector3.Distance(myChar.transform.position, transform.position) > range)
            castingBar.InterruptCasting();

    }
    protected override void OnInitialize()
    {
        myChar = Main.instance.GetChar();
        rb = GetComponent<Rigidbody>();
        area = GetComponentInParent<CombatArea>();
        life = GetComponent<EnemyLifeSystem>();
        castingBar = GetComponent<CastingBar>();
        damageReceiver = GetComponent<DamageReceiver>();
        castTimer = TimeBetweenCasts;

        life.Initialize(life.life, () => { }, () => { }, () => { });

        damageReceiver.Initialize(
            transform,
            () => { return false; },
            (x) => { Die(); },
            (x) => { TakeDamage(); },
            rb,
            life
            );
        StartCoroutine(Stun());

        castingBar.AddEventListener_OnStartCasting( ()=> IsCasting(true) );

        castingBar.AddEventListener_OnInterruptCasting(StartStun);
        castingBar.AddEventListener_OnInterruptCasting(() => IsCasting(false));

        castingBar.AddEventListener_OnFinishCasting(StunChar);
        castingBar.AddEventListener_OnFinishCasting(() => IsCasting(false));
    }
    
    void StartStun()
    {
        StartCoroutine(Stun());
    }

    IEnumerator Stun()
    {
        yield return new WaitForSeconds(TimeBetweenCasts);
        while (Vector3.Distance(transform.position, myChar.transform.position) > range)
            yield return new WaitForSeconds(0.1f);
        castingBar.StartCasting();
    }

    bool isCasting = false;
    void IsCasting(bool casting)
    {
        isCasting = casting;
    }

    void StunChar()
    {
        myChar.GetStunned(stunDuration);
        StartStun();
    }

    void TakeDamage()
    {
        castingBar.InterruptCasting();
        Vector2 aux;
        if(spawneablePosition.isCircle)
        {
            float ang = Random.Range(0,360);

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
    }

    void Die()
    {
        Debug.Log("Die");
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, range);
    }
       
    protected override void OnUpdateEntity() {}
    protected override void OnTurnOn()
    {
    }
    protected override void OnTurnOff()
    {
    }
    protected override void OnFixedUpdate()
    {
    }
    protected override void OnPause()
    {
    }
    protected override void OnResume()
    {
    }
}
