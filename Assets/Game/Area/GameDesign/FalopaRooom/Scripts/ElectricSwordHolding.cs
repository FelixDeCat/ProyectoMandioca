using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricSwordHolding : MonoBehaviour
{
    [Header("Fast")]
    [SerializeField] int speed = 5;
    [SerializeField] float lifeTime = 2;
    [SerializeField] float timeToCharge = 1.5f;
    [SerializeField] ChargeModule chargeModule = null;

    [Header("Orb")]
    [SerializeField] ElectricOrb electricOrb = null;
    [SerializeField] float orbSpeed = 2;
    [SerializeField] float orbLifeTime = 5;
    [SerializeField] ParticleSystem particlesLindasPre = null;

    CharacterHead myChar;
    [Header("Other")]
    [SerializeField] float charSpeed = 0;

    bool canUpdate = false;
    float timer = 0;
    const string spawnBullet = "SpawnBullet";
    const string spawnOrb = "SpawnOrb";
    const string spawnOrbPart = "SpawnOrbPart";
    [SerializeField] AudioClip _groundHit;
    [SerializeField] AudioClip _lightningStrike;
    private void Start()
    {
        //chargeModule.Subscribe_Feedback_OnRelease((x) => { if (x == 0) CancelExecute(); else ExecuteShort(); });
       

    }
    public void OnPress()
    {
        //Aca supongo que van cosas de feedback
        canUpdate = true;
        timer = 0;
        Main.instance.GetChar().SwordAbiltyCharge(charSpeed);
        myChar.charanim.SetLightnings(true);
    }
    public void OnStopUse()
    {
        //Aca tambien
        myChar.charanim.SetLightnings(false);
        canUpdate = false;
        //Llamar animevent que salga de disparar
    }

    public void OnUpdate()
    {
        if (!canUpdate) return;
        timer += Time.deltaTime;
    }

    public void OnEquip()
    {
        //Sonidos?
        myChar = Main.instance.GetChar();
        myChar.charAnimEvent.Add_Callback(spawnBullet, DetonateOrb);
        myChar.charAnimEvent.Add_Callback(spawnOrb, InstantiateOrb);
        GetComponent<EquipedItem>().CanUseCallback = CanUse;
    }

    public void OnEquipAux()
    {
        //Sonidos?
        GetComponent<EquipedItem>().CanUseCallback = CanUse;
        myChar = Main.instance.GetChar();
        myChar.charAnimEvent.Add_Callback(spawnBullet, InstantiateOrb);
        myChar.charAnimEvent.Add_Callback(spawnOrb,DetonateOrb );
        myChar.charAnimEvent.Add_Callback(spawnOrbPart, InstantiateOrbPart);
        ParticlesManager.Instance.GetParticlePool(particlesLindasPre.name, particlesLindasPre);
        AudioManager.instance.GetSoundPool(_groundHit.name, AudioManager.SoundDimesion.TwoD, AudioGroups.GAME_FX, _groundHit);
        AudioManager.instance.GetSoundPool(_lightningStrike.name, AudioManager.SoundDimesion.TwoD, AudioGroups.GAME_FX, _lightningStrike);
    }
    public void UnEquip()
    {
        myChar.charAnimEvent.Remove_Callback(spawnBullet, DetonateOrb);
        myChar.charAnimEvent.Remove_Callback(spawnOrb, InstantiateOrb);

        //Sonidos? quiza
    }

    public void UnEquipAux()
    {
        myChar.charAnimEvent.Remove_Callback(spawnBullet, InstantiateOrb);
        myChar.charAnimEvent.Remove_Callback(spawnOrb, DetonateOrb);
        myChar.charAnimEvent.Remove_Callback(spawnOrbPart, InstantiateOrbPart);

        //Sonidos? quiza
    }

    public void OnExecute(int charges)
    {
        //Aca deje lo de la habilidad porque sino crashea todo si no, a ver como solucionarlo
        //if (charges == 0)

        if (timer >= timeToCharge)
            ExecuteShort();
        else
            CancelExecute();

        timer = 0;

    }

    void CancelExecute()
    {
        myChar.charanim.CancelAttackAnimations();
        Main.instance.GetChar().SwordAbilityRelease();
    }
    void InstantiateOrb()
    {
        Main.instance.GetChar().SwordAbilityRelease();
        var orb = Instantiate(electricOrb);
        orb.SetSpeed(orbSpeed).SetLifeTime(orbLifeTime);
        orb.transform.forward = myChar.GetCharMove().GetRotatorDirection();
        orb.transform.position = myChar.transform.position + Vector3.up + orb.transform.forward;
        

    }

    void ExecuteShort()
    {

        myChar.charanim.ThrowLightningOrb();
    }

    void ExecuteLong()
    {
        myChar.charanim.ThrowLightningBullets();
        //Aca llamo al animator para que empiece a disparar
    }

    void InstantiateOrbPart()
    {
        ParticlesManager.Instance.PlayParticle(particlesLindasPre.name, myChar.transform.position);
    }

    void DetonateOrb()
    {
        Main.instance.GetChar().SwordAbilityRelease();
        var orb = Instantiate(electricOrb);
        orb.SetSpeed(orbSpeed).SetLifeTime(orbLifeTime);
        orb.transform.forward = myChar.GetCharMove().GetRotatorDirection();
        orb.transform.position = myChar.transform.position + Vector3.up + orb.transform.forward;
        StartCoroutine(AndaBienPls(orb));
        /*orb.OnInitialize();
        orb.Explode()*/
        AudioManager.instance.PlaySound(_lightningStrike.name, transform);
        AudioManager.instance.PlaySound(_groundHit.name, transform);
    }

    IEnumerator AndaBienPls(ElectricOrb orb)
    {
        yield return new WaitForEndOfFrame();
        orb.Explode();
    }

    public void OnEnd()
    {
    }

    bool CanUse() => myChar.CheckStateMachinInput(CharacterHead.PlayerInputs.START_ACTIVE);
}
