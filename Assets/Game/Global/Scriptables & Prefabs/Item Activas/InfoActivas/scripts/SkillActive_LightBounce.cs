using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillActive_LightBounce : SkillActivas
{
    [SerializeField] private int damage = 5;
    [SerializeField] private Damagetype dmgType = Damagetype.Fire;
    [SerializeField] private float range = 6;
    [SerializeField] private GameObject lightBeam = null;
    [SerializeField] private ParticleSystem sparks_ps = null;

    public float laserWidth = 0.1f;
    public float laserMaxLength = 5f;

    private CharacterHead _hero;
    private EntityBlock blocker;

    [SerializeField] private LineRenderer _lineRenderer = null;

    [SerializeField] private AudioClip fireClip;
    [SerializeField] private AudioClip celestialChorus;
    private const string _fireSound = "fireSound";
    private const string _celestialChorus = "celestialChorus";


    protected override void OnStart()
    {
        base.OnStart();
        lightBeam.SetActive(false);
    }
    protected override void OnOneShotExecute()
    {
        Debug.Log("OnOneSHot");
    }

    protected override void OnBeginSkill()
    {
        lightBeam.SetActive(false);
        _hero = Main.instance.GetChar();
        blocker = _hero.GetCharBlock();
        AudioManager.instance.GetSoundPool(_fireSound, AudioGroups.GAME_FX,fireClip, true);
        AudioManager.instance.GetSoundPool(_celestialChorus, AudioGroups.GAME_FX,celestialChorus);
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
            var enemy = raycastHit.collider.gameObject.GetComponent<EnemyBase>();

            if (enemy != null)
            {
                sparks_ps.transform.position = raycastHit.point;
                sparks_ps.Play();
                Main.instance.Vibrate();
                
                enemy.TakeDamage(damage, _hero.transform.position, dmgType, _hero);
            }
            else
            {
                sparks_ps.Stop();
                endPosition = raycastHit.point;
            }
        }

        _lineRenderer.SetPosition(0, targetPosition);
        _lineRenderer.SetPosition(1, endPosition);
    }

    protected override void OnStartUse()
    {
        lightBeam.SetActive(true);
        AudioManager.instance.PlaySound(_celestialChorus);

    }

    protected override void OnStopUse()
    {
        _lineRenderer.enabled = false;
        sparks_ps.Stop();
        lightBeam.SetActive(false);
        AudioManager.instance.StopAllSounds(_fireSound);
    }

    protected override void OnUpdateUse()
    {
        lightBeam.transform.position = _hero.transform.position;
        if (blocker.onBlock)
        {
            if (!AudioManager.instance.GetSoundPool(_fireSound).soundPoolPlaying)
            {
                AudioManager.instance.PlaySound(_fireSound);
            }
            
            ShootLaserFromTargetPosition(_hero.transform.position + Vector3.up * 1, _hero.GetCharMove().GetRotatorDirection(), range);
            
            _lineRenderer.enabled = true;    
        }
        else
        {
            if (AudioManager.instance.GetSoundPool(_fireSound).soundPoolPlaying)
            {
                AudioManager.instance.StopAllSounds(_fireSound);
            }
            _lineRenderer.enabled = false;
        }
        
    }

}
