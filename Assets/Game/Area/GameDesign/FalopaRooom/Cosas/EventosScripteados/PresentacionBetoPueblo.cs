using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresentacionBetoPueblo : MonoBehaviour, ISpawner
{
    [SerializeField] Transform betoRoot;

    [Header("Primera parte. Summon")]
    [SerializeField] SpawnerSpot spot = null;
    [SerializeField] PlayObject prefab = null;
    public int amountSummoned;
    [SerializeField] TotemFeedback totemFeedback = new TotemFeedback();

    [Header("Segunda parte. Summon")]
    [SerializeField] Transform bridgePos;
    Transform currentPlaceToGo;
    [SerializeField] float betoSpeed;
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
        spot.SpawnPrefab(pos, prefab, sceneName, this);

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
        cdModule.AddCD("RomperPuente", DestroyPuente, 5);
    }

    void DestroyPuente()
    {
        _betoAnim.SetTrigger("finishSkill");
        puente.BreakYourselfBaby();
    }

    void Update()
    {
        cdModule.UpdateCD();

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

            StartCastDestroyPuente();
        }
    }
}
