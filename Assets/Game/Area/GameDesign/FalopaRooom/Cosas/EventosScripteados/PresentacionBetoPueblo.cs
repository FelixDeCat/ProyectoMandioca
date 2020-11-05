using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresentacionBetoPueblo : MonoBehaviour, ISpawner
{
    [SerializeField] Transform betoRoot;

    [Header("Primera parte. Summon")]
    [SerializeField] SpawnerSpot spot = null;
    [SerializeField] PlayObject prefab = null;
    [SerializeField] TotemFeedback totemFeedback = new TotemFeedback();
    public int amountSummoned;
    int amountKilled;
    List<PlayObject> summonedEnemies = new List<PlayObject>();
    bool finishKillSummon;

    [Header("Segunda parte: Romper puente")]
    [SerializeField] Transform bridgePos;
    Transform currentPlaceToGo;
    [SerializeField] float betoSpeed;
    [SerializeField] float particleDelay; // este es el tiempo entre que le decis Play a la particula y se rompe el puente
    [SerializeField] ParticleSystem rayoQueRompePuente_cargando;//carga el rayo
    [SerializeField] ParticleSystem rayoQueRompePuente_impacto;//tira el rayo
    [SerializeField] EventDestructible puente;

    CharacterHead _hero;
    Animator _betoAnim;

    CDModule cdModule = new CDModule();

    private void Start()
    {
        _hero = Main.instance.GetChar();
        totemFeedback.Initialize(StartCoroutine);

        _betoAnim = betoRoot.GetComponentInChildren<Animator>();
        _betoAnim.Play("IdleFly");
        _betoAnim.SetInteger("heighLevel", 1);

        
    }

    public void ActivateBetoEvento()
    {
        _betoAnim.Play("StartCastStaff");
        totemFeedback.StartChargeFeedback(Summon);
    }

    //Primer parte: summon bichos

    void Summon()
    {
        for (int i = 0; i < amountSummoned; i++)
        {
            Vector3 pos = spot.GetSurfacePos(_hero.Root);

            totemFeedback.StartGoToFeedback(pos, (x) => SpawnPrefab(x));    
        }

        _betoAnim.SetTrigger("finishSkill");

        GoToPuente();

    }

    public void SpawnPrefab(Vector3 pos, string sceneName = null)
    {
        var newSpawn = spot.SpawnPrefab(pos, prefab, sceneName, this);

        newSpawn.GetComponent<EnemyBase>().OnDeath.AddListener(OnEnemydead);
        summonedEnemies.Add(newSpawn);
    }

    void OnEnemydead()
    {
        amountKilled++;
        if (amountKilled >= amountSummoned)
        {
            finishKillSummon = true;

            for (int i = 0; i < summonedEnemies.Count; i++)
            {
                summonedEnemies[i].GetComponent<EnemyBase>().OnDeath.RemoveListener(OnEnemydead);
            }

            summonedEnemies.Clear();
        }
    }

    public void ReturnObject(PlayObject newPrefab)
    {
        
    }

    //Segunda parte: romper puente

    void GoToPuente()
    {
        currentPlaceToGo = bridgePos;
    }

    void StartCastDestroyPuente()
    {
        _betoAnim.Play("StartCastOrb");
        if (rayoQueRompePuente_cargando != null) rayoQueRompePuente_cargando.Play();
        cdModule.AddCD("RomperPuente", DestroyPuente, particleDelay);
    }

    void DestroyPuente()
    {
        if (rayoQueRompePuente_impacto != null) rayoQueRompePuente_impacto.Play();
        _betoAnim.SetTrigger("finishSkill");
        puente.BreakYourselfBaby();
    }

    void Update()
    {
        cdModule.UpdateCD();

        if (currentPlaceToGo == null && finishKillSummon)
        {
            StartCastDestroyPuente();
            finishKillSummon = false;
            return;
        }

        if (currentPlaceToGo == null) return;

        var dir = (currentPlaceToGo.position - betoRoot.position).normalized;

        

        if(Vector3.Distance(betoRoot.position, currentPlaceToGo.position) >= .5f)
        {
            betoRoot.transform.position += dir * betoSpeed * Time.deltaTime;
        }
        else
        {
            betoRoot.transform.position = currentPlaceToGo.position;
            currentPlaceToGo = null;

           // StartCastDestroyPuente();
        }
    }
}
