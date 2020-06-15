using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillActive_LightBounce : SkillActivas
{
    [SerializeField] private int damage = 5;
    [SerializeField] private Damagetype dmgType = Damagetype.Fire;
    [SerializeField] private bool isTickDamage;
    [SerializeField] private float range = 6;
    [SerializeField] private GameObject lightAura = null;
    [SerializeField] private GameObject lightBeam = null;

   [SerializeField] private ParticleSystem sparks_ps = null;

    public float laserWidth = 0.1f;
    public float laserMaxLength = 5f;

    private CharacterHead _hero;
    private EntityBlock blocker;

    DamageData dmgData;      

    [SerializeField] private LineRenderer _lineRenderer = null;

    [SerializeField] private AudioClip fireClip;
    [SerializeField] private AudioClip celestialChorus;
    [SerializeField] private AudioClip pickUp_skill;
    private const string _fireSound = "fireSound";
    private const string _celestialChorus = "celestialChorus";



    protected override void OnStart()
    {
        base.OnStart();
        lightAura.SetActive(false);
        lightBeam.SetActive(false);

        dmgData = GetComponent<DamageData>();
        dmgData.Initialize(Main.instance.GetChar());
        dmgData.SetDamage(damage).SetDamageTick(isTickDamage).SetDamageType(dmgType).SetKnockback(0).SetPositionAndDirection(Main.instance.GetChar().transform.position);
    }
    protected override void OnOneShotExecute()
    {
        Debug.Log("OnOneSHot");
    }

    protected override void OnBeginSkill()
    {
        lightAura.SetActive(false);
        lightBeam.SetActive(false);
        _hero = Main.instance.GetChar();
        blocker = _hero.GetCharBlock();
        AudioManager.instance.GetSoundPool(_fireSound, AudioGroups.GAME_FX,fireClip, true);
        AudioManager.instance.GetSoundPool(_celestialChorus, AudioGroups.GAME_FX,celestialChorus);
        AudioManager.instance.GetSoundPool("pick_lightBounceSkill", AudioGroups.GAME_FX,pickUp_skill);
        
        AudioManager.instance.PlaySound("pick_lightBounceSkill");
    }
    protected override void OnEndSkill() { }

    protected override void OnUpdateSkill()
    {
        
    }

    void ShootLaserFromTargetPosition(Vector3 targetPosition, Vector3 direction, float length)
    {
        Ray ray = new Ray(targetPosition, direction);
        RaycastHit raycastHit;
        Vector3 endPosition = targetPosition + (length * direction);        

        if (Physics.Raycast(ray, out raycastHit, length))
        {
            var enemy = raycastHit.collider.gameObject.GetComponent<WalkingEntity>();

            if (enemy != null)
            {
                if (sparks_ps)
                {
                    sparks_ps.transform.position = raycastHit.point;
                    sparks_ps.Play();
                }
                Main.instance.Vibrate();
                
                enemy.OnPetrified();
                
            }
            else
            {
                if(sparks_ps)
                    sparks_ps.Stop();
                endPosition = raycastHit.point;
            }
        }
    }

    protected override void OnStartUse()
    {
        lightAura.SetActive(true);
        lightBeam.SetActive(true);
        AudioManager.instance.PlaySound(_celestialChorus);

    }

    protected override void OnStopUse()
    {
        if(sparks_ps)
            sparks_ps.Stop();
        lightAura.SetActive(false);
        lightBeam.SetActive(false);
        AudioManager.instance.StopAllSounds(_fireSound);
    }

    protected override void OnUpdateUse()
    {
        lightAura.transform.position = _hero.transform.position;
        lightBeam.transform.position = _hero.ShieldVectorDirection.position;
        lightBeam.transform.forward = _hero.ShieldVectorDirection.forward;

        if (blocker.onBlock)
        {
            lightBeam.SetActive(true);

            if (!AudioManager.instance.GetSoundPool(_fireSound).soundPoolPlaying)
            {
                AudioManager.instance.PlaySound(_fireSound);
            }
            
            ShootLaserFromTargetPosition(_hero.transform.position + Vector3.up * 1, _hero.GetCharMove().GetRotatorDirection(), range);
            
        }
        else
        {
            lightBeam.SetActive(false);
            if (AudioManager.instance.GetSoundPool(_fireSound).soundPoolPlaying)
            {
                AudioManager.instance.StopAllSounds(_fireSound);
            }
        }
        
    }

}
