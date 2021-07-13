using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhantomSkill : BossSkills
{
    ThrowData data = new ThrowData();
    [SerializeField] ParticleSystem appearParticle = null;
    [SerializeField] ParticleSystem shootParticle = null;
    [SerializeField] BossProjectile projectile = null;
    [SerializeField] Transform model = null;
    [SerializeField] float projectileSpeed = 2;
    [SerializeField] int projectileDamage = 4;
    [SerializeField] int itteration = 6;
    [SerializeField] float timeToItteration = 1;
    [SerializeField] float distanceToChar = 5;
    [SerializeField] Animator anim = null;
    [SerializeField] AnimEvent animEvent = null;
    [SerializeField] Transform shootPoint = null;
    [SerializeField] LayerMask wallMask = 1 << 0;

    [SerializeField] AudioClip shootSound = null;
    Transform target;
    Vector3 firstPos;

    List<string> probPosition;
    string lastPosition = "Forward";

    public override void Initialize()
    {
        base.Initialize();
        AudioManager.instance.GetSoundPool(shootSound.name, AudioManager.SoundDimesion.ThreeD, AudioGroups.GAME_FX, shootSound);
        data.Owner = model;
        data.Force = projectileSpeed;
        data.Damage = projectileDamage;
        ThrowablePoolsManager.instance.CreateAPool(projectile.name, projectile);
        ParticlesManager.Instance.GetParticlePool(appearParticle.name, appearParticle);
        ParticlesManager.Instance.GetParticlePool(shootParticle.name, shootParticle);
        target = Main.instance.GetChar().transform;
        probPosition = new List<string>() { "Right", "Left", "Backward", "Forward" };
    }

    protected override void OnUseSkill()
    {
        animEvent.Add_Callback("Shoot", Shoot);
        firstPos = model.position;
        Itteration(itteration, timeToItteration, PhantomShoot, OverSkill);
    }

    void Shoot()
    {
        AudioManager.instance.PlaySound(shootSound.name, shootPoint);
        data.Position = shootPoint.position;
        data.Direction = model.forward;
        ParticlesManager.Instance.PlayParticle(shootParticle.name, shootPoint.position);
        ThrowablePoolsManager.instance.Throw(projectile.name, data);
    }

    void PhantomShoot()
    {
        if(probPosition.Contains(lastPosition)) probPosition.Remove(lastPosition);
        int index = Random.Range(0, probPosition.Count);
        model.position = PosSwitcher(probPosition[index]);
        probPosition.Add(lastPosition);
        lastPosition = probPosition[index];
        ParticlesManager.Instance.PlayParticle(appearParticle.name, model.position);
        model.forward = (target.position - model.position).normalized;
        anim.Play("PhantomShoot");
    }

    protected override void OnInterruptSkill()
    {

    }

    protected override void OnOverSkill()
    {
        animEvent.Remove_Callback("Shoot", Shoot);
        model.position = firstPos;
        ParticlesManager.Instance.PlayParticle(appearParticle.name, model.position);
        anim.Play("Idle");
    }

    Vector3 PosSwitcher(string posName)
    {
        Vector3 posResult = Vector3.zero;
        if (posName == "Right") posResult = target.right;
        else if (posName == "Forward") posResult = target.forward;
        else if (posName == "Left") posResult = -target.right;
        else if (posName == "Backward") posResult = -target.forward;

        posResult = ConfirmDir(posResult);

        return posResult;
    }

    Vector3 ConfirmDir(Vector3 dir)
    {
        if (Physics.Raycast(model.position, dir, distanceToChar, wallMask))
        {
            return target.position - dir * distanceToChar;
        }
        else
            return target.position + dir * distanceToChar;
    }
}
