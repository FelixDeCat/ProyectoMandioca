using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ElectricSwordHolding : MonoBehaviour
{
    [Header("Fast")]
    //[SerializeField] int speed = 5;
    //[SerializeField] float lifeTime = 2;
    [SerializeField] float timeToCharge = 1.5f;

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
    [SerializeField] float explodeRadious = 10;
    [SerializeField] DamageData dmgData = null;
    [SerializeField] int damage = 30;
    const string spawnOrb = "SpawnOrb";
    const string spawnOrbPart = "SpawnOrbPart";
    [SerializeField] AudioClip _groundHit = null;
    [SerializeField] AudioClip _lightningStrike = null;
    private void Start()
    {
        dmgData.SetDamage(damage).SetDamageInfo(DamageInfo.NonBlockAndParry).SetDamageType(Damagetype.Explosion).SetKnockback(4500).Initialize(Main.instance.GetChar());
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
        //Llamar animevent que salga de disparar
    }

    public void OnEquip()
    {
        //Sonidos?
        myChar = Main.instance.GetChar();
        myChar.charAnimEvent.Add_Callback(spawnOrb, InstantiateOrb);
        GetComponent<EquipedItem>().CanUseCallback = CanUse;
    }

    public void OnEquipAux()
    {
        //Sonidos?
        GetComponent<EquipedItem>().CanUseCallback = CanUse;
        myChar = Main.instance.GetChar();
        myChar.charAnimEvent.Add_Callback(spawnOrb,DetonateOrb );
        myChar.charAnimEvent.Add_Callback(spawnOrbPart, InstantiateOrbPart);
        ParticlesManager.Instance.GetParticlePool(particlesLindasPre.name, particlesLindasPre);
        AudioManager.instance.GetSoundPool(_groundHit.name, AudioManager.SoundDimesion.TwoD, AudioGroups.GAME_FX, _groundHit);
        AudioManager.instance.GetSoundPool(_lightningStrike.name, AudioManager.SoundDimesion.TwoD, AudioGroups.GAME_FX, _lightningStrike);
    }
    public void UnEquip()
    {
        myChar.charAnimEvent.Remove_Callback(spawnOrb, InstantiateOrb);

        //Sonidos? quiza
    }

    public void UnEquipAux()
    {
        myChar.charAnimEvent.Remove_Callback(spawnOrb, DetonateOrb);
        myChar.charAnimEvent.Remove_Callback(spawnOrbPart, InstantiateOrbPart);

        //Sonidos? quiza
    }

    public void OnExecute(int charges)
    {
        //Aca deje lo de la habilidad porque sino crashea todo si no, a ver como solucionarlo
        //if (charges == 0)

        ExecuteShort();

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

    void InstantiateOrbPart()
    {
        ParticlesManager.Instance.PlayParticle(particlesLindasPre.name, myChar.transform.position);
    }

    void DetonateOrb()
    {
        Main.instance.GetChar().SwordAbilityRelease();
        var overlap = Physics.OverlapSphere(Main.instance.GetChar().transform.position, explodeRadious).Where(x => !x.GetComponent<CharacterHead>())
            .Where(x => x.GetComponent<DamageReceiver>()).Select(x => x.GetComponent<DamageReceiver>()).ToList();
        dmgData.SetPositionAndDirection(Main.instance.GetChar().transform.position);
        for (int i = 0; i < overlap.Count; i++)
            overlap[i].TakeDamage(dmgData);
        AudioManager.instance.PlaySound(_lightningStrike.name, transform);
        AudioManager.instance.PlaySound(_groundHit.name, transform);
    }

    bool CanUse() => myChar.CheckStateMachinInput(CharacterHead.PlayerInputs.START_ACTIVE);
}
