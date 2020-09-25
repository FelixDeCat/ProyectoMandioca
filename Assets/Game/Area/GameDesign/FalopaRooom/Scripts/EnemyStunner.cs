using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStunner : WalkingEntity
{
    [SerializeField] private float TimeBetweenCasts = 0.5f;

    [SerializeField] private float range = 40;

    [SerializeField] private float stunDuration = 10;

    [SerializeField] Vector3 particleOffset = new Vector3(0.25f, 0, 0.065f);
    [SerializeField] ParticleSystem onRootParticles = null;

    [SerializeField] Transform spawnPointsParent = null;
    [SerializeField] Transform[] spawnPoints = new Transform[1];
    int lastTP = -1;

    CastingBar castingBar;
    EnemyLifeSystem life;
    DamageReceiver damageReceiver;
    Rigidbody rb;

    CharacterHead myChar;
    EffectReceiver charEffect;

    EnemyStunnerStates currentState = EnemyStunnerStates.Idle;

    void Start()
    {
        OnInitialize();
    }
    private void Update()
    {
        if (currentState == EnemyStunnerStates.Casting && Vector3.Distance(myChar.transform.position, transform.position) > range)
        {
            castingBar.InterruptCasting();
            currentState = EnemyStunnerStates.Idle;
        }
    }
    protected override void OnInitialize()
    {
        myChar = Main.instance.GetChar();
        charEffect = myChar.GetComponent<EffectReceiver>();
        rb = GetComponent<Rigidbody>();
        life = GetComponent<EnemyLifeSystem>();
        castingBar = GetComponent<CastingBar>();
        damageReceiver = GetComponent<DamageReceiver>();

        life.Initialize(life.life, () => { }, () => { }, () => { });

        damageReceiver.AddDead((x) => Die()).AddTakeDamage((x) => TakeDamage()).Initialize(transform, rb, life);
        StartCoroutine(StartCastStun());

        castingBar.AddEventListener_OnStartCasting(() => ChangeState(EnemyStunnerStates.Casting));

        castingBar.AddEventListener_OnInterruptCasting(StartStun);        

        castingBar.AddEventListener_OnFinishCasting(StunChar);
        castingBar.AddEventListener_OnFinishCasting(() => ChangeState(EnemyStunnerStates.Idle));

        spawnPoints = spawnPointsParent.GetComponentsInChildren<Transform>();
    }


    void ChangeState(EnemyStunnerStates state)
    {
        currentState = state;
    }

    void StartStun()
    {
        StartCoroutine(StartCastStun());
    }

    IEnumerator StartCastStun()
    {
        yield return new WaitForSeconds(TimeBetweenCasts);
        while (Vector3.Distance(transform.position, myChar.transform.position) > range || currentState != EnemyStunnerStates.Idle)
            yield return new WaitForSeconds(0.1f);
        castingBar.StartCasting();  
    }

    void StunChar()
    {
        charEffect.TakeEffect(EffectName.OnRoot, stunDuration);

        onRootParticles.transform.position = myChar.transform.position + particleOffset.x * myChar.transform.forward + particleOffset.z * myChar.transform.right;
        onRootParticles.Play();

        StartStun();
    }

    public void GetStunned()
    {
        if(currentState == EnemyStunnerStates.Casting)
        {
            castingBar.InterruptCasting();
            ChangeState(EnemyStunnerStates.Stunned);
        }
    }
    public void EndStun()
    {
        if (currentState == EnemyStunnerStates.Stunned)
        {
            ChangeState(EnemyStunnerStates.Idle);
            Teleport();
        }
    }

    void Teleport()
    {
        /*Vector2 aux;
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
        }*/
        int aux = Random.Range(0, spawnPoints.Length);
        while(lastTP == aux && spawnPoints.Length > 1)
        {
            aux = Random.Range(0, spawnPoints.Length);
        }
        transform.position = spawnPoints[aux].transform.position;
        lastTP = aux;

    }

    void TakeDamage()
    {
        if (currentState == EnemyStunnerStates.Stunned) return;

        ChangeState(EnemyStunnerStates.Idle);
        castingBar.InterruptCasting();
        Teleport();
    }

    void Die()
    {
        Debug.Log("Die");
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, range);
    }

    protected override void OnUpdateEntity() { }
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

public enum EnemyStunnerStates
{
    Idle, Casting, Stunned
}
